namespace Storage.Host.Caches;

public class MinioWriteCache
{
    public string UploadId { get; set; }

    public int PartNumber { get; set; }

    public MemoryStream MemoryStream { get; set; } = new();


    public Dictionary<int, string> Etags = new();

    public DateTime UpdateTime { get; set; }
}