namespace Storage.Client.Caches;

public class ReadCache
{
    public int Offset { get; set; }

    public byte[] Buffer { get; set; }

    public ReadCache(int offset)
    {
        Offset = offset;
        Buffer = Array.Empty<byte>();
    }
}