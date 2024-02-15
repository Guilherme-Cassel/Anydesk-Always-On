namespace AnydeskAlwaysOn;

class Program : ProcessManager
{
    static void Main()
    {
        if (ListProcess(AppDomain.CurrentDomain.FriendlyName).Count > 1)
            return;

        ProcessAlwaysOn processAlwaysOn = new
        (
            "AnyDesk.exe"
        );
        processAlwaysOn.Start();
    }
}