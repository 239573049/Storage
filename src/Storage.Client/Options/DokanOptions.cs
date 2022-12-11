using System.Text.Json.Serialization;

namespace Storage.Client.Options;

public class DokanOptions
{
    /// <summary>
    /// 挂载点
    /// </summary>
    public string MountPoint { get; set; } = "T:\\";

    /// <summary>
    /// 是否默认自启动
    /// </summary>
    public bool StartDefault { get; set; }

    /// <summary>
    /// 服务状态 是否启动
    /// </summary>
    [JsonIgnore] 
    public bool ServiceState { get; set; }
}