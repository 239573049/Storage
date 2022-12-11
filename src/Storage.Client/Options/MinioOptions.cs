namespace Storage.Client.Options;

public class MinioOptions
{
    /// <summary>
    /// minio AccessKey
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// minio SecreKey
    /// </summary>
    public string SecretKey { get; set; }

    private string bucketName = "storage";

    /// <summary>
    /// minio桶
    /// </summary>
    public string BucketName
    {
        get => bucketName;
        // TODO: Minio桶名称只能是小写
        set => bucketName = value.ToLower();
    }

    /// <summary>
    /// minio服务地址
    /// </summary>
    public string Endpoint { get; set; } = "127.0.0.1";

    /// <summary>
    /// minio服务端口
    /// </summary>
    public int Port { get; set; } = 9000;

    public string VolumeLabel { get; set; } = "Token";
}