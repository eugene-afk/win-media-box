using System;
using System.Threading.Tasks;
using WinMediaBox.Classes;
using WinMediaBox.Classes.MediaActions;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.View;

namespace WinMediaBox.ViewModel.MediaActions
{
    public class PowerControlAction : MediaActionBase, IMediaAction, IResizable 
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Standart;
        private PowerControlWindow _page;
        private PowerTimerWindow _tPage;

        public PowerControlAction()
        {
            img = "/img/power.png";
            color = "#222";
            title = "Power";
        }

        public async Task Start()
        {
            if (!isActive)
            {
                isActive = true;
                await Task.Run(() =>
                {
                    SessionExiting.SetEndCurrentMediaAction(new OnSessionEndidngActions.EndCurrentMediaAction(Stop));
                });
                _tPage = new PowerTimerWindow();
                _page = new PowerControlWindow(this, _tPage);
                _page.Show();
            }
        }

        public void Stop()
        {
            if (isActive && !SessionExiting.manualExit)
            {
                isActive = false;

                SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;

                _page.Close();
                _tPage.Close();
            }
        }

    }
}
