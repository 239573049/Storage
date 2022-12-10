using System.Collections.Concurrent;
using DokanNet;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Storage.Host.Options;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Text;
using Storage.Host.Caches;
using System.Drawing;
using System;
using System.Diagnostics;

namespace Storage.Host.Storage;

public class MinioService : IStorageService, IDisposable
{
    private bool disposable;
    private readonly ConcurrentDictionary<string, MinioWriteCache> _writeCache = new();

    private readonly MinioClient _client;
    private readonly MinioOptions _minio;
    private readonly MemoryCaching _memoryCaching;

    public MinioService(IOptions<MinioOptions> minio, MemoryCaching memoryCaching)
    {
        _memoryCaching = memoryCaching;
        _minio = minio.Value;
        _client = new MinioClient()
            .WithEndpoint(_minio.Endpoint, _minio.Port)
            .WithCredentials(_minio.AccessKey, _minio.SecretKey)
            .Build();

        //判断桶是否存在，如果不存在则创建桶，否则上传文件会异常
        var bucketExistsArgs = new BucketExistsArgs();
        bucketExistsArgs.WithBucket(_minio.BucketName);

        if (!_client.BucketExistsAsync(bucketExistsArgs).GetAwaiter().GetResult())
        {
            var makeBucketArgs = new MakeBucketArgs();
            makeBucketArgs.WithBucket(_minio.BucketName);
            _client.MakeBucketAsync(makeBucketArgs).GetAwaiter().GetResult();
        }

        _ = Task.Factory.StartNew(StartWriteCache);
    }

    private async void StartWriteCache()
    {
        while (!disposable)
        {
            await Task.Delay(3000);
            var now = DateTime.Now;
            try
            {
                foreach (var t in _writeCache.Where(x => x.Value.UpdateTime.AddSeconds(5) < now))
                {
                    if (_writeCache.TryRemove(t.Key, out var writeCache))
                    {
                        // 当tag为空时数据是不适合切片的所以需要单独上传
                        if (!writeCache.Etags.Any())
                        {
                            var memoryStream = new MemoryStream(writeCache.MemoryStream.ToArray());
                            var args = new PutObjectArgs()
                                .WithBucket(_minio.BucketName)
                                .WithObject(writeCache.FileName)
                                .WithStreamData(memoryStream)
                                .WithObjectSize(writeCache.MemoryStream.Length);
                            await _client.PutObjectAsync(args);
                            return;
                        }

                        WriteCache(writeCache!, GetPutObject(writeCache!.FileName, writeCache.MemoryStream.Length),
                            false);

                        var completeMultipartUploadArgs = new CompleteMultipartUploadArgs()
                            .WithBucket(_minio.BucketName)
                            .WithObject(writeCache.FileName)
                            .WithUploadId(writeCache.FileName)
                            .WithETags(writeCache.Etags);

                        await _client.CompleteMultipartUploadAsync(completeMultipartUploadArgs, CancellationToken.None)
                            .ConfigureAwait(false);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }

    private void Info(string? message, params object?[] args)
    {
#if DEBUG
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(message, args);
#endif
    }

    private void Error(string? message, params object?[] args)
    {
#if DEBUG
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message, args);
#endif
    }

    public void GetPath(ref string fileName)
    {
        if (fileName != "/")
        {
            fileName = fileName.Replace('\\', '/').TrimStart('/');
        }
    }

    public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(DeleteFile), fileName, info.IsDirectory);

        if (!ExistFile(fileName))
            return NtStatus.Error;

        var o = new RemoveObjectArgs();
        o.WithBucket(_minio.BucketName);
        o.WithObject(fileName);
        _client.RemoveObjectAsync(o).GetAwaiter().GetResult();

        return NtStatus.Success;
    }

    public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(DeleteDirectory), fileName, info.IsDirectory);

        fileName += "/";
        if (ExistDirectory(fileName))
        {
            return NtStatus.DirectoryNotEmpty;
        }

        var o = new ListObjectsArgs();
        o.WithPrefix(fileName);
        o.WithBucket(_minio.BucketName);
        o.WithRecursive(true);
        foreach (var x in _client.ListObjectsAsync(o).ToList().GetAwaiter().GetResult())
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_minio.BucketName)
                .WithObject(x.Key);

            _client.RemoveObjectAsync(args).GetAwaiter().GetResult();
        }

        return NtStatus.Success;
    }

    public bool MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
    {
        GetPath(ref oldName);
        GetPath(ref newName);

        Info("{0}=> oldName:{1}  newName:{2}  IsDirectory:{3}", nameof(MoveFile), oldName, newName, info.IsDirectory);
        var removeObjectArgs = new RemoveObjectArgs();
        removeObjectArgs.WithBucket(_minio.BucketName);
        removeObjectArgs.WithObject(oldName);

        try
        {
            var cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket(_minio.BucketName)
                .WithObject(oldName)
                .WithCopyConditions(null)
                .WithServerSideEncryption(null);
            var args = new CopyObjectArgs()
                .WithBucket(_minio.BucketName)
                .WithObject(newName)
                .WithCopyObjectSource(cpSrcArgs)
                .WithHeaders(null)
                .WithServerSideEncryption(null);

            _client.CopyObjectAsync(args).GetAwaiter().GetResult();
            _client.RemoveObjectAsync(removeObjectArgs).GetAwaiter().GetResult();

            return true;
        }
        catch (Exception exception)
        {
            Error("MoveFile exception :{0} oldName:{1} newName：{newName}", exception, oldName, newName);
            return false;
        }
    }

    public bool MoveDirectory(string path, string dest)
    {
        GetPath(ref path);
        GetPath(ref dest);

        var cpSrcArgs = new CopySourceObjectArgs()
            .WithBucket(_minio.BucketName)
            .WithObject(path + '/')
            .WithCopyConditions(null)
            .WithServerSideEncryption(null);

        var args = new CopyObjectArgs()
            .WithBucket(_minio.BucketName)
            .WithObject(dest + '/')
            .WithCopyObjectSource(cpSrcArgs)
            .WithHeaders(null)
            .WithServerSideEncryption(null);

        var removeObjectArgs = new RemoveObjectArgs();
        removeObjectArgs.WithBucket(_minio.BucketName);
        removeObjectArgs.WithObject(path + '/');

        try
        {
            _client.CopyObjectAsync(args).GetAwaiter().GetResult();
            _client.RemoveObjectAsync(removeObjectArgs).GetAwaiter().GetResult();
        }
        catch
        {
            return false;
        }

        return true;
    }

    public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        // 根目录单独处理
        if (string.IsNullOrEmpty(fileName))
        {
            fileInfo = new FileInformation
            {
                Attributes = FileAttributes.Directory,
                FileName = fileName,
                Length = 0
            };
            return DokanResult.Success;
        }


        var fileInformation = _memoryCaching.GetFileInformation(fileName);
        if (fileInformation == null)
        {
            Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(GetFileInformation), fileName, info.IsDirectory);
            var o = new ListObjectsArgs();
            o.WithBucket(_minio.BucketName);
            o.WithPrefix(fileName);
            o.WithRecursive(false);

            Item? data = null;
            try
            {
                data = _client.ListObjectsAsync(o).GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                Error("GetFileInformation exception :{0} fileName:{1} ", exception, fileName);
            }

            if (data == null)
            {
                fileInfo = new FileInformation();
                return DokanResult.FileNotFound;
            }

            fileInfo = new FileInformation
            {
                Attributes = data.IsDir ? FileAttributes.Directory : FileAttributes.Normal,
                LastWriteTime = data.LastModifiedDateTime,
                FileName = data.Key.TrimEnd('/'),
                CreationTime = data.LastModifiedDateTime,
                Length = (long)data.Size
            };
            _memoryCaching.AddFileInformation(fileName, fileInfo);
        }
        else
        {
            fileInfo = (FileInformation)fileInformation;
        }

        return DokanResult.Success;
    }

    public NtStatus FindFiles(string filePath, string searchPattern, out IList<FileInformation> files)
    {
        GetPath(ref filePath);


        var info = _memoryCaching.GetFileInformationList(filePath);
        if (info == null)
        {
            Info("{0}=> filePath:{1}  searchPattern:{2}", nameof(FindFiles), filePath, searchPattern);
            var o = new ListObjectsArgs();
            o.WithBucket(_minio.BucketName);

            var name = filePath + "/" + (searchPattern == "*" ? "" : searchPattern);
            name = name.EndsWith('/') ? name : name + '/';

            o.WithPrefix(name);

            var list = _client.ListObjectsAsync(o)
                .ToList()
                .GetAwaiter()
                .GetResult().Select(x => new FileInformation()
                {
                    Attributes = x.IsDir ? FileAttributes.Directory : FileAttributes.Normal,
                    LastWriteTime = x.LastModifiedDateTime,
                    FileName = TrimStart(x.Key.TrimEnd('/'), name),
                    CreationTime = x.LastModifiedDateTime,
                    Length = (long)x.Size
                }).Where(x => x.FileName?.EndsWith(".desktop") == false).ToList();
            _memoryCaching.AddFileInformationList(filePath, list);
            files = list;
        }
        else
        {
            files = info;
        }

        return NtStatus.Success;
    }


    // 设置上传切片大小
    const int ReadSize = 1024 * 1024 * 5;

    public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        var result = _readCaches.TryGetValue(fileName, out var cache);
        if (result == false)
        {
            cache = new ReadCache((int)offset);
            _readCaches.TryAdd(fileName, cache);
        }
        else if (offset >= cache?.Offset && cache!.Buffer.Length - cache.Offset > buffer.Length) // 当需要缓存长度大于需要的长度时进入
        {

            if (offset - cache.Offset < cache.Buffer.Length)
            {
                Info("{0}=> filePath:{1} 缓存 buffer:{2}  offset:{3}", nameof(ReadFile), fileName, buffer.Length, offset);
                Array.Copy(cache.Buffer, offset - cache.Offset, buffer, 0, buffer.Length);
                bytesRead = buffer.Length;
                return NtStatus.Success;
            }
        }

        Info("{0}=> filePath:{1}  buffer:{2}  offset:{3}", nameof(ReadFile), fileName, buffer.Length, offset);

        var obj = new GetObjectArgs();
        obj.WithBucket(_minio.BucketName);
        obj.WithObject(fileName);
        // 当buffer大于切片大小直接使用buffer长度
        obj.WithOffsetAndLength(offset, buffer.Length > ReadSize ? buffer.Length : ReadSize);
        var read = 0;
        obj.WithCallbackStream(stream =>
        {
            using var m = new MemoryStream();
            cache.Offset = (int)offset; // offset 记录第一次偏移值
            stream.CopyTo(m);
            cache.Buffer = m.ToArray();
        });

        try
        {
            _client.GetObjectAsync(obj).GetAwaiter().GetResult();
            bytesRead = buffer.Length;
            Array.Copy(cache.Buffer, cache.Offset - offset, buffer, 0,
                buffer.Length > cache.Buffer.Length ? cache.Buffer.Length : buffer.Length);
        }
        catch (Exception exception)
        {
            bytesRead = 0;
        }

        //if (!_readCaches.TryGetValue(fileName, out var cache))
        //{
        //    cache = new ReadCache(offset, buffer.Length < size ? null : buffer.Length);
        //    _readCaches.TryAdd(fileName, cache);

        //    var obj = new GetObjectArgs();
        //    obj.WithBucket(_minio.BucketName);
        //    obj.WithObject(fileName);
        //    obj.WithOffsetAndLength(offset, 1024 * 1024 * 5);
        //    var read = 0;
        //    obj.WithCallbackStream(stream =>
        //    {
        //        var m = new MemoryStream();
        //        stream.CopyTo(m);
        //        cache.Buffer = m.ToArray();
        //        cache.Buffer = m.GetBuffer();
        //        read = (int)cache.Buffer.Length;
        //    });

        //    try
        //    {
        //        _client.GetObjectAsync(obj).GetAwaiter().GetResult();
        //        bytesRead = 0;

        //    }
        //    catch (Exception exception)
        //    {
        //        Error("ReadFile exception :{0} fileName:{1} ", exception, fileName);
        //        bytesRead = 0;
        //    }

        //    cache.Buffer.ToArray();
        //    if (cache.Buffer.Length <= 0)
        //    {
        //        _readCaches.TryRemove(fileName, out _);
        //    }
        //}
        //else
        //{
        //    var obj = new GetObjectArgs();
        //    obj.WithBucket(_minio.BucketName);
        //    obj.WithObject(fileName);
        //    obj.WithOffsetAndLength(offset, 1024 * 1024 * 5);
        //    var read = 0;
        //    obj.WithCallbackStream(stream =>
        //    {
        //        var m = new MemoryStream();
        //        stream.CopyTo(m);
        //        cache.Buffer = m.ToArray();
        //        read = (int)cache.Buffer.Length;
        //    });

        //    try
        //    {
        //        _client.GetObjectAsync(obj).GetAwaiter().GetResult();
        //        bytesRead = 0;

        //    }
        //    catch (Exception exception)
        //    {
        //        Error("ReadFile exception :{0} fileName:{1} ", exception, fileName);
        //        bytesRead = 0;
        //    }
        //    Array.Copy(cache.Buffer, cache.Offset, buffer, 0, buffer.Length);
        //    cache.Offset = buffer.Length;
        //    bytesRead = buffer.Length;
        //    if (cache.Buffer.Length <= 0)
        //    {
        //        _readCaches.TryRemove(fileName, out _);
        //    }
        //}


        return NtStatus.Success;
    }

    private readonly ConcurrentDictionary<string, ReadCache>
        _readCaches = new();

    class ReadCache
    {
        public int Offset { get; set; }

        public byte[] Buffer { get; set; }

        public ReadCache(int offset)
        {
            Offset = offset;
            Buffer = Array.Empty<byte>();
        }
    }

    public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
    {
        GetPath(ref fileName);
        //Info("{0}=> fileName:{1}  buffer:{2} offset :{3}", nameof(WriteFile), fileName, buffer.Length, offset);
        bytesWritten = 0;

        var put = GetPutObject(fileName, buffer.Length);

        if (offset == 0)
        {
            if (buffer.Length < ReadSize)
            {
                var memoryStream = new MemoryStream();
                memoryStream.Write(buffer);
                var writeCache = new MinioWriteCache
                {
                    FileName = fileName,
                    MemoryStream = memoryStream,
                    UpdateTime = DateTime.Now,
                    Etags = new Dictionary<int, string>()
                };
                _writeCache.TryAdd(fileName, writeCache);
            }
            else
            {
                var multipartUploadArgs = new NewMultipartUploadPutArgs()
                    .WithBucket(_minio.BucketName)
                    .WithContentType(null)
                    .WithObject(fileName);

                var uploadId = _client.NewMultipartUploadAsync(multipartUploadArgs).GetAwaiter().GetResult();
                var putObjectArgs = new PutObjectArgs(put)
                    .WithRequestBody(buffer)
                    .WithUploadId(uploadId)
                    .WithPartNumber(1);

                var etag = _client.PutObjectSinglePartAsync(putObjectArgs).GetAwaiter().GetResult();

                var writeCache = new MinioWriteCache
                {
                    UploadId = uploadId,
                    FileName = fileName,
                    UpdateTime = DateTime.Now,
                    Etags =
                    {
                        [1] = etag
                    }
                };
                _writeCache.TryAdd(fileName, writeCache);
            }
        }
        else
        {
            _writeCache.TryGetValue(fileName, out var writeCache);

            writeCache!.MemoryStream.Write(buffer);
            writeCache.UpdateTime = DateTime.Now;
            WriteCache(writeCache, put);
        }

        bytesWritten = buffer.Length;

        return NtStatus.Success;
    }

    private void WriteCache(MinioWriteCache writeCache, PutObjectPartArgs put, bool newMemory = true)
    {
        if (writeCache.MemoryStream.Length >= ReadSize)
        {
            Console.WriteLine("WriteCache size:{0}", writeCache.MemoryStream.Length);
            var s = Stopwatch.StartNew();
            writeCache!.PartNumber += 1;
            if (string.IsNullOrEmpty(writeCache.UploadId))
            {
                var multipartUploadArgs = new NewMultipartUploadPutArgs()
                    .WithBucket(_minio.BucketName)
                    .WithContentType(null)
                    .WithObject(writeCache.FileName);

                writeCache.UploadId = _client.NewMultipartUploadAsync(multipartUploadArgs).GetAwaiter().GetResult();
            }

            var putObjectArgs = new PutObjectArgs(put)
                .WithRequestBody(writeCache.MemoryStream.ToArray())
                .WithUploadId(writeCache.UploadId)
                .WithObjectSize(writeCache.MemoryStream.Length)
                .WithPartNumber(writeCache.PartNumber);

            s.Stop();
            Info("{0}=> 耗时1：{1}ms", nameof(WriteCache), s.ElapsedMilliseconds);
            s = Stopwatch.StartNew();
            var etag = _client.PutObjectSinglePartAsync(putObjectArgs).GetAwaiter().GetResult();
            writeCache.Etags[writeCache.PartNumber] = etag;
            writeCache.UpdateTime = DateTime.Now;

            writeCache.MemoryStream.Close();
            s.Stop();
            Info("{0}=> 耗时2：{1}ms", nameof(WriteCache), s.ElapsedMilliseconds);
            if (newMemory)
            {
                writeCache.MemoryStream = new MemoryStream();
            }
        }
    }

    private PutObjectPartArgs GetPutObject(string fileName, long length)
    {
        var put = new PutObjectPartArgs();

        put.WithBucket(_minio.BucketName)
            .WithObject(fileName)
            .WithObjectSize(length);

        return put;
    }


    public bool CreateFile(string path)
    {
        GetPath(ref path);

        Info("{0}=> path:{1} ", nameof(CreateFile), path);

        if (ExistFile(path)) return false;

        var millstream = new MemoryStream(Encoding.UTF8.GetBytes(" "));
        var put = new PutObjectArgs();
        put.WithBucket(_minio.BucketName);
        put.WithObjectSize(millstream.Length);
        put.WithStreamData(millstream);
        put.WithObject(path);
        put.WithContentType("application/octet-stream");

        _client.PutObjectAsync(put).GetAwaiter().GetResult();

        return true;
    }

    public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections,
        IDokanFileInfo info)
    {
        security = null;
        return DokanResult.NotImplemented;
    }

    public bool ExistFile(string path)
    {
        GetPath(ref path);
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        if (path.EndsWith("HEAD"))
        {
            return false;
        }


        var item = _memoryCaching.GetExistFile(path);
        if (item == null)
        {
            Info("{0}=> path:{1} ", nameof(ExistFile), path);
            var o = new ListObjectsArgs();
            o.WithBucket(_minio.BucketName);
            o.WithPrefix(path);
            try
            {
                var data = _client.ListObjectsAsync(o).GetAwaiter().GetResult();
                _memoryCaching.AddExistFile(path, data);
                return data?.IsDir == false;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return item.IsDir == false;
        }
    }

    public bool CreateDirectory(string path)
    {
        GetPath(ref path);


        var stream = new MemoryStream(Encoding.UTF8.GetBytes("desktop"));

        try
        {
            Info("{0}=> path:{1} ", nameof(CreateDirectory), path);
            var put = new PutObjectArgs();
            put.WithObjectSize(stream.Length);
            put.WithStreamData(stream);
            put.WithBucket(_minio.BucketName);
            put.WithObject(path + "/.desktop");
            put.WithContentType("application/octet-stream");
            _client.PutObjectAsync(put).GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            Error("CreateDirectory Exception :{0} path:{1} ", exception, path);
            return false;
        }

        return true;
    }

    public void GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes,
        out long totalNumberOfFreeBytes,
        IDokanFileInfo info)
    {
        totalNumberOfBytes = (long)1024 * 1024 * 1024 * 1024;
        freeBytesAvailable = Convert.ToInt64(0.95 * totalNumberOfBytes);
        totalNumberOfFreeBytes =
            Convert.ToInt64(0.95 * totalNumberOfBytes *
                            0.95);
    }

    public void GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName,
        out uint maximumComponentLength, IDokanFileInfo info)
    {
        volumeLabel = _minio.VolumeLabel;
        fileSystemName = "NTFS";
        maximumComponentLength = 256;
        features = FileSystemFeatures.CasePreservedNames | FileSystemFeatures.CaseSensitiveSearch |
                   FileSystemFeatures.PersistentAcls | FileSystemFeatures.SupportsRemoteStorage |
                   FileSystemFeatures.UnicodeOnDisk;
    }

    public async void SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime,
        DateTime? lastWriteTime,
        IDokanFileInfo info)
    {
        GetPath(ref fileName);

        if (_writeCache.Remove(fileName, out var cache))
        {
            WriteCache(cache!, GetPutObject(fileName, cache.MemoryStream.Length), false);

            var completeMultipartUploadArgs = new CompleteMultipartUploadArgs()
                .WithBucket(_minio.BucketName)
                .WithObject(fileName)
                .WithUploadId(cache.UploadId)
                .WithETags(cache.Etags);

            await _client.CompleteMultipartUploadAsync(completeMultipartUploadArgs, CancellationToken.None)
                .ConfigureAwait(false);
        }
    }

    public bool ExistDirectory(string path)
    {
        GetPath(ref path);


        // 当path为空是说明查询的是根目录 根目录一定存在
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        if (path.EndsWith("HEAD"))
        {
            return false;
        }

        var item = _memoryCaching.GetExistFile(path);
        if (item == null)
        {
            Info("{0}=> path:{1} ", nameof(ExistDirectory), path);

            var o = new ListObjectsArgs();
            o.WithBucket(_minio.BucketName);
            o.WithPrefix(path);
            try
            {
                var data = _client.ListObjectsAsync(o).GetAwaiter().GetResult();
                _memoryCaching.AddExistFile(path, data);
                return data?.IsDir == true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
        else
        {
            return item.IsDir == true;
        }
    }

    public static string TrimStart(string value, string search)
    {
        if (value.StartsWith(search))
        {
            var c = new char[value.Length - search.Length];
            int size = 0;
            for (var i = search.Length; i < value.Length; i++)
            {
                c[size++] = value[i];
            }

            return new string(c);
        }

        return value;
    }

    public void Dispose()
    {
        disposable = true;
        _memoryCaching.Dispose();
    }
}