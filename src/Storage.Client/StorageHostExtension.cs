using DokanNet;
using Storage.Client.Options;
using Storage.Client.Storage;
using Storage.Host;
using Storage.Host.Storage;
using System.Collections.Concurrent;
using DokanOptions = DokanNet.DokanOptions;

namespace Storage.Client;

public static class StorageHostExtension
{
    private static readonly ConcurrentDictionary<StorageDokan, StorageDokanInstance> DokanInstance = new();

    public static bool StartMinIo(StorageDokan dokan)
        => DokanInstance.Any(x => x.Key == dokan && x.Value.StartMinio);

    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddSingleton<IDokanOperations, IntegrationOperations>();

        return services;
    }
    
    public static void UseDokan(this IServiceProvider app, BaseDokanOptions dokanOptions, StorageDokan storageDokan, Action<bool>? succeed = null)
    {
        _ = Task.Factory.StartNew(() =>
        {
            if (StartMinIo(storageDokan))
            {
                succeed?.Invoke(true);
                return;
            }

            try
            {
                var manualReset = new ManualResetEvent(false);
                var dokan = new Dokan(new DokanNet.Logging.NullLogger());

                var dokanOperations = app.GetRequiredService<IDokanOperations>() as IntegrationOperations;

                var dokanBuilder = new DokanInstanceBuilder(dokan)
                    .ConfigureLogger(() => new DokanNet.Logging.NullLogger())
                    .ConfigureOptions(options =>
                    {
                        options.Options = DokanOptions.FixedDrive;
                        options.MountPoint = dokanOptions.MountPoint;
                        // 是否单线程
                        options.SingleThread = false;
                    });

                switch (storageDokan)
                {
                    case StorageDokan.MinIo:
                        dokanOperations!.Start(new MinioService());
                        break;
                    case StorageDokan.Oss:
                        dokanOperations!.Start(new OssService());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(storageDokan), storageDokan, null);
                }

                DokanInstance.TryAdd(storageDokan, new StorageDokanInstance(dokanBuilder.Build(dokanOperations)));

                succeed?.Invoke(true);
                manualReset.WaitOne();
            }
            catch (Exception e)
            {
                succeed?.Invoke(false);
                MessageBox.Show("挂载硬盘驱动错误", e.Message);
            }
        });

    }

    /// <summary>
    /// 释放MinIo服务
    /// </summary>
    public static void Stop(StorageDokan naem)
    {
        try
        {
            if (DokanInstance.TryRemove(naem, out var value))
            {

                value.DokanInstance.Dispose();
            }
        }
        catch
        {
        }
    }
}

public class StorageDokanInstance
{
    public DokanInstance DokanInstance { get; set; }

    public bool StartMinio { get; set; }

    public StorageDokanInstance(DokanInstance dokanInstance)
    {
        DokanInstance = dokanInstance;
    }
}