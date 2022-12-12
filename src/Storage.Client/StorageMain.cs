using Storage.Client.Helpers;
using Storage.Client.Options;
using Storage.Client.Storage;
using System.Diagnostics;
using System.ServiceProcess;

namespace Storage.Client;

public partial class StorageMain : Form
{
    public StorageMain()
    {
        InitializeComponent();

        Control.CheckForIllegalCrossThreadCalls = false;

        StorageNotify.ContextMenuStrip = NotifyMenus;

        LoadConfig();
    }

    private void LoadConfig()
    {
        var minIo = ConfigHelper.GetMinIoOptions();
        MinIoAccessKey.Text = minIo?.AccessKey;
        MinIoSecretKey.Text = minIo?.SecretKey;
        MinIoBucketName.Text = minIo?.BucketName;
        MinIoVolumeLabel.Text = minIo?.VolumeLabel;
        MinIoPort.Text = minIo.Port.ToString();
        MinIoEndpoint.Text = minIo.Endpoint;
        StartDefault.Checked = minIo.StartDefault;
        MountPoint.Text = minIo.MountPoint;
        MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
        AddWindowServer.Text = ServiceController
            .GetServices().Any(x => x.ServiceName == Constant.ServerName) ? Constant.AddWindowServer : Constant.DeleteWindowServer;

        var oss = ConfigHelper.GetOssOptions();
        OssAccessKeyId.Text = oss.AccessKeyId;
        OssAccessKeySecret.Text = oss.AccessKeySecret;
        OssBucketName.Text = oss.BucketName;
        OssMountPoint.Text = oss.MountPoint;
        OssEndpoint.Text = oss.Endpoint;
        OssStartDefault.Checked = oss.StartDefault;
        OssMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;

    }


    private void StorageNotify_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        ShowInTaskbar = true;
        Show();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Dispose();
        Close();
    }

    private void MinIoMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (StorageHostExtension.StartMinIo(StorageDokan.MinIo))
        {
            StorageHostExtension.Stop(StorageDokan.MinIo);
            MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
        }
        else
        {
            Program.ServiceProvider.UseDokan(ConfigHelper.GetMinIoOptions(), StorageDokan.MinIo, succeed =>
            {
                ServerButton.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
                MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
            });
        }
    }

    const int WM_SYSCOMMAND = 0x112;
    const int SC_MINIMIZE = 0xF020;


    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_SYSCOMMAND)
        {
            if (m.WParam.ToInt32() == SC_MINIMIZE)
            {
                ShowInTaskbar = false;
                Hide();
            }
        }

        base.WndProc(ref m);
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            var port = Convert.ToInt16(MinIoPort.Text);

            var minIo = new MinIoOptions()
            {
                AccessKey = MinIoAccessKey.Text,
                SecretKey = MinIoSecretKey.Text,
                BucketName = MinIoBucketName.Text,
                Endpoint = MinIoEndpoint.Text,
                Port = port,
                VolumeLabel = MinIoVolumeLabel.Text
            };
            ConfigHelper.SaveMinIoOptions(minIo);
        }
        catch (FormatException)
        {
            MessageBox.Show("映射配置数据错误");
        }
        catch (Exception)
        {
            MessageBox.Show("映射配置数据错误");
        }
    }

    private void ServerButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (StorageHostExtension.StartMinIo(StorageDokan.MinIo))
            {
                StorageHostExtension.Stop(StorageDokan.MinIo);
                ServerButton.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
                MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
            }
            else
            {
                Program.ServiceProvider.UseDokan(ConfigHelper.GetMinIoOptions()!, StorageDokan.MinIo, succeed =>
                {
                    ServerButton.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
                    MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.MinIo) ? Constant.StopServer : Constant.StartServer;
                });
            }
        }
        catch (Exception)
        {
        }

    }

    private void tabControl1_SizeChanged(object sender, EventArgs e)
    {
        MinioGroup.Size = tabControl1.Size;
    }

    private async void AddWindowServer_Click(object sender, EventArgs e)
    {
        var button = sender as Button;
        string? code;
        if (button?.Text == Constant.AddWindowServer)
        {
            code = $"sc create {Constant.ServerName} binpath=\"{Path.Combine(AppContext.BaseDirectory, "Storage.Client.exe")}\"  type=own start=auto displayname=Storage";
        }
        else
        {
            code = $"sc stop {Constant.ServerName} & sc delete {Constant.ServerName}";
        }
        Process process = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            }
        };
        process.Start();
        await process.StandardInput.WriteLineAsync(code);
        process.StandardInput.Close();
        await process.WaitForExitAsync();
        Debug.WriteLine(process.StandardOutput.ReadToEnd());

        var serviceControllers = ServiceController
            .GetServices().FirstOrDefault(x => x.ServiceName == Constant.ServerName);
        AddWindowServer.Text = serviceControllers == null ? Constant.AddWindowServer : Constant.DeleteWindowServer;
    }

    private void StorageMain_Shown(object sender, EventArgs e)
    {
        ShowInTaskbar = false;
        Hide();
    }

    private void OssSaveConfig_Click(object sender, EventArgs e)
    {
        try
        {
            var ossOptions = new OssOptions()
            {
                AccessKeyId = OssAccessKeyId.Text,
                AccessKeySecret = OssAccessKeySecret.Text,
                BucketName = OssBucketName.Text,
                Endpoint = OssEndpoint.Text,
                MountPoint = OssMountPoint.Text,
                StartDefault = OssStartDefault.Checked,
                VolumeLabel = OssVolumeLabel.Text
            };
            ConfigHelper.SaveOssOptions(ossOptions);
        }
        catch (Exception)
        {
            MessageBox.Show("映射配置数据错误");
        }
    }

    private void OssStartServer_Click(object sender, EventArgs e)
    {
        try
        {
            if (StorageHostExtension.StartMinIo(StorageDokan.MinIo))
            {
                StorageHostExtension.Stop(StorageDokan.Oss);
                OssServerButton.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
                OssMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
            }
            else
            {
                Program.ServiceProvider.UseDokan(ConfigHelper.GetMinIoOptions()!, StorageDokan.Oss, succeed =>
                {
                    OssServerButton.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
                    OssMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
                });
            }
        }
        catch (Exception)
        {
        }
    }

    private void OssMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (StorageHostExtension.StartMinIo(StorageDokan.Oss))
        {
            StorageHostExtension.Stop(StorageDokan.Oss);
            MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
        }
        else
        {
            Program.ServiceProvider.UseDokan(ConfigHelper.GetOssOptions(), StorageDokan.Oss, succeed =>
            {
                ServerButton.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
                MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinIo(StorageDokan.Oss) ? Constant.StopServer : Constant.StartServer;
            });
        }
    }
}