using System;
using System.Windows.Input;
using WinMediaBox.Classes;
using WinMediaBox.Classes.Tools;
using WinMediaBox.View;

namespace WinMediaBox.ViewModel
{
    public class PowerControlViewModel : Base
    {
        #region Properties
        private PowerTimerWindow _timerWindow;
        private PowerControlWindow _powerWindow;
        private int _timerMinutes = 15;
        public int timerMinutes
        {
            get { return _timerMinutes; }
            set { _timerMinutes = value; OnPropertyChanged(); }
        }

        public ICommand SuspendCommand { protected set; get; }
        public ICommand ShutdownCommand { protected set; get; }
        public ICommand OpenTimerWindowCommand { protected set; get; }
        public ICommand SetTimerShutDownCommand { protected set; get; }
        public ICommand SetTimerSuspendCommand { protected set; get; }
        public ICommand ExitAppCommand { protected set; get; }

        #endregion

        public PowerControlViewModel(PowerControlWindow pcw, PowerTimerWindow tPage)
        {
            _powerWindow = pcw;
            _timerWindow = tPage;
            SuspendCommand = new RelayCommand((a) => Suspend());
            ShutdownCommand = new RelayCommand((a) => ShutDown());
            OpenTimerWindowCommand = new RelayCommand((a) => OpenTimerWindow());
            SetTimerShutDownCommand = new RelayCommand((a) => { SetTimerShutDown(); });
            SetTimerSuspendCommand = new RelayCommand((a) => { SetTimerSuspend(); });
            ExitAppCommand = new RelayCommand((a) => { ExitApp(); });
        }

        #region Commands
        private void Suspend()
        {
            _powerWindow.Close();
            SendKeys.SetSuspendState(false, true, true);
        }

        private void ShutDown()
        {
            _powerWindow.Close();
            SendKeys.ShutDown();
        }

        private void SetTimerShutDown()
        {
            TimeSpan ts = TimeSpan.FromMinutes(timerMinutes);
            int secondsValue = (int)ts.TotalSeconds;
            SendKeys.ShutDown(secondsValue);
            _timerWindow.Close();
            _powerWindow.Close();
        }

        private void SetTimerSuspend()
        {
            TimeSpan ts = TimeSpan.FromMinutes(timerMinutes);
            double msValue = ts.TotalMilliseconds;
            SendKeys.SuspendWithTimer(msValue);
            _timerWindow.Close();
            _powerWindow.Close();
        }

        private void OpenTimerWindow()
        {
            //_timerWindow = new PowerTimerWindow();
            _timerWindow.DataContext = this;
            _timerWindow.ShowDialog();
        }

        private void ExitApp()
        {
            SessionExiting.manualExit = true;
            App.Current.Shutdown();
        }
        #endregion

    }
}
