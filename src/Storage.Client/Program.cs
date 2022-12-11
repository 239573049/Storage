using Storage.Client.Helpers;
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

        services.AddStorage();
        services.AddMinio();

        var app = builder.Build();

        var option = ConfigHelper.GetDokanOptions();

        if (option?.StartDefault == true)
        {
            app.Services.UseDokan(null);
        }

        ServiceProvider = app.Services;

        _ = Task.Run(() =>
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new StorageMain());
        });

        app.Run();

    }
}