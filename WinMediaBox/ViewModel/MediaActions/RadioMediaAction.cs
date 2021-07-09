using System;
using System.Threading.Tasks;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.View;
using WinMediaBox.ViewModel.MediaActions;
using WinMediaBox.ViewModel.SubMediaActions;
using WinMediaBox.ViewModel.SubMediaActions.Builders;

namespace WinMediaBox.Classes
{
    public class RadioMediaAction : MediaActionWSubBase, IMediaAction, IResizable, ISubMediaAction
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Standart;
        private RadioViewWindow viewPage;

        public RadioMediaAction()
        {
            img = "/img/radio.png";
            color = "#ff8a50";
            title = "Radio";
        }

        public void Reload()
        {
            items = new SubMenuItems(new RadioBuilder());
            switchPage.UpdateData(items);
        }

        public async Task Start()
        {
            if (!isActive)
            {
                items = new SubMenuItems(new RadioBuilder());
                isActive = true;
                await Task.Run(() =>
                {
                    SessionExiting.SetEndCurrentMediaAction(new OnSessionEndidngActions.EndCurrentMediaAction(Stop));
                });
                switchPage = new SubMenuSwitchWindow(this);
                switchPage.Show();
            }
        }

        public void StartWith()
        {
            if (selectedItem != null)
            {
                switchPage.Close();
                SimpleSubMenuItem s = (SimpleSubMenuItem)selectedItem;
                viewPage = new RadioViewWindow(s.option1, s.img);
                viewPage.Show();
            }
        }

        public void Stop()
        {
            if (isActive)
            {
                items = null;
                isActive = false;

                SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
                
                if (selectedItem != null)
                {
                    selectedItem = null;
                    viewPage.Close();
                }

                switchPage.Close();
            }
        }

    }
}
