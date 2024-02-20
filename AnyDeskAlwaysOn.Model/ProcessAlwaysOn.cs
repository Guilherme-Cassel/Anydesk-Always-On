using System.Diagnostics;

namespace AnyDeskAlwaysOn.Model;

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
            throw new HandledException(ex);
        }
    }

    private void Restart()
    {
        Process = Process.Start(processExecutableName);

        HandleExitEvent();
    }
}
