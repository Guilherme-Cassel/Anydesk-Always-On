using System.Diagnostics;

namespace AnyDeskAlwaysOn.Model;

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
            // Attempt to access the MainModule property to see if it's accessible ᕦ(ò_óˇ)ᕤ
            var testIfAcessible = process.MainModule;
            return true;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // Access denied to MainModule
            return false;
        }
    }

    public static void EnsureSingleInstance(string instanceName)
    {
        using ProcessManager processManager = new(instanceName);

        if (processManager.ListProcess().Count > 1)
        {
            throw new DuplicatedInstanceException();
        }
    }

    public void Dispose()
    {
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.SuppressFinalize(this);
    }
}

