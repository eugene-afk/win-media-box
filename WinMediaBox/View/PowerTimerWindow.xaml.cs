using System;
using System.Windows;
using System.Windows.Input;
using WinMediaBox.Classes.Tools;

namespace WinMediaBox.View
{
    public partial class PowerTimerWindow : Window
    {
        public PowerTimerWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TimerInput.Focus();
            TimerInput.SelectionStart = TimerInput.Text.Length;
            TimerInput.SelectionLength = 0;
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Down)
            {
                if (TimerInput.IsFocused)
                {
                    //Sending Tab to left input
                    await SendKeys.Send(AppDomain.CurrentDomain.FriendlyName, 0x09, false, 0);
                }
            }
        }

    }
}
