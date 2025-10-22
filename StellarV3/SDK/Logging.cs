using MelonLoader;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StellarV3External.SDK
{
    internal static class Logging
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        public static void InitConsole()
        {
            var consoleHandle = GetConsoleWindow();
            if (consoleHandle != IntPtr.Zero)
                ShowWindow(consoleHandle, SW_HIDE);

            string logReaderPath = Path.Combine(Environment.CurrentDirectory, "ConsoleLogs.exe");
            if (File.Exists(logReaderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = logReaderPath,
                    UseShellExecute = true
                });
            }
            else
            {
                MelonLogger.Msg("[Error] ~ ConsoleLogs.exe not found at: " + logReaderPath);
            }


            Log("ConsoleLogs.exe launched, original console hidden.", LType.Success);
        }

        public static void Log(string message, LType type = LType.Info)
        {
            if (type == LType.Info)
                Console.ForegroundColor = ConsoleColor.White;
            else if (type == LType.Warning)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (type == LType.Error)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (type == LType.Success)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (type == LType.Debug)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (type == LType.Join)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (type == LType.Leave)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("[" + type.ToString() + "]" + " ~ " + message);
            MelonLogger.Msg("[" + type.ToString() + "]" + " ~ " + message);
            Console.ResetColor();
        }
    }

    internal enum LType
    {
        Info,
        Warning,
        Error,
        Success,
        Debug,
        Join,
        Leave
    }
}
