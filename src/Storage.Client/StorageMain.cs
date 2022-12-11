using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Client.Helpers;

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
        services.AddMasaBlazor();
        BlazorWebView.HostPage = "wwwroot\\index.html";
        BlazorWebView.Services = services.BuildServiceProvider();
        BlazorWebView.RootComponents.Add<Main>("#app");
        var option = ConfigHelper.GetDokanOptions();
        if (option.StartDefault)
        {
            BlazorWebView.Services.UseDokan();
        }
    }

    private void StorageNotify_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        Show();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Dispose();
        Close();
    }

    private void MinioMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (StorageHostExtension.StartMinio)
        {
            StorageHostExtension.Stop();
        }
        else
        {
            BlazorWebView.Services.UseDokan();
        }
    }

    const int WM_SYSCOMMAND = 0x112;
    const int SC_MINIMIZE = 0xF020;


    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_SYSCOMMAND)
        {
            if (m.WParam.ToInt32() == SC_MINIMIZE)  //拦截最小化按钮
            {
                ShowInTaskbar = false;
                Hide();
            }
        }
        base.WndProc(ref m);
    }
}
