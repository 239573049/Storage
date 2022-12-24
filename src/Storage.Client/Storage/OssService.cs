using Aliyun.OSS;
using DokanNet;
using Minio;
using Storage.Client.Caches;
using Storage.Client.Helpers;
using Storage.Client.Options;
using Storage.Host;
using Storage.Host.Caches;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;

namespace Storage.Client.Storage;

public class OssService : IStorageService, IDisposable
{
    private bool disposable;

    private readonly OssOptions _ossOptions;
    private readonly OssClient _client;
    private readonly ConcurrentDictionary<string, ReadCache> _readCaches = new();
    private readonly ConcurrentDictionary<string, OssWriteCache> _writeCache = new();
    public OssService()
    {
        _ossOptions = ConfigHelper.GetOssOptions();
        _client = new OssClient(_ossOptions.Endpoint, _ossOptions.AccessKeyId, _ossOptions.AccessKeySecret);
        _ = Task.Factory.StartNew(StartWriteCache);
    }

    private async void StartWriteCache()
    {
        while (!disposable)
        {
            await Task.Delay(2000);
            var now = DateTime.Now;
            try
            {
                foreach (var t in _writeCache.Where(x => x.Value.UpdateTime.AddSeconds(1) < now))
                {
                    if (_writeCache.TryRemove(t.Key, out var writeCache))
                    {
                        await Upload(writeCache);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
    private async Task Upload(OssWriteCache writeCache)
    {
        // TODO:当tag为空时数据是不适合切片的所以需要单独上传
        if (writeCache.Tags == null || writeCache.Tags?.Any() == false)
        {
            _client.PutObject(_ossOptions.BucketName, writeCache.FileName, new MemoryStream(writeCache.MemoryStream.ToArray()));
            return;
        }

        WriteCache(writeCache!, false, true);

        var completeMultipartUploadRequest = new CompleteMultipartUploadRequest(_ossOptions.BucketName, writeCache.FileName, writeCache.UploadId);

        foreach (var tag in writeCache.Tags)
        {
            completeMultipartUploadRequest.PartETags.Add(tag);
        }
        _client.CompleteMultipartUpload(completeMultipartUploadRequest);
    }

    public void Dispose()
    {
        disposable = true;
    }

    public void GetPath(ref string fileName)
    {
        if (fileName != "/")
        {
            // TODO： 由于Mino和win的路径符号不一样需要置换
            fileName = fileName.Replace('\\', '/').TrimStart('/');
        }
    }

    public bool CreateDirectory(string path)
    {
        GetPath(ref path);
        try
        {
            _client.PutObject(_ossOptions.BucketName, path, new MemoryStream());

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool CreateFile(string path)
    {
        GetPath(ref path);
        try
        {
            _client.PutObject(_ossOptions.BucketName, path, new MemoryStream());

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    NtStatus IStorageService.DeleteDirectory(string fileName, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(DeleteFile), fileName, info.IsDirectory);

        if (!ExistFile(fileName))
            return DokanResult.FileNotFound;

        _client.DeleteObject(_ossOptions.BucketName, fileName + "/");

        return NtStatus.Success;
    }

    public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(DeleteFile), fileName, info.IsDirectory);

        if (!ExistFile(fileName))
            return DokanResult.FileNotFound;

        _client.DeleteObject(_ossOptions.BucketName, fileName);

        return NtStatus.Success;
    }

    public bool ExistDirectory(string path)
    {
        GetPath(ref path);

        // 当path为空是说明查询的是根目录 根目录一定存在
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        path = path.EndsWith("/") ? path : path + '/';
        try
        {
            var result = _client.DoesObjectExist(_ossOptions.BucketName, path);
            return result;
        }
        catch (Exception e)
        {
            return false;
        }

    }

    public bool ExistFile(string path)
    {
        GetPath(ref path);
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        try
        {

            var result = _client.DoesObjectExist(_ossOptions.BucketName, path);
            
            return result;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public NtStatus FindFiles(string filePath, string searchPattern, out IList<FileInformation> files)
    {


        var name = filePath + "/" + (searchPattern == "*" ? "" : searchPattern);
        name = name.EndsWith('/') ? name : name + '/';

        GetPath(ref name);
        if (name.Length <= 1)
        {
            name = "/";
        }
        var listObjectsRequest = new ListObjectsRequest(_ossOptions.BucketName)
        {
            Delimiter = name,
            MaxKeys = 1000
        };

        try
        {
            // 简单列举Bucket中的文件，默认返回100条记录。
            var result = _client.ListObjects(listObjectsRequest);

            files = result.ObjectSummaries.Select(x => new FileInformation()
            {
                Attributes = FileAttributes.Normal,
                FileName = TrimStart(x.Key.TrimEnd('/'), name),
                Length = x.Size,
                LastWriteTime = x.LastModified
            }).ToList();

            foreach (var x in result.CommonPrefixes)
            {
                files.Add(new FileInformation()
                {
                    Attributes = FileAttributes.Directory,
                    FileName = TrimStart(x.TrimEnd('/'), name),
                });
            }

            return NtStatus.Success;

        }
        catch (Exception e)
        {
            Error("{0}=》filePath:{1} searchPattern:{2}", nameof(FindFiles), filePath, searchPattern);
            files = null;
            return NtStatus.Error;
        }
    }

    void IStorageService.GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
    {
        // TODO:设置硬盘显示的数据
        totalNumberOfBytes = (long)1024 * 1024 * 1024 * 1024 * 1024;
        freeBytesAvailable = (long)1024 * 1024 * 1024 * 1024 * 1024;
        totalNumberOfFreeBytes = (long)1024 * 1024 * 1024 * 1024 * 1024;
    }

    NtStatus IStorageService.GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
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

        var listObjectsRequest = new ListObjectsRequest(_ossOptions.BucketName)
        {
            Prefix = fileName,
            MaxKeys = 1,
        };
        var result = _client.ListObjects(listObjectsRequest);

        if (result.CommonPrefixes.Any(x => x == fileName))
        {
            fileInfo = new FileInformation()
            {
                Attributes = FileAttributes.Directory,
                FileName = fileName.TrimEnd('/'),

            };
        }
        else
        {
            var data = result.ObjectSummaries.FirstOrDefault(x => x.Key == fileName);
            if (data == null)
            {
                fileInfo = new FileInformation();
                return NtStatus.Error;
            }

            fileInfo = new FileInformation()
            {
                Attributes = FileAttributes.Normal,
                FileName = data.Key,
                Length = data.Size
            };

        }
        return NtStatus.Success;
    }

    private void Info(string? message, params object?[] args)
    {
#if DEBUG
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Debug.WriteLine(message, args);
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
        Debug.WriteLine(message, args);
#endif
    }

    NtStatus IStorageService.GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
    {
        security = null;
        return DokanResult.NotImplemented;
    }

    void IStorageService.GetPath(ref string fileName)
    {
        if (fileName != "/")
        {
            // TODO： 由于Mino和win的路径符号不一样需要置换
            fileName = fileName.Replace('\\', '/').TrimStart('/');
        }
    }

    void IStorageService.GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
    {
        volumeLabel = _ossOptions.VolumeLabel;
        fileSystemName = "NTFS";
        maximumComponentLength = 256;
        features = FileSystemFeatures.CasePreservedNames | FileSystemFeatures.CaseSensitiveSearch |
                   FileSystemFeatures.PersistentAcls | FileSystemFeatures.SupportsRemoteStorage |
                   FileSystemFeatures.UnicodeOnDisk;
    }

    bool IStorageService.MoveDirectory(string path, string dest)
    {
        GetPath(ref path);
        GetPath(ref dest);
        var copy = new CopyObjectRequest(_ossOptions.BucketName, path + "/", _ossOptions.BucketName, dest + "/");
        var delete = new DeleteObjectRequest(_ossOptions.BucketName, dest + "/");

        try
        {
            _client.CopyObject(copy);

            _client.DeleteObject(delete);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
    {
        GetPath(ref oldName);
        GetPath(ref newName);
        var copy = new CopyObjectRequest(_ossOptions.BucketName, oldName, _ossOptions.BucketName, oldName);
        var delete = new DeleteObjectRequest(_ossOptions.BucketName, newName);

        try
        {
            _client.CopyObject(copy);

            _client.DeleteObject(delete);
            return true;
        }
        catch (Exception)
        {
            return false;
        }

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
        else if (offset >= cache?.Offset) // 当需要缓存长度大于需要的长度时进入
        {
            if ((offset - cache.Offset + buffer.Length) < cache.Buffer.Length)
            {
                Info("{0}=> filePath:{1} 缓存 buffer:{2}  offset:{3}", nameof(ReadFile), fileName, buffer.Length, offset);
                Array.Copy(cache.Buffer, offset - cache.Offset, buffer, 0, buffer.Length);
                bytesRead = buffer.Length;
                return NtStatus.Success;
            }
        }

        Info("{0}=> filePath:{1}  buffer:{2}  offset:{3}", nameof(ReadFile), fileName, buffer.Length, offset);
        var getObjectRequest = new GetObjectRequest(_ossOptions.BucketName, fileName);

        getObjectRequest.SetRange(offset, buffer.Length > ReadSize ? buffer.Length : ReadSize);

        var ossObject = _client.GetObject(getObjectRequest);
        using (var requestStream = ossObject.Content)
        {
            using var m = new MemoryStream();
            cache.Offset = (int)offset;
            requestStream.CopyTo(m);
            cache.Buffer = m.ToArray();
        }

        bytesRead = cache.Buffer.Length;

        Array.Copy(cache.Buffer, cache.Offset - offset, buffer, 0,
            buffer.Length > cache.Buffer.Length ? cache.Buffer.Length : buffer.Length);

        return NtStatus.Success;
    }

    void IStorageService.SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
    {
        GetPath(ref fileName);
        if (_writeCache.Remove(fileName, out var cache))
        {
            WriteCache(cache, false);
            _ = Task.Run(async () =>
            {
                await Upload(cache);
            });
        }
    }

    NtStatus IStorageService.WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        if (offset == 0)
        {
            // TODO：当第一次写入时小于设置的切片先添加到缓存
            if (buffer.Length < ReadSize)
            {
                var memoryStream = new MemoryStream();
                memoryStream.Write(buffer);
                var writeCache = new OssWriteCache
                {
                    FileName = fileName,
                    MemoryStream = memoryStream,
                    UpdateTime = DateTime.Now,
                };
                _writeCache.TryAdd(fileName, writeCache);
            }
            else
            {
                var request = new InitiateMultipartUploadRequest(_ossOptions.BucketName, fileName);
                var result = _client.InitiateMultipartUpload(request);

                var uploadPart = new UploadPartRequest(_ossOptions.BucketName, fileName, result.UploadId)
                {
                    InputStream = new MemoryStream(buffer),
                    PartSize = buffer.Length,
                    PartNumber = 1
                };
                // 调用UploadPart接口执行上传功能，返回结果中包含了这个数据片的ETag值。
                var resultUpload = _client.UploadPart(uploadPart);
                var writeCache = new OssWriteCache
                {
                    FileName = fileName,
                    UpdateTime = DateTime.Now,
                    UploadId = result.UploadId,
                    PartNumber = 1
                };
                writeCache.Tags.Add(resultUpload.PartETag);
                _writeCache.TryAdd(fileName, writeCache);
            }
        }
        else
        {
            _writeCache.TryGetValue(fileName, out var writeCache);

            if (writeCache != null)
            {
                // TODO： 将数据写入缓存
                writeCache!.MemoryStream.Write(buffer);
                writeCache.UpdateTime = DateTime.Now;
                WriteCache(writeCache);
            }
        }
        bytesWritten = buffer.Length;
        return NtStatus.Success;
    }


    private void WriteCache(OssWriteCache writeCache, bool newMemory = true, bool section = false)
    {
        // TODO:如果缓存数据小于切片将不启用切片
        if (writeCache.MemoryStream.Length >= ReadSize || section)
        {
            Console.WriteLine("WriteCache size:{0}", writeCache.MemoryStream.Length);
            var s = Stopwatch.StartNew();
            writeCache!.PartNumber += 1;
            if (string.IsNullOrEmpty(writeCache.UploadId))
            {
                var request = new InitiateMultipartUploadRequest(_ossOptions.BucketName, writeCache.FileName);
                var result = _client.InitiateMultipartUpload(request);
                writeCache.UploadId = result.UploadId;
            }

            var uploadPart = new UploadPartRequest(_ossOptions.BucketName, writeCache.FileName, writeCache.UploadId)
            {
                InputStream = new MemoryStream(writeCache.MemoryStream.ToArray()),
                PartSize = writeCache.MemoryStream.Length,
                PartNumber = writeCache.PartNumber + 1,
            };

            var resultUpload = _client.UploadPart(uploadPart);

            writeCache.Tags.Add(resultUpload.PartETag);
            s.Stop();
            Info("{0}=> 耗时2：{1}ms", nameof(WriteCache), s.ElapsedMilliseconds);
            if (newMemory)
            {
                writeCache.MemoryStream = new MemoryStream();
            }
        }
    }

    private static string TrimStart(string value, string search)
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
}
