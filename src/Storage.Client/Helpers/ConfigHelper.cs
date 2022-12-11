using System.Text.Json;
using Storage.Client.Options;

namespace Storage.Client.Helpers;

public class ConfigHelper
{
    public static DokanOptions? GetDokanOptions()
    {
        if (File.Exists(nameof(DokanOptions) + ".json"))
        {
            return JsonSerializer.Deserialize<DokanOptions>(File.ReadAllText(nameof(DokanOptions) + ".json"));
        }

        return new DokanOptions();
    }

    public static void SaveDokanOptions(DokanOptions value)
    {
        using var fileStream = File.Open(nameof(DokanOptions) + ".json", FileMode.Create);
        fileStream.Position = 0;
        fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(value));
        fileStream.Flush();
        fileStream.Close();
    }

    public static MinioOptions? GetMinioOptions()
    {
        return File.Exists(nameof(MinioOptions) + ".json") ? JsonSerializer.Deserialize<MinioOptions>(File.ReadAllText(nameof(MinioOptions) + ".json")) : new MinioOptions();
    }

    public static void SaveMinioOptions(MinioOptions value)
    {
        using var fileStream = File.Open(nameof(MinioOptions) + ".json", FileMode.Create);
        fileStream.Position = 0;
        fileStream.Write(JsonSerializer.SerializeToUtf8Bytes(value));
        fileStream.Flush();
        fileStream.Close();
    }
}