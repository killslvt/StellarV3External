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

        private const string ConsoleLogsUrl = "https://github.com/killslvt/ClarityLib/releases/download/Beta/ClarityConsole.exe";
        private const string ClarityPluginUrl = "https://github.com/killslvt/Clarity/releases/download/v1.0.0/Clarity.dll";

        public static void InitConsole()
        {
            if (MelonUtils.IsUnderWineOrSteamProton())
            {
                ClarityLib.Logs.DisableAnsi = true;
                ClarityLib.Logs.Log("Wine/Proton detected, ansi disabled", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
                Log("Wine/Proton detected, ansi disabled", LType.Info);
            }
            else
            {
                ClarityLib.Logs.DisableAnsi = false;
                ClarityLib.Logs.Log("Wine/Proton not detected, ansi enabled", LType.Info.ToString(), Logging.GetColor(LType.Info), System.ConsoleColor.Cyan, "Stellar");
                Log("Wine/Proton not detected, ansi enabled", LType.Info);
            }

            var consoleHandle = GetConsoleWindow();
            string consoleLogPath = Path.Combine(Environment.CurrentDirectory, "ClarityConsole.exe");

            ShowWindow(consoleHandle, SW_HIDE);

            if (!File.Exists(consoleLogPath))
            {
                MelonLogger.Msg("[Info] ~ ConsoleLogs.exe not found. Attempting to download...");
                try
                {
                    using (var client = new HttpClient())
                    {
                        var data = client.GetByteArrayAsync(ConsoleLogsUrl).GetAwaiter().GetResult();
                        File.WriteAllBytes(consoleLogPath, data);
                        MelonLogger.Msg("[Success] ~ ConsoleLogs.exe downloaded successfully.");
                    }
                }
                catch (Exception e)
                {
                    MelonLogger.Msg("[Error] ~ Failed to download ConsoleLogs.exe: " + e.Message);
                    ShowWindow(consoleHandle, SW_SHOW);
                    return;
                }
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = consoleLogPath,
                    UseShellExecute = true
                });
                ClarityLib.Logs.Log("ConsoleLogs.exe launched, original console hidden", LType.Success.ToString(), Logging.GetColor(LType.Success), System.ConsoleColor.Cyan, "Stellar");
            }
            catch (Exception e)
            {
                MelonLogger.Msg("[Error] ~ Failed to launch ConsoleLogs.exe: " + e.Message);
                if (consoleHandle != IntPtr.Zero)
                    ShowWindow(consoleHandle, SW_SHOW);
            }
        }

        //old logging method
        public static void Log(string message, LType type = LType.Info)
        {
            MelonLogger.Msg("[" + type.ToString() + "]" + " ~ " + message);
        }

        public static ConsoleColor GetColor(LType type)
        {
            return type switch
            {
                LType.Info => ConsoleColor.White,
                LType.Warning => ConsoleColor.Yellow,
                LType.Error => ConsoleColor.Red,
                LType.Success => ConsoleColor.Green,
                LType.Debug => ConsoleColor.Cyan,
                LType.Join => ConsoleColor.Green,
                LType.Leave => ConsoleColor.Red,
                _ => ConsoleColor.Gray,
            };
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
