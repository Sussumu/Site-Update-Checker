using IWshRuntimeLibrary;
using System;

namespace UpdateChecker.Lib
{
    public static class Utils
    {
        static private string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        static public void CreateShortcutStartWithWindows(string fileName)
        {
            WshShell shell = new WshShell();
            string shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "//" + fileName + ".lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.WorkingDirectory = Environment.CurrentDirectory;
            shortcut.TargetPath = Environment.CurrentDirectory + "//" + fileName + ".exe";
            shortcut.Save();
        }

        static public void EraseFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }
    }
}
