namespace Storage.Client.Caches;

public class MinioWriteCache
{
    public string UploadId { get; set; }

    public int PartNumber { get; set; }

    public MemoryStream MemoryStream { get; set; } = new();

    public string FileName { get; set; }


    public Dictionary<int, string> Etags = new();

    public DateTime UpdateTime { get; set; }

}