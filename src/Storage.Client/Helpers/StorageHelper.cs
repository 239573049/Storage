using System.Text;
using System.Security.Cryptography;

namespace Storage.Client.Helpers;

public static class StorageHelper
{
    /// <summary>
    /// md5加密
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetMd5Hash(this string input)
    {
        byte[] data = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
        StringBuilder sBuilder = new();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }
}
