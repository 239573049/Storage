using DokanNet;
using System.Security.AccessControl;

namespace Storage.Host;

public interface IStorageService
{
    /// <summary>
    /// 解析文件地址
    /// </summary>
    /// <param name="fileName"></param>
    void GetPath(ref string fileName);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    NtStatus DeleteFile(string fileName, IDokanFileInfo info);

    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    NtStatus DeleteDirectory(string fileName, IDokanFileInfo info);

    /// <summary>
    /// 移动文件
    /// </summary>
    /// <param name="oldName"></param>
    /// <param name="newName"></param>
    /// <param name="replace"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    bool MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info);

    /// <summary>
    /// 移动文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    bool MoveDirectory(string path, string dest);

    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileInfo"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info);

    /// <summary>
    /// 获取文件列表
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="searchPattern"></param>
    /// <param name="files"></param>
    /// <returns></returns>
    NtStatus FindFiles(string filePath, string searchPattern, out IList<FileInformation> files);

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="buffer"></param>
    /// <param name="bytesRead"></param>
    /// <param name="offset"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info);

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="buffer"></param>
    /// <param name="bytesWritten"></param>
    /// <param name="offset"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info);

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool CreateFile(string path);

    /// <summary>
    /// 获取文件的信息
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="security"></param>
    /// <param name="sections"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections,
        IDokanFileInfo info);

    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool ExistDirectory(string path);

    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool ExistFile(string path);

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool CreateDirectory(string path);

    /// <summary>
    /// 获取硬盘信息
    /// </summary>
    /// <param name="freeBytesAvailable"></param>
    /// <param name="totalNumberOfBytes"></param>
    /// <param name="totalNumberOfFreeBytes"></param>
    /// <param name="info"></param>
    void GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes,
        IDokanFileInfo info);

    /// <summary>
    /// 获取硬盘基本信息
    /// </summary>
    /// <param name="volumeLabel"></param>
    /// <param name="features"></param>
    /// <param name="fileSystemName"></param>
    /// <param name="maximumComponentLength"></param>
    /// <param name="info"></param>
    void GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName,
        out uint maximumComponentLength, IDokanFileInfo info);
}