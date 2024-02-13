using AnydeskAlwaysOn.Properties;
using System.Diagnostics;

namespace AnydeskAlwaysOn;

public static class AnyDeskProcessManager
{
    public const string ProcessName = "Anydesk Always On";

    public static Process MainProcess() => GetProcessesListByName().First();

    public static List<Process> ListProcess() => GetProcessesListByName();

    public static void EnsureSingleInstance()
    {
        List<Process> anyDeskAlwaysOnProcesses = ListProcess();

        if (anyDeskAlwaysOnProcesses.Count > 1)
        {
            for (int i = 1; i < anyDeskAlwaysOnProcesses.Count; i++)
            {
                anyDeskAlwaysOnProcesses[i].Kill();
            }
        }

        List<Process> anyDeskProcesses = GetProcessesListByName("AnyDesk");

        if (anyDeskProcesses.Count <= 0)
        {
            Process.Start(Resources.AnyDeskExecutablePath);
            Thread.Sleep(100);
        }
    }

    private static List<Process> GetProcessesListByName(string processName = ProcessName)
    {
        var processes = Process.GetProcessesByName(processName)
            .ToList();

        processes.RemoveAll(x => x.MainWindowHandle == IntPtr.Zero);

        return processes;
    }
}
