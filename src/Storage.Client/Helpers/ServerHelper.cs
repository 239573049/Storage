using System.Diagnostics;
using System.ServiceProcess;

namespace Storage.Client.Helpers;

public class ServerHelper
{
    public static async Task AddWinServerAsync()
    {

        string code;
        var serviceControllers = ServiceController
            .GetServices().FirstOrDefault(x => x.ServiceName == Constant.ServerName);

        if (serviceControllers == null)
        {
            code = $"sc create {Constant.ServerName} binpath=\"{Path.Combine(AppContext.BaseDirectory, "Storage.Client.exe")}\"  type=own start=auto displayname=Storage";

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
        }
    }
}
