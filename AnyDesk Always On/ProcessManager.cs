using AnyDesk_Always_On;
using System.Diagnostics;

namespace AnydeskAlwaysOn;

public partial class ProcessManager(string processExecutableName) : IDisposable
{
    public Process? MainProcess() => GetProcessesListByName().FirstOrDefault();

    public List<Process> ListProcess() => GetProcessesListByName();

    private List<Process> GetProcessesListByName()
    {
        var formattedProcessName = Path.GetFileNameWithoutExtension(processExecutableName).Trim();

        var AllProcesses = Process.GetProcessesByName(formattedProcessName);
        List<Process> myProcesses = [];

        foreach (var item in AllProcesses)
        {
            try
            {
                if (IsProcessAccessible(item))
                    myProcesses.Add(item);
            }
            catch (Exception ex)
            {
                throw new ProcessCapturingException(ex);
            }
        }

        return myProcesses;
    }

    private static bool IsProcessAccessible(Process process)
    {
        try
        {
            // Attempt to access the MainModule property to see if it's accessible
            if (process.MainModule != null) { /* ᕦ(ò_óˇ)ᕤ */ }
            return true;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // Access denied to MainModule
            return false;
        }
    }

    public void Dispose()
    {
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.SuppressFinalize(this);
    }
}

