using System.Security.AccessControl;
using DokanNet;
using Storage.Host;
using FileAccess = DokanNet.FileAccess;

namespace Storage.Client;

public class IntegrationOperations : IDokanOperations
{
    private const FileAccess DataAccess =
        FileAccess.ReadData | FileAccess.WriteData | FileAccess.AppendData |
        FileAccess.Execute | FileAccess.GenericExecute |
        FileAccess.GenericWrite | FileAccess.GenericRead;

    private const FileAccess DataWriteAccess =
        FileAccess.WriteData | FileAccess.AppendData |
        FileAccess.Delete | FileAccess.GenericWrite;

    private IStorageService? _storageService;

    public void Start(IStorageService? storageService)
    {
        _storageService = storageService;
    }

    public NtStatus CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode, FileOptions options,
        FileAttributes attributes, IDokanFileInfo info)
    {
        string fName = fileName.ToLower();
        if (fName.IndexOf("desktop.ini", StringComparison.Ordinal) > 0 ||
                                    fName.IndexOf("autorun.ini", StringComparison.Ordinal) > 0)
            return DokanResult.FileNotFound;

        var result = DokanResult.Success;
        if (info.IsDirectory)
        {
            try
            {
                switch (mode)
                {
                    case FileMode.Open:
                        if (!_storageService!.ExistDirectory(fileName))
                        {
                            return DokanResult.AccessDenied;
                        }

                        return DokanResult.Success;

                    case FileMode.CreateNew:
                        if (_storageService!.ExistDirectory(fileName))
                            return DokanResult.FileExists;
                        _storageService.CreateDirectory(fileName);
                        break;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return DokanResult.AccessDenied;
            }
        }
        else
        {
            var pathExists = true;
            var pathIsDirectory = false;

            var readWriteAttributes = (access & DataAccess) == 0;
            var readAccess = (access & DataWriteAccess) == 0;

            try
            {
                pathExists = (_storageService!.ExistDirectory(fileName) || _storageService!.ExistFile(fileName));
            }
            catch (IOException)
            {
            }

            switch (mode)
            {
                case FileMode.Open:

                    if (pathExists)
                    {
                        if (readWriteAttributes || pathIsDirectory)
                        {
                            if (pathIsDirectory && (access & DokanNet.FileAccess.Delete) ==
                                                DokanNet.FileAccess.Delete
                                                && (access & DokanNet.FileAccess.Synchronize) !=
                                                DokanNet.FileAccess.Synchronize)
                                return DokanResult.AccessDenied;

                            info.IsDirectory = pathIsDirectory;
                            return DokanResult.Success;
                        }
                    }
                    else
                    {
                        return DokanResult.FileNotFound;
                    }

                    break;
                case FileMode.Create:
                case FileMode.Append:
                case FileMode.OpenOrCreate:
                case FileMode.CreateNew:
                    if (pathExists)
                    {
                        return DokanResult.FileExists;
                    }

                    _storageService.CreateFile(fileName);
                    break;

                case FileMode.Truncate:
                    if (!pathExists)
                        return DokanResult.FileNotFound;
                    break;
                default:
                    return DokanResult.NotImplemented;
            }
        }

        return result;
    }

    public void Cleanup(string fileName, IDokanFileInfo info)
    {
        //(info.Context as FileStream)?.Dispose();
        //info.Context = null;

        if (info.DeleteOnClose)
        {
            if (info.IsDirectory)
            {
                _storageService.DeleteDirectory(fileName, info);
            }
            else
            {
                _storageService.DeleteFile(fileName, info);
            }
        }
    }

    public void CloseFile(string fileName, IDokanFileInfo info)
    {
        //(info.Context as FileStream)?.Dispose();
        //info.Context = null;
    }

    public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
    {
        if (info.Context == null)
        {
            return _storageService.ReadFile(fileName, buffer, out bytesRead, offset, info);
        }

        bytesRead = 0;

        //if (info.Context is not FileStream stream) return DokanResult.Success;

        //lock (stream)
        //{
        //    stream.Position = offset;
        //    bytesRead = stream.Read(buffer, 0, buffer.Length);
        //}

        return DokanResult.Success;
    }

    public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
    {
        return _storageService.WriteFile(fileName, buffer, out bytesWritten, offset, info);
    }

    public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info)
    {
        try
        {
            //((FileStream)(info.Context))?.Flush();
            return DokanResult.Success;
        }
        catch (IOException)
        {
            return DokanResult.DiskFull;
        }
    }

    public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
    {
        return _storageService.GetFileInformation(fileName, out fileInfo, info);
    }

    public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info)
    {
        return _storageService.FindFiles(fileName, "*", out files);
    }

    public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files,
        IDokanFileInfo info)
    {
        return _storageService.FindFiles(fileName, searchPattern, out files);
    }

    public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info)
    {
        return DokanResult.Success;
    }

    public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime,
        DateTime? lastWriteTime,
        IDokanFileInfo info)
    {
        _storageService.SetFileTime(fileName, creationTime, lastAccessTime, lastWriteTime, info);

        return DokanResult.Success;
    }

    public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
    {
        return _storageService.DeleteFile(fileName, info);
    }

    public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
    {
        return _storageService.DeleteDirectory(fileName, info);
    }

    public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
    {
        //(info.Context as FileStream)?.Dispose();
        //info.Context = null;

        bool pathExists = _storageService.ExistDirectory(oldName);
        bool fileExists = _storageService.ExistFile(oldName);

        try
        {
            if (info.IsDirectory)
            {
                if (pathExists && _storageService.MoveDirectory(oldName, newName))
                {
                    return DokanResult.Success;
                }
                else if (fileExists && _storageService.MoveFile(oldName, newName, replace, info))
                {
                    return DokanResult.Success;
                }
                else
                {
                    return DokanResult.Error;
                }
            }
            else
            {
                if (fileExists && _storageService.MoveFile(oldName, newName, replace, info))
                {
                    return DokanResult.Success;
                }
                else if (pathExists && _storageService.MoveDirectory(oldName, newName))
                {
                    return DokanResult.Success;
                }
                else
                {
                    return DokanResult.Error;
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            return DokanResult.AccessDenied;
        }
    }

    public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info)
    {
        return DokanResult.Success;
    }

    public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info)
    {
        return DokanResult.Success;
    }

    public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info)
    {
        try
        {
            //((FileStream)(info.Context))?.Lock(offset, length);
            return DokanResult.Success;
        }
        catch (IOException)
        {
            return DokanResult.AccessDenied;
        }
    }

    public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
    {
        try
        {
            //((FileStream)(info.Context))?.Unlock(offset, length);
            return DokanResult.Success;
        }
        catch (IOException)
        {
            return DokanResult.AccessDenied;
        }
    }

    public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes,
        out long totalNumberOfFreeBytes,
        IDokanFileInfo info)
    {
        totalNumberOfBytes = (long)1024 * 1024 * 1024 * 1024;
        freeBytesAvailable = Convert.ToInt64(0.95 * totalNumberOfBytes);
        totalNumberOfFreeBytes =
            Convert.ToInt64(0.95 * totalNumberOfBytes *
                            0.95);

        return NtStatus.Success;
    }

    public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features,
        out string fileSystemName,
        out uint maximumComponentLength, IDokanFileInfo info)
    {
        _storageService.GetVolumeInformation(out volumeLabel, out features, out fileSystemName,
            out maximumComponentLength, info);

        return DokanResult.Success;
    }

    public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections,
        IDokanFileInfo info)
    {
        security = null;
        return DokanResult.NotImplemented;
    }

    public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections,
        IDokanFileInfo info)
    {
        return DokanResult.NotImplemented;
    }

    public NtStatus Mounted(string mountPoint, IDokanFileInfo info)
    {
        return NtStatus.Success;
    }

    public NtStatus Unmounted(IDokanFileInfo info)
    {
        return NtStatus.Success;
    }

    public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info)
    {
        streams = new List<FileInformation>();
        return DokanResult.NotImplemented;
    }
}