using DokanNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Storage.Host.Options;
using Storage.Host.Storage;
using DokanOptions = DokanNet.DokanOptions;

namespace Storage.Host;

public static class StorageHostExtension
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDokanOperations, IntegrationOperations>();

        var section = configuration.GetSection(nameof(DokanOptions));
        services.Configure<Options.DokanOptions>(section);

        return services;
    }

    public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(MinioOptions));
        services.Configure<MinioOptions>(section);

        services.AddSingleton<IStorageService, MinioService>();

        return services;
    }

    public static void UseDokan(this IServiceProvider app)
    {
        Task.Factory.StartNew(() =>
        {
            var dokanOptions = app.GetRequiredService<IOptions<Options.DokanOptions>>().Value;

            var manualReset = new ManualResetEvent(false);
            var dokan = new Dokan(new DokanNet.Logging.NullLogger());

            var dokanOperations = app.GetRequiredService<IDokanOperations>();

            var dokanBuilder = new DokanInstanceBuilder(dokan)
                .ConfigureLogger(() => new DokanNet.Logging.NullLogger())
                .ConfigureOptions(options =>
                {
                    options.Options = DokanOptions.FixedDrive;
                    options.MountPoint = dokanOptions!.MountPoint;
                });

            using (var dokanInstance = dokanBuilder.Build(dokanOperations))
            {
                manualReset.WaitOne();
            }
        });
    }
}