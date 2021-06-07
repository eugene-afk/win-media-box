using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;

namespace WinMediaBox.Classes.Tools
{
    public static class SendKeys
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("User32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);
        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        const int KEYEVENTF_KEYUP = 0x0002;

        const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;  //The left button was pressed

        const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;  //The left button was released.

        private static Timer _timer;

        public static async Task Send(string procName, byte key, bool foreground = true, int delay = 1000)
        {
            if (foreground)
            {
                bool isForegrounded = SetForegroundWindow(procName);
                if (!isForegrounded)
                {
                    return;
                }
            }
            await Task.Delay(delay);
            keybd_event(key, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(key, 0, KEYEVENTF_KEYUP, 0);
        }

        public static async Task SendMouseLeft()
        {
            await Task.Delay(1000);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        public static bool SetForegroundWindow(string procName)
        {
            var processes = Process.GetProcessesByName(procName);
            if(processes.Length == 0)
            {
                return false;
            }
            SetForegroundWindow(processes[0].MainWindowHandle);
            ShowWindow(processes[0].MainWindowHandle, int.Parse("9"));
            SetCursorPos(50, 250);
            return true;
        }

        public static void ShutDown(int timer = 0)
        {
            var psi = new ProcessStartInfo("shutdown", " -f /s /t " + timer);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }

        public static void SuspendWithTimer(double timer = 0)
        {
            _timer = new Timer(timer);
            _timer.Elapsed += OnTimerSuspendEvent;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        private static void OnTimerSuspendEvent(object s, ElapsedEventArgs e)
        {
            SetSuspendState(false, true, true);
            _timer.Dispose();
        }

    }
}
