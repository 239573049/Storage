using System.Text;

namespace Storage.Client;

internal static class Program
{
    /// <summary>
    ///  
    /// </summary>
    [STAThread]
    static void Main()
    {  //����Ӧ�ó������쳣��ʽ��ThreadException����
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        //����UI�߳��쳣
        Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        //�����UI�߳��쳣
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        ApplicationConfiguration.Initialize();
        Application.Run(new StorageMain());
    }



    static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        string str = GetExceptionMsg(e.Exception, e.ToString());
        MessageBox.Show(str, "ϵͳ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
        MessageBox.Show(str, "ϵͳ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    /// <summary>
    /// �����Զ����쳣��Ϣ
    /// </summary>
    /// <param name="ex">�쳣����</param>
    /// <param name="backStr">�����쳣��Ϣ����exΪnullʱ��Ч</param>
    /// <returns>�쳣�ַ����ı�</returns>
    static string GetExceptionMsg(Exception ex, string backStr)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("****************************�쳣�ı�****************************");
        sb.AppendLine("������ʱ�䡿��" + DateTime.Now.ToString());
        if (ex != null)
        {
            sb.AppendLine("���쳣���͡���" + ex.GetType().Name);
            sb.AppendLine("���쳣��Ϣ����" + ex.Message);
            sb.AppendLine("����ջ���á���" + ex.StackTrace);
        }
        else
        {
            sb.AppendLine("��δ�����쳣����" + backStr);
        }
        sb.AppendLine("***************************************************************");
        return sb.ToString();
    }

}