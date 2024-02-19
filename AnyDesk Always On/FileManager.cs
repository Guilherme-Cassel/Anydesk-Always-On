using IWshRuntimeLibrary;

namespace AnyDesk_Always_On;

public static class FileManager
{
    private static readonly string StartupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "AnyDesk Always On.lnk");
    private static readonly string CurrentExePath = Environment.ProcessPath!;
    public static void CreateShortcutOnStartup()
    {
        if (System.IO.File.Exists(StartupShortcutPath))
            return;

        IWshShell_Class wshShell = new();

        IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(StartupShortcutPath);

        shortcut.TargetPath = CurrentExePath;

        shortcut.Save();
    }
}
