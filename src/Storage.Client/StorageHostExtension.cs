using DokanNet;
using Storage.Client.Caches;
using Storage.Client.Helpers;
using Storage.Client.Options;
using Storage.Client.Storage;
using Storage.Host.Storage;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using DokanOptions = DokanNet.DokanOptions;

namespace Storage.Client;

public static class StorageHostExtension
{
    private static readonly ConcurrentDictionary<StorageDokan, StorageDokanInstance> DokanInstance = new();

    public static List<StorageDokanInstance> GetDokanInstance
    {
        get
        {
            return DokanInstance.Select(x => x.Value).ToList();
        }
    }

    public static bool StartMinIo(StorageDokan dokan)
        => DokanInstance.Any(x => x.Key == dokan);

    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDokanOperations, IntegrationOperations>();
        services.AddSingleton<FileReadCache>();
        services.Configure<StorageOption>(configuration.GetSection(nameof(StorageOption)));

        return services;
    }

    public static void UseDokan(this IServiceProvider app, BaseDokanOptions dokanOptions, StorageDokan storageDokan, Action<bool>? succeed = null)
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
            var fileCache = app.GetRequiredService<FileReadCache>();
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
                    dokanOperations!.Start(new MinioService(fileCache));
                    break;
                case StorageDokan.Oss:
                    dokanOperations!.Start(new OssService(fileCache));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageDokan), storageDokan, null);
            }

            DokanInstance.TryAdd(storageDokan, new StorageDokanInstance(dokanBuilder.Build(dokanOperations), storageDokan));

            succeed?.Invoke(true);

            _ = Task.Factory.StartNew(() =>
            {
                manualReset.WaitOne();
            });
        }
        catch (Exception e)
        {
            succeed?.Invoke(false);
        }


    }

    public static void UseApi(this IEndpointRouteBuilder app)
    {
        // 修改配置信息
        app.MapPut("/api/minio-config", (MinIoOptions options)
            => ConfigHelper.SaveMinIoOptions(options));

        // 获取minio配置信息
        app.MapGet("/api/minio-config", ()
            => ConfigHelper.GetMinIoOptions());

        // 查看服务列表
        app.MapGet("/api/server/list", ()
            => StorageHostExtension.GetDokanInstance);

        // 获取服务状态
        app.MapGet("/api/server", (StorageDokan dokan) =>
        {
            return StorageHostExtension.StartMinIo(dokan);
        });

        // 启动服务
        app.MapPost("/api/server/start", (IServiceProvider serviceProvider, StorageDokan dokan) =>
        {
            BaseDokanOptions options;
            if (dokan == StorageDokan.MinIo)
            {
                options = ConfigHelper.GetMinIoOptions();
            }
            else
            {
                options = ConfigHelper.GetOssOptions();
            }

            StorageHostExtension.UseDokan(serviceProvider, options, dokan);
        });

        // 关闭服务
        app.MapPost("/api/server/stop", (StorageDokan dokan)
            => StorageHostExtension.Stop(dokan));

        // 注册win服务
        app.MapPost("/api/server/window-server", ()
            => ServerHelper.AddWinServerAsync());
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
    [JsonIgnore]
    public DokanInstance DokanInstance { get; set; }

    public bool StartMinio { get; set; }

    public StorageDokan StorageDokan { get; set; }

    /// <summary>
    /// 服务启动时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    public StorageDokanInstance(DokanInstance dokanInstance, StorageDokan storageDokan)
    {
        DokanInstance = dokanInstance;
        StorageDokan = storageDokan;
        CreatedTime = DateTime.Now;
    }
}