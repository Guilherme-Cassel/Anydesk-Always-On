using System.Diagnostics;

namespace AnydeskAlwaysOn;

public class ProcessManager
{
    public static Process? MainProcess(string processName) => GetProcessesListByName(processName).FirstOrDefault();

    public static List<Process> ListProcess(string processName) => GetProcessesListByName(processName);

    private static List<Process> GetProcessesListByName(string processName)
    {
        processName = processName.Replace(".exe", string.Empty);

        var AllProcesses = Process.GetProcessesByName(processName);
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
                throw new Exception(@"Houve um erro ao capturar os processos referentes ao AnyDesk Always On" , ex);
            }
        }

        return myProcesses;
    }
}
