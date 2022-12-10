using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Host;

namespace Storage.Client;

public partial class StorageMain : Form
{
    public StorageMain()
    {
        InitializeComponent();

        StorageNotify.ContextMenuStrip = NotifyMenus;

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        services.AddWindowsFormsBlazorWebView();
        services.AddSingleton(configuration);
        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddStorage(configuration);
        services.AddMinio(configuration);

        BlazorWebView.HostPage = "wwwroot\\index.html";
        BlazorWebView.Services = services.BuildServiceProvider();
        BlazorWebView.RootComponents.Add<Main>("#app");
        BlazorWebView.Services.UseDokan();
    }

    private void StorageNotify_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }
        Activate();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Dispose();
        Close();
    }
}
