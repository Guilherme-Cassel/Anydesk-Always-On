using AnyDeskAlwaysOn.Model;

namespace Anydesk_Always_On;

class Program
{
    static void Main()
    {
        try
        {
            ProcessManager.EnsureSingleInstance(AppDomain.CurrentDomain.FriendlyName);
            FileManager.CreateShortcutOnStartup();

            ProcessAlwaysOn processAlwaysOn = new("Application\\AnyDesk.exe");
            processAlwaysOn.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show
                    (ex.Message,
                    "AnyDesk Always On",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    default);

            Environment.Exit(0);
        }
    }
}
