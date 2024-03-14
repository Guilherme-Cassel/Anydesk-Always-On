using System.Diagnostics;
using System.Management;

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
        string GetProcessOwner()
        {
            string processOwner = "Unknown";

            try
            {
                string query = "Select * From Win32_Process Where ProcessID = " + process.Id;

                using ManagementObjectSearcher? searcher = new(query);
                using ManagementObjectCollection? processList = searcher.Get();

                foreach (var obj in processList)
                {
                    var mo = (ManagementObject)obj;
                    string[] argList = [string.Empty, string.Empty];
                    int returnValue = Convert.ToInt32(mo.InvokeMethod("GetOwner", argList));
                    if (returnValue == 0)
                    {
                        // Index 0 is the user name
                        processOwner = argList.First();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HandledException(ex);
            }

            return processOwner;
        }

        try
        {
            var testIfAcessible = process.MainModule;

            if (GetProcessOwner() != Environment.UserName)
                throw new Exception();

            return true;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // Access denied to MainModule
            return false;
        }
        catch (Exception)
        {
            // Process Owner is not current user
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

