using System.Diagnostics;

namespace AnydeskAlwaysOn;

class Program
{
    static void Main()
    {
        EnsureSingleInstance();

        ProcessAlwaysOn processAlwaysOn = new("AnyDesk.exe");
        processAlwaysOn.Start();
    }
    static void EnsureSingleInstance()
    {
        ProcessManager? processManager = new(AppDomain.CurrentDomain.FriendlyName);

        if (processManager.ListProcess().Count > 1)
        {
            MessageBox.Show(
            "Já Existe uma Instancia do Software Sendo Executada!",
            "AnyDesk Always On"
            );

            Process.GetCurrentProcess().Kill();
        }
    }
}
