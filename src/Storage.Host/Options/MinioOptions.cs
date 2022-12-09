namespace Storage.Host.Options;

public class MinioOptions
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public string Endpoint { get; set; }
    public int Port { get; set; }

    public string VolumeLabel { get; set; }
}