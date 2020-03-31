using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using CommonLib.StringTools;

namespace WpfApp1.WpfTools
{
    public static class CurrentExe
    {
        public static string GetFullPath()
            => Assembly.GetEntryAssembly()?.Location;


        public static string GetShortName()
            => Path.GetFileNameWithoutExtension(GetFullPath());


        public static string GetDirectory()
        {
            var exe = GetFullPath();
            if (exe.IsBlank()) return string.Empty;
            return Directory.GetParent(exe).FullName;
        }


        public static string GetVersion()
        {
            var exe = GetFullPath();
            return exe.IsBlank() ? "" : exe.GetVersion();
        }


        public static string GetShortVersion()
        {
            var exe = GetFullPath();
            return exe.IsBlank() ? "" : exe.GetShortVersion();
        }


        public static string ShortNameAndVersion()
            => $"{GetShortName()} v{GetShortVersion()}";


        public static void Shutdown()
            => UIThread.Run(() 
                => Application.Current?.Shutdown());


        public static void RelaunchApp()
        {
            //Process.Start(GetFullPath());
            var orig = Environment.GetCommandLineArgs();
            var args = orig.QuotifyCommandLineArgs();
            Process.Start(orig[0], args);
            CurrentExe.Shutdown();
        }


        public static Process   GetProcess    () => Process.GetCurrentProcess();
        public static Process[] GetProcesses  () => Process.GetProcessesByName(GetProcessName());
        public static string    GetProcessName() => GetProcess().ProcessName;


        public static int CountRunningInstances() 
            => GetProcesses().Length;


        public static bool HasAnotherInstance()
            => CountRunningInstances() > 1;


        private static string GetVersion(this string filePath)
            => FileVersionInfo.GetVersionInfo(filePath).FileVersion;


        private static string GetShortVersion(this string filePath)
        {
            var ver = filePath.GetVersion();
            if (ver.IsBlank()) return "";
            var ss = ver.Split('.');
            if (ss.Length != 4) return ver;
            return $"{ss[2]}.{int.Parse(ss[3])}";
        }
    }
}
