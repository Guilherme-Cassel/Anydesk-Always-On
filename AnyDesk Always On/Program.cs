using AnyDesk_Always_On;

namespace AnydeskAlwaysOn;

class Program
{
    static void Main()
    {
        try
        {
            EnsureSingleInstance();
            FileManager.CreateShortcutOnStartup();

            ProcessAlwaysOn processAlwaysOn = new("Application\\AnyDesk.exe");
            processAlwaysOn.Start();
        }
        catch (Exception ex)
        {
            throw new HandledFatalException(ex);
        }
    }

    static void EnsureSingleInstance()
    {
        var appName = AppDomain.CurrentDomain.FriendlyName;

        using ProcessManager processManager = new(appName);

        if (processManager.ListProcess().Count > 1)
        {
            MessageBox.Show(
            "Já Existe uma Instancia do Software Sendo Executada!",
            "AnyDesk Always On"
            );

            Environment.Exit(0);
        }
    }
}
