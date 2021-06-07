using System.Windows;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel;

namespace WinMediaBox.View
{
    public partial class PowerControlWindow : Window
    {
        private IMediaAction _mediaAction;
        public PowerControlWindow(IMediaAction mediaAction, PowerTimerWindow tPage)
        {
            _mediaAction = mediaAction;
            InitializeComponent();
            DataContext = new PowerControlViewModel(this, tPage);
            SleepBtn.Focus();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _mediaAction.Stop();
        }
    }
}
