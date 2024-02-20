using IWshRuntimeLibrary;

namespace AnyDeskAlwaysOn.Model;

public static class FileManager
{
    private static readonly string StartupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "AnyDesk Always On.lnk");
    private static readonly string CurrentExePath = Environment.ProcessPath!;

    public static void CreateShortcutOnStartup()
    {
        if (System.IO.File.Exists(StartupShortcutPath))
            return;

        IWshShell_Class wshShell_Class = new();

        IWshShortcut wshShortcut = (IWshShortcut)wshShell_Class.CreateShortcut(StartupShortcutPath);

        wshShortcut.TargetPath = CurrentExePath;

        wshShortcut.Save();
    }
}

