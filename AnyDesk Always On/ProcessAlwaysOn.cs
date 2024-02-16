using System.Diagnostics;

namespace AnydeskAlwaysOn;

public class ProcessAlwaysOn(string processExecutableName) : ProcessManager(processExecutableName)
{
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
        Process = Process.Start(processExecutableName);

        HandleExitEvent();
    }
}
