using Storage.Client;
using Storage.Client.Helpers;
using Storage.Client.Storage;
using System.Runtime.InteropServices;

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
services.AddControllers();
services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.SetIsOriginAllowed((string _) => true).AllowAnyMethod().AllowAnyHeader()
            .AllowCredentials();
    });
});
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

app.UseCors("CorsPolicy");
app.UseStaticFiles();
app.MapControllers();
app.UseApi();
app.Run();

