using Microsoft.Win32;
using System;
using System.Windows;
using WinMediaBox.Classes;
using WinMediaBox.Classes.Tools;

namespace WinMediaBox
{
    public partial class App : Application
    {
        public App()
        {
            UCommons.Init();
            SystemEvents.PowerModeChanged += OnPowerChange;
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            SessionExiting.EndAll();

            //e.Cancel = true;
            //Process.Start("shutdown", "hybrid");
        }

        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                    App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
                    break;
                case PowerModes.Suspend:
                    SessionExiting.EndAll();
                    break;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SystemEvents.PowerModeChanged -= OnPowerChange;
            SessionExiting.EndAll();
        }
    }
}
