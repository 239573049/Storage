using System.Text.Json.Serialization;

namespace Minio.Credentials;

public class JsonWebToken
{
    public JsonWebToken(string token, uint expiry)
    {
        AccessToken = token;
        Expiry = expiry;
    }

    [JsonPropertyName("access_token")] public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")] public uint Expiry { get; set; }
}