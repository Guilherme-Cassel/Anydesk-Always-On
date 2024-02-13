using AnydeskAlwaysOn.Properties;
using System.Diagnostics;

namespace AnydeskAlwaysOn;
class Program
{
    public static event EventHandler? ProcessDied;

    static void Main()
    {
        AnyDeskProcessManager.EnsureSingleInstance();

        ProcessDied += Process_Died;

        CheckIfAnyDeskExecutableExists();

        MakeSureThereIsAnInstanceOfAnyDeskOpen();

        AttachEventToProcess(MainProcess("AnyDesk"), ProcessDied);
    }



    ////////////////////////////////////////////////////////////////

    private static void MakeSureThereIsAnInstanceOfAnyDeskOpen()
    {
        
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