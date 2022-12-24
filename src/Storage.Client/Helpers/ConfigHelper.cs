using Storage.Client.Options;
using System.Text.Json;

namespace Storage.Client.Helpers;

public class ConfigHelper
{

    public static MinIoOptions? GetMinIoOptions()
    {
        return File.Exists(nameof(MinIoOptions) + ".json") ? JsonSerializer.Deserialize<MinIoOptions>(File.ReadAllText(nameof(MinIoOptions) + ".json")) : new MinIoOptions();
    }

    public static void SaveMinIoOptions(MinIoOptions value)
    {
        using var fileStream = File.Open(nameof(MinIoOptions) + ".json", FileMode.Create);
        fileStream.Position = 0;
        fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(value));
        fileStream.Flush();
        fileStream.Close();
    }

    public static void SaveOssOptions(OssOptions value)
    {
        using var fileStream = File.Open(nameof(OssOptions) + ".json", FileMode.Create);
        fileStream.Position = 0;
        fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(value));
        fileStream.Flush();
        fileStream.Close();
    }
 
    /// <summary>
    /// 获取Oss配置
    /// </summary>
    /// <returns></returns>
    public static OssOptions? GetOssOptions()
    {
        return File.Exists(nameof(OssOptions) + ".json") ? JsonSerializer.Deserialize<OssOptions>(File.ReadAllText(nameof(OssOptions) + ".json")) : new OssOptions();
    }
}