﻿using DokanNet;
using Storage.Client.Helpers;
using Storage.Host;
using Storage.Host.Storage;
using DokanOptions = DokanNet.DokanOptions;

namespace Storage.Client;

public static class StorageHostExtension
{
    private static DokanInstance? _dokanInstance;
    private static bool startMinio;

    public static bool StartMinio => startMinio;

    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddSingleton<IDokanOperations, IntegrationOperations>();

        return services;
    }

    public static IServiceCollection AddMinio(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, MinioService>();

        return services;
    }

    public static void UseDokan(this IServiceProvider app, Options.DokanOptions? dokanOptions = null, Action<bool>? succeed = null)
    {
        _ = Task.Factory.StartNew(() =>
        {
            if (startMinio)
            {
                succeed?.Invoke(startMinio);
                return;
            }

            try
            {
                startMinio = true;
                dokanOptions ??= ConfigHelper.GetDokanOptions();

                var manualReset = new ManualResetEvent(false);
                var dokan = new Dokan(new DokanNet.Logging.NullLogger());

                var dokanOperations = app.GetRequiredService<IDokanOperations>();

                var dokanBuilder = new DokanInstanceBuilder(dokan)
                    .ConfigureLogger(() => new DokanNet.Logging.NullLogger())
                    .ConfigureOptions(options =>
                    {
                        options.Options = DokanOptions.FixedDrive;
                        options.MountPoint = dokanOptions.MountPoint;
                    });

                _dokanInstance = dokanBuilder.Build(dokanOperations);

                succeed?.Invoke(startMinio);
                manualReset.WaitOne();
            }
            catch (Exception e)
            {
                startMinio = false;
                succeed?.Invoke(startMinio);
                MessageBox.Show("启动挂载硬盘驱动时错误：{0}", e.Message);
            }
        });

    }

    /// <summary>
    /// 释放Minio服务
    /// </summary>
    public static void Stop()
    {
        try
        {
            if (startMinio)
            {
                _dokanInstance?.Dispose();
                startMinio = false;
            }
        }
        catch
        {
        }
    }
}