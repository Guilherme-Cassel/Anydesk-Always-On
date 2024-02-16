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
            MessageBox.Show(ex.Message, "Erro Capturado com Sucesso! Contate seu TI", MessageBoxButtons.OK);
        }
    }

    private void Restart()
    {
        RestartCount++;

        Process = Process.Start(processExecutableName);

        if (RestartCount > 5)
            MessageBox.Show
                (
                (RestartCount > 20) ? "PODES PARAR DE FECHAR POR FAVOR?!?!" : "Favor Manter o AnyDesk Aberto, Para Fins de Acesso Remoto",
                "AnyDesk Always On",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                default,
                MessageBoxOptions.ServiceNotification
                );

        HandleExitEvent();
    }
}
