using Microsoft.Extensions.Options;
using Storage.Client.Helpers;
using Storage.Client.Options;
using System.Collections.Concurrent;
using System.IO;

namespace Storage.Client.Caches;

public class FileReadCache : IDisposable
{
    private bool _disposed;
    private readonly StorageOption _storageOption;
    private readonly ConcurrentDictionary<string, FileStreamCache> _StreamCache = new();

    public FileReadCache(IOptions<StorageOption> storageOption)
    {
        _storageOption = storageOption.Value;
        if (!Directory.Exists(_storageOption.CachePath))
        {
            Directory.CreateDirectory(_storageOption.CachePath);
        }
        _ = Task.Factory.StartNew(Start);
    }

    private async void Start()
    {
        while (!_disposed)
        {
            await Task.Delay(3000);
            var now = DateTime.Now;

            var data = _StreamCache.Where(x => x.Value.UpdateTime.AddMinutes(_storageOption.CacheExpiration) < now);
            if (data.Any())
            {
                foreach (var item in data)
                {
                    _StreamCache.Remove(item.Key, out var stream);
                    if (stream != null)
                    {
                        await stream.Stream.FlushAsync();
                        stream?.Stream.Close();
                    }
                }
            }
        }
    }

    public bool Write(string fileName, byte[] buffer, int offset)
    {
        if (!_storageOption.Cache)
        {
            return false;
        }
        SetPath(ref fileName);
        lock (fileName)
        {
            using var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.Position = offset;
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
            fileStream.Close();
        }
        return true;
    }

    public int Read(string fileName, byte[] buffer, int offset)
    {
        if (!_storageOption.Cache)
        {
            return -1;
        }
        SetPath(ref fileName);
        if (!File.Exists(fileName))
        {
            return -1;
        }
        lock (fileName)
        {
            using var fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            if (offset + buffer.Length > fileStream.Length)
            {
                fileStream.Close();
                return -1;
            }
            fileStream.Position = offset;
            var readSize = fileStream.Read(buffer);
            fileStream.Close();
            return readSize;
        }
    }


    public void Remove(string fileName)
    {
        SetPath(ref fileName);
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    private void SetPath(ref string path)
        => path = Path.Combine(_storageOption.CachePath, path.GetMd5Hash());

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// FileStream缓存模型
    /// </summary>
    internal class FileStreamCache
    {
        public FileStream Stream { get; set; }

        public DateTime UpdateTime { get; set; }

        public FileStreamCache(FileStream stream)
        {
            Stream = stream;
            UpdateTime = DateTime.Now;
        }
    }
}
