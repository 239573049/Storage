namespace Storage.Client.Options;

public class StorageOption
{
    /// <summary>
    /// 缓存地址
    /// </summary>
    public string CachePath { get; set; } = AppContext.BaseDirectory;

    /// <summary>
    /// 是否使用文件缓存
    /// </summary>
    public bool Cache { get; set; }

    /// <summary>
    /// 缓存过期时间（分）
    /// </summary>
    public int CacheExpiration { get; set; } = 60;

    /// <summary>
    /// Web展示url
    /// </summary>
    public string WebUI { get; set; } = "/";
}
