namespace AnydeskAlwaysOn;

class Program
{
    static void Main()
    {
        using (ProcessManager? processManager = new(AppDomain.CurrentDomain.FriendlyName))
        {
            if (processManager.ListProcess().Count > 1)
            {
                MessageBox.Show(
                    "Já Existe uma Instancia do Software Sendo Executada!",
                    "AnyDesk Always On"
                    );
                return;
            }
        }


        ProcessAlwaysOn processAlwaysOn = new
            (
                "AnyDesk.exe"
            );
        processAlwaysOn.Start();
    }
}
