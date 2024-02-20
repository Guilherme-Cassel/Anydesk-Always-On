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

        foreach (var process in AllProcesses)
        {
            try
            {
                var isMine = process.MainModule;
                myProcesses.Add(process);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                //Access Denied To Property
            }
            catch (Exception ex)
            {
                throw new ProcessCapturingException(ex);
            }
        }

        return myProcesses;
    }

    public void Dispose()
    {
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.SuppressFinalize(this);
    }
}

