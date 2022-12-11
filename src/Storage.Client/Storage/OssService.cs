using Aliyun.OSS;
using DokanNet;
using Storage.Client.Options;
using Storage.Host;
using System.Security.AccessControl;

namespace Storage.Client.Storage;

public class OssService : IStorageService, IDisposable
{
    private bool disposable;

    private readonly OssOptions _ossOptions;
    private readonly OssClient _client;
    public OssService(OssOptions ossOptions)
    {
        _ossOptions = ossOptions;
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
        throw new NotImplementedException();
    }

    bool IStorageService.ExistFile(string path)
    {
        throw new NotImplementedException();
    }

    public NtStatus FindFiles(string filePath, string searchPattern, out IList<FileInformation> files)
    {
        GetPath(ref filePath);
        var listObjectsRequest = new ListObjectsRequest(_ossOptions.BucketName)
        {
            Delimiter = filePath
        };
        // 简单列举Bucket中的文件，默认返回100条记录。
        var result = _client.ListObjects(listObjectsRequest);

        files = result.ObjectSummaries.Select(x => new FileInformation()
        {
            Attributes = FileAttributes.Directory,
            FileName = x.Key,
            Length = x.Size,
            LastWriteTime = x.LastModified
        }).ToList();

        foreach (var x in result.ObjectSummaries)
        {
            files.Add(new FileInformation()
            {
                Attributes = FileAttributes.Normal,
                FileName = x.Key,
                Length = x.Size,
                LastWriteTime = x.LastModified
            });
        }

        return NtStatus.Success;
    }

    void IStorageService.GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
    {
        throw new NotImplementedException();
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
        fileInfo = new FileInformation();


        return NtStatus.Success;
    }

    NtStatus IStorageService.GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    void IStorageService.GetPath(ref string fileName)
    {
        throw new NotImplementedException();
    }

    void IStorageService.GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    bool IStorageService.MoveDirectory(string path, string dest)
    {
        throw new NotImplementedException();
    }

    bool IStorageService.MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    NtStatus IStorageService.ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    void IStorageService.SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }

    NtStatus IStorageService.WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
    {
        throw new NotImplementedException();
    }
}
