using DokanNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Storage.Client.Helpers;
using Storage.Client.Options;
using Storage.Host;
using Storage.Host.Storage;
using DokanOptions = DokanNet.DokanOptions;

namespace Storage.Client;

public static class StorageHostExtension
{
    private static DokanInstance? _dokanInstance;
    private static bool startMinio;

    public static bool StartMinio => startMinio;

    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDokanOperations, IntegrationOperations>();

        var section = configuration.GetSection(nameof(DokanOptions));
        services.Configure<Client.Options.DokanOptions>(section);

        return services;
    }

    public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(MinioOptions));
        services.Configure<MinioOptions>(section);

        services.AddSingleton<IStorageService, MinioService>();

        return services;
    }

    public static void UseDokan(this IServiceProvider app, Client.Options.DokanOptions? dokanOptions = null)
    {
        _ = Task.Factory.StartNew(() =>
        {
            if (startMinio)
            {
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
                manualReset.WaitOne();
            }
            catch (Exception e)
            {
                startMinio = false;
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