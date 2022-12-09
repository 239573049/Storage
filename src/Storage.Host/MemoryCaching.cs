using System.Collections.Concurrent;

namespace Storage.Host;

public class MemoryCaching : IDisposable
{
    private bool dispose = false;
    private readonly ConcurrentDictionary<string, Caching> _caching = new();

    public MemoryCaching()
    {
        Task.Factory.StartNew(ClearCache);
    }

    private async Task ClearCache()
    {
        while (!dispose)
        {
            await Task.Delay(5000);
            var now = DateTime.Now.AddMinutes(5);
            var list = _caching.Where(x => x.Value.RefreshTime > now).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                _caching.TryRemove(list[i].Key, out _);
            }
        }
    }

    public void Add<T>(string key, T? value)
    {
        var v = new Caching(value);

        _caching.AddOrUpdate(key, v, ((s, o) => v));
    }

    public void Remove(string key)
    {
        _caching.TryRemove(key, out _);
    }

    public T? Get<T>(string key)
    {
        if (_caching.TryGetValue(key, out var value))
        {
            return (T?)value.Value;
        }

        return default;
    }

    public bool Exist(string key)
    {
        return _caching.TryGetValue(key, out _);
    }

    class Caching
    {
        /// <summary>
        /// 内容
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public DateTime RefreshTime { get; set; }

        public Caching(object value)
        {
            Value = value;
            RefreshTime = DateTime.Now;
        }
    }

    public void Dispose()
    {
        dispose = true;
        _caching.Clear();
    }

}