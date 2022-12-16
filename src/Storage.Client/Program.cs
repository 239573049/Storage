using Storage.Client.Helpers;
using Storage.Client.Storage;
using System.Runtime.InteropServices;

namespace Storage.Client;

internal static class Program
{
    public static IServiceProvider ServiceProvider;
    /// <summary>
    ///  
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        });

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.Host.UseWindowsService();
        }


        var services = builder.Services;
        services.AddStorage(builder.Configuration);

        var app = builder.Build();

        var minIoOptions = ConfigHelper.GetMinIoOptions();
        var ossOptions = ConfigHelper.GetOssOptions();

        if (ossOptions?.StartDefault == true)
        {
            app.Services.UseDokan(ossOptions, StorageDokan.Oss);
        }

        if (minIoOptions?.StartDefault == true)
        {
            app.Services.UseDokan(minIoOptions, StorageDokan.MinIo);
        }

        ServiceProvider = app.Services;

        _ = Task.Run(() =>
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new StorageMain());
        });

        app.UseApi();
        app.Run();
    }

    private static void UseApi(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/minio-config", () => "");
    }
}