using DokanNet;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Storage.Host.Options;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Storage.Host.Storage;

public class MinioService : IStorageService
{
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

    }

    private void Info(string? message, params object?[] args)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(message, args);
    }

    private void Error(string? message, params object?[] args)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(message, args);
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

        _memoryCaching.Remove(fileName);

        return NtStatus.Success;
    }

    public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(DeleteDirectory), fileName, info.IsDirectory);

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

        _memoryCaching.Remove(fileName);

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
            .WithObject(path+'/')
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

        Info("{0}=> fileName:{1}  IsDirectory:{2}", nameof(GetFileInformation), fileName, info.IsDirectory);

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

        var filesInformation = _memoryCaching.Get<FileInformation?>(fileName + "/GetFileInformation");
        if (filesInformation == null)
        {
            var o = new ListObjectsArgs();
            o.WithBucket(_minio.BucketName);
            o.WithPrefix(fileName);
            o.WithRecursive(false);

            Item data = null;
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

            _memoryCaching.Add(fileName + "GetFileInformation", fileInfo);
        }
        else
        {
            fileInfo = (FileInformation)filesInformation;
        }

        return DokanResult.Success;
    }

    public NtStatus FindFiles(string filePath, string searchPattern, out IList<FileInformation> files)
    {
        GetPath(ref filePath);

        Info("{0}=> filePath:{1}  searchPattern:{2}", nameof(FindFiles), filePath, searchPattern);

        var o = new ListObjectsArgs();
        o.WithBucket(_minio.BucketName);

        var name = filePath + "/" + (searchPattern == "*" ? "" : searchPattern);
        name = name.EndsWith('/') ? name : name + '/';

        o.WithPrefix(name);

        files = _client.ListObjectsAsync(o)
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


        return NtStatus.Success;
    }

    public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
    {
        GetPath(ref fileName);

        Info("{0}=> filePath:{1}  searchPattern:{2}", nameof(ReadFile), fileName, offset);

        var obj = new GetObjectArgs();
        obj.WithBucket(_minio.BucketName);
        obj.WithObject(fileName);
        obj.WithOffsetAndLength(offset, buffer.Length);
        var read = 0;
        obj.WithCallbackStream(stream =>
        {
            var m = new MemoryStream();
            stream.CopyTo(m);
            read = (int)m.Length;
            Array.Copy(m.GetBuffer(), 0, buffer, 0, read);
        });
        try
        {
            _client.GetObjectAsync(obj).GetAwaiter().GetResult();
            bytesRead = read;
        }
        catch (Exception exception)
        {
            Error("ReadFile exception :{0} fileName:{1} ", exception, fileName);
            bytesRead = 0;
        }

        return NtStatus.Success;
    }

    public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
    {
        GetPath(ref fileName);
        Info("{0}=> filePath:{1}  searchPattern:{2}", nameof(WriteFile), fileName, offset);
        bytesWritten = 0;

        if (!info.PagingIo)
        {
            var put = new PutObjectArgs();
            put.WithBucket(_minio.BucketName);
            put.WithObject(fileName);
            put.WithObjectSize(buffer.Length);
            put.WithStreamData(new MemoryStream(buffer));
            _client.PutObjectAsync(put).GetAwaiter().GetResult();
        }

        bytesWritten = buffer.Length;

        return NtStatus.Success;
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

        _memoryCaching.Remove(path);

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
        Info("{0}=> path:{1} ", nameof(ExistFile), path);

        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        var value = _memoryCaching.Get<Item?>(path + "ExistFile");
        if (value == null)
        {
            var o = new ListObjectsArgs();
            o.WithBucket(_minio.BucketName);
            o.WithPrefix(path);
            try
            {
                var data = _client.ListObjectsAsync(o).GetAwaiter().GetResult();
                _memoryCaching.Add(path + "ExistFile", data);
                return data?.IsDir == false;
            }
            catch
            {
                return false;
            }
        }

        return value.IsDir == false;
    }

    public bool CreateDirectory(string path)
    {
        GetPath(ref path);

        Info("{0}=> path:{1} ", nameof(CreateDirectory), path);

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("desktop"));

        try
        {
            var put = new PutObjectArgs();
            put.WithObjectSize(stream.Length);
            put.WithStreamData(stream);
            put.WithBucket(_minio.BucketName);
            put.WithObject(path + "/.desktop");
            put.WithContentType("application/octet-stream");
            _client.PutObjectAsync(put).GetAwaiter().GetResult();
            _memoryCaching.Remove(path);
        }
        catch (Exception exception)
        {
            Error("CreateDirectory Exception :{0} path:{1} ", exception, path);
            return false;
        }
        return true;
    }

    public void GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes,
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
        volumeLabel = "Token";
        fileSystemName = "NTFS";
        maximumComponentLength = 256;
        features = FileSystemFeatures.CasePreservedNames | FileSystemFeatures.CaseSensitiveSearch |
                   FileSystemFeatures.PersistentAcls | FileSystemFeatures.SupportsRemoteStorage |
                   FileSystemFeatures.UnicodeOnDisk;
    }

    public bool ExistDirectory(string path)
    {
        GetPath(ref path);

        Info("{0}=> path:{1} ", nameof(ExistDirectory), path);

        // 当path为空是说明查询的是根目录 根目录一定存在
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        var o = new ListObjectsArgs();
        o.WithBucket(_minio.BucketName);
        o.WithPrefix(path);
        try
        {
            var data = _client.ListObjectsAsync(o).GetAwaiter().GetResult();
            return data?.IsDir == true;
        }
        catch (Exception exception)
        {

            return false;
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
}