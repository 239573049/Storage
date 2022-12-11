namespace Storage.Client.Options;

public class OssOptions
{
    public string AccessKeyId { get; set; }

    public string AccessKeySecret { get; set; }

    public string Endpoint { get; set; }

    /// <summary>
    /// 存储空间
    /// </summary>
    public string BucketName { get; set; }
}
