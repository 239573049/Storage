using Storage.Client.Helpers;
using Storage.Client.Options;
using System.Diagnostics;
using System.ServiceProcess;

namespace Storage.Client;

public partial class StorageMain : Form
{
    public StorageMain()
    {
        InitializeComponent();
        MinioGroup.Hide();

        Control.CheckForIllegalCrossThreadCalls = false;

        StorageNotify.ContextMenuStrip = NotifyMenus;

        LoadMapList();
        LoadConfig();
    }

    private void LoadConfig()
    {
        var minio = ConfigHelper.GetMinioOptions();
        var dokan = ConfigHelper.GetDokanOptions();
        AccessKey.Text = minio?.AccessKey;
        SecretKey.Text = minio?.SecretKey;
        BucketName.Text = minio?.BucketName;
        VolumeLabel.Text = minio?.VolumeLabel;
        Port.Text = minio.Port.ToString();
        Endpoint.Text = minio.Endpoint;

        StartDefault.Checked = dokan.StartDefault;
        MountPoint.Text = dokan.MountPoint;
        MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;

        AddWindowServer.Text = ServiceController
            .GetServices().Any(x => x.ServiceName == Constant.ServerName) ? Constant.AddWindowServer : Constant.DeleteWindowServer;
    }

    private void LoadMapList()
    {
        MapList.Items.Add("Minio");
        MapList.Items.Add("Oss");
        MapList.SelectedIndex = 0;
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

    private void MinioMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (StorageHostExtension.StartMinio)
        {
            StorageHostExtension.Stop();
            MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
        }
        else
        {
            Program.ServiceProvider.UseDokan(null, succeed =>
            {
                ServerButton.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
                MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
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
            var port = Convert.ToInt16(Port.Text);

            var minio = new MinioOptions()
            {
                AccessKey = AccessKey.Text,
                SecretKey = SecretKey.Text,
                BucketName = BucketName.Text,
                Endpoint = Endpoint.Text,
                Port = port,
                VolumeLabel = VolumeLabel.Text
            };

            var dokan = new DokanOptions()
            {
                StartDefault = StartDefault.Checked,
                MountPoint = MountPoint.Text,
            };

            ConfigHelper.SaveMinioOptions(minio);
            ConfigHelper.SaveDokanOptions(dokan);
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
            if (StorageHostExtension.StartMinio)
            {
                StorageHostExtension.Stop();
                ServerButton.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
                MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
            }
            else
            {
                Program.ServiceProvider.UseDokan(null, succeed =>
                {
                    ServerButton.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
                    MinioMapToolStripMenuItem.Text = StorageHostExtension.StartMinio ? Constant.StopServer : Constant.StartServer;
                });
            }
        }
        catch (Exception)
        {
        }

    }

    private void MapList_SelectedIndexChanged(object sender, EventArgs e)
    {
        var comboBox = sender as ComboBox;
        var index = comboBox?.SelectedIndex;
        if (index == 0)
        {
            MinioGroup.Show();
        }
        else
        {
            MinioGroup.Hide();
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
        process.StandardInput.WriteLine(code);
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
}