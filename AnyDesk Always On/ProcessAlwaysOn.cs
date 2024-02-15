using System.Diagnostics;

namespace AnydeskAlwaysOn;

public class ProcessAlwaysOn(string processExecutableName) : ProcessManager
{
    public Process Process { get; private set; } = null!;
    public string ProcessExecutableName { get; } = processExecutableName;

    public void Start()
    {
        Process = MainProcess(ProcessExecutableName) ?? Process.Start(ProcessExecutableName);
        HandleExitEvent();
    }

    private void HandleExitEvent()
    {
        try
        {
            Process.WaitForExit();
            RestartProcess();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erro Capturado com Sucesso! Contate seu TI", MessageBoxButtons.OK);
        }
    }

    private void RestartProcess()
    {
        Process = Process.Start(ProcessExecutableName);

        HandleExitEvent();
    }
}
