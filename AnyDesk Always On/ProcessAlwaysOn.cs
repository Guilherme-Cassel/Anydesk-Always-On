using AnyDesk_Always_On;
using System.Diagnostics;

namespace AnydeskAlwaysOn;

public class ProcessAlwaysOn(string processExecutableName) : ProcessManager(processExecutableName)
{
    private int RestartCount = 0;
    public Process Process { get; private set; } = null!;

    public void Start()
    {
        Process = MainProcess() ?? Process.Start(processExecutableName);

        HandleExitEvent();
    }

    private void HandleExitEvent()
    {
        try
        {
            Process.WaitForExit();
            Restart();
        }
        catch (Exception ex)
        {
            throw new HandledException(ex);
        }
    }

    private void Restart()
    {
        RestartCount++;

        Process = Process.Start(processExecutableName);

        InformUserAboutBehavior();

        HandleExitEvent();
    }

    private void InformUserAboutBehavior()
    {
        const string friendlyMessage = "Favor Manter o AnyDesk Aberto, Para Fins de Acesso Remoto";
        const string warningMessage = "PODES PARAR DE FECHAR POR FAVOR?!?!";

        var message = (RestartCount > 20) ? warningMessage : friendlyMessage;

        if (RestartCount > 5)
            MessageBox.Show
                (
                message,
                "AnyDesk Always On",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                default,
                MessageBoxOptions.ServiceNotification
                );
    }
}
