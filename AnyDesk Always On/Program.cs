using System.Diagnostics;

namespace AnydeskAlwaysOn;

class Program
{
    static void Main()
    {
        if (new ProcessManager(AppDomain.CurrentDomain.FriendlyName).ListProcess().Count > 1)
        {
            MessageBox.Show("Já existe uma instancia do AnyDesk Always On Rodando!");
            return;
        }

        ProcessAlwaysOn processAlwaysOn = new
        (
            "AnyDesk.exe"
        );
        processAlwaysOn.Start();
    }
}