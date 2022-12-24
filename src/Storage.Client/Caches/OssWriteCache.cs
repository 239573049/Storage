using Aliyun.OSS;

namespace Storage.Client.Caches;

public class OssWriteCache
{
    public List<PartETag> Tags { get; set; } = new List<PartETag>();

    public string UploadId { get; set; }

    public string FileName { get; set; }

    public DateTime UpdateTime { get; set; }

    public int PartNumber { get; set; }
    public MemoryStream MemoryStream { get; set; } = new();
}
