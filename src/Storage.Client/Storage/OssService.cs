using Aliyun.OSS;
using DokanNet;
using Storage.Client.Caches;
using Storage.Client.Helpers;
using Storage.Client.Options;
using Storage.Host;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Storage.Client.Storage;

public class OssService : IStorageService, IDisposable
{
    private bool disposable;

    private readonly OssOptions _ossOptions;
    private readonly OssClient _client;
    private readonly ConcurrentDictionary<string, ReadCache> _readCaches = new();
    public OssService()
    {
        _ossOptions = ConfigHelper.GetOssOptions();
        _client = new OssClient(_ossOptions.Endpoint, _ossOptions.AccessKeyId, _ossOptions.AccessKeySecret);
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

    bool IStorageService.CreateDirectory(string path)
    {
        throw new NotImplementedException();
    }

    bool IStorageService.CreateFile(string path)
    {
        throw new NotImplementedException();
    }

    NtStatus IStorageService.DeleteDirectory(string fileName, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    NtStatus IStorageService.DeleteFile(string fileName, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    bool IStorageService.ExistDirectory(string path)
    {
        GetPath(ref path);

        // 当path为空是说明查询的是根目录 根目录一定存在
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        path = path.EndsWith("/") ? path : path + '/';
        var result = _client.DoesObjectExist(_ossOptions.BucketName, path);

        return result;
    }

    bool IStorageService.ExistFile(string path)
    {
        GetPath(ref path);
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        var result = _client.DoesObjectExist(_ossOptions.BucketName, path);

        return result;
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
        totalNumberOfBytes = (long)1024 * 1024 * 1024 * 1024;
        freeBytesAvailable = Convert.ToInt64(0.95 * totalNumberOfBytes);
        totalNumberOfFreeBytes =
            Convert.ToInt64(0.95 * totalNumberOfBytes *
                            0.95);
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
        throw new NotImplementedException();
    }

    bool IStorageService.MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    // 设置上传切片大小
    const int ReadSize = 1024 * 1024 * 10;

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
        throw new NotImplementedException();
    }

    NtStatus IStorageService.WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
    {
        throw new NotImplementedException();
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
