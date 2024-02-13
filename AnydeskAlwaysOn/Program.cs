using System.Diagnostics;

namespace AnydeskAlwaysOn;
class Program
{
    public static event EventHandler? ProcessDied;
    private static string AnyDeskExecutablePath = @$"{Environment.GetEnvironmentVariable("USERPROFILE")}\AppData\Local\AnyDesk\AnyDesk.exe";
    private static string NeededFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"NeededFiles\");

    static void Main()
    {
        CheckIfThereIsntAlreadyAnInstanceOfThisProgram();

        ProcessDied += Process_Died;

        CheckIfAnyDeskExecutableExists();

        MakeSureThereIsAnInstanceOfAnyDeskOpen();

        AttachEventToProcess(MainProcess("AnyDesk"), ProcessDied);
    }

    private static Process MainProcess(string processName)
    {
        return GetProcessesListByName(processName)[0];
    }

    private static List<Process> ListProcess(string processName)
    {
        return GetProcessesListByName(processName);
    }

    ////////////////////////////////////////////////////////////////

    private static void MakeSureThereIsAnInstanceOfAnyDeskOpen()
    {
        if (ListProcess("AnyDesk").Count <= 0)
        {
            Process.Start(AnyDeskExecutablePath);
            Thread.Sleep(1000);
        }
    }

    private static void CheckIfAnyDeskExecutableExists()
    {
        if (!File.Exists(AnyDeskExecutablePath))
        {
            CopyNeededFiles(AnyDeskExecutablePath);
        }
    }

    private static void CopyNeededFiles(string destiny)
    {
        var DirectoryPathWithoutSufix = destiny.Remove(destiny.IndexOf("AnyDesk.exe"));

        if (NeededFiles == null)
            return;

        Directory.CreateDirectory(DirectoryPathWithoutSufix);

        DirectoryInfo directory = new DirectoryInfo(NeededFiles);
        foreach (FileInfo file in directory.GetFiles())
        {
            file.CopyTo(DirectoryPathWithoutSufix + file.Name, false);
        }
    }

    private static void CheckIfThereIsntAlreadyAnInstanceOfThisProgram()
    {
        List<Process> thisProcesses = ListProcess("AnyDesk Always On");
        if (thisProcesses.Count > 1)
        {
            for (int i = 1; i < thisProcesses.Count; i++)
            {
                thisProcesses[i].Kill();
            }
        }
    }

    private static List<Process> GetProcessesListByName(string processName)
    {
        Process[] processes = Process.GetProcessesByName(processName);

        List<Process> correctProcesses = new List<Process>();
        foreach (Process process in processes)
        {
            try
            {
                string processPath = process.MainModule.FileName;
                correctProcesses.Add(process);
            }
            catch (Exception) { }
        }

        if (correctProcesses.Count == 0)
            return new List<Process>();

        return correctProcesses;
    }

    private static void AttachEventToProcess(Process process, EventHandler? e)
    {
        Process isSelectedProcess = Process.GetProcessById(process.Id);

        if (isSelectedProcess != null)
        {
            isSelectedProcess.WaitForExit();
        }
        if (e != null)
        {
            e.Invoke(isSelectedProcess, new EventArgs());
        }
    }

    private static void Process_Died(object? sender, EventArgs e)
    {
#if DEBUG
        Console.WriteLine("Anydesk Closed!! Re-opening...");
#endif
        List<Process> leftOverAnyDeskProcesses = ListProcess("AnyDesk");
        foreach (Process anydeskProcess in leftOverAnyDeskProcesses)
        {
            anydeskProcess.Kill(); // kill the leftover sub-processes of anydesk
        }

        Thread.Sleep(1000);

        Process process = Process.Start(AnyDeskExecutablePath);

        Thread.Sleep(1000);

        AttachEventToProcess(process /*firt index is the key process!*/, ProcessDied);
    }
}