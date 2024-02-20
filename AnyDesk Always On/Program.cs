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
        using Mutex singleton = new (true, AppDomain.CurrentDomain.FriendlyName);

        if (!singleton.WaitOne(TimeSpan.Zero, true)) //there is already another instance running!
        {
            MessageBox.Show(
                "Já Existe uma Instancia do Software Sendo Executada!",
                "AnyDesk Always On");
            
            Environment.Exit(0);
        }
    }
}
