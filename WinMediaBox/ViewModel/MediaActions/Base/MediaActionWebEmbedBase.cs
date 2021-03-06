using System;
using WinMediaBox.Classes.Tools;
using WinMediaBox.View;
using WinMediaBox.ViewModel.SubMediaActions;

namespace WinMediaBox.ViewModel.MediaActions
{
    public class MediaActionWebEmbedBase : MediaActionWSubBase
    {
        public WebViewWindow viewPage;
        public EmbedWebType type;

        public void StartWith()
        {
            if (selectedItem != null)
            {
                switchPage.Close();
                EmbedSubMenuItem w = (EmbedSubMenuItem)selectedItem;
                w.SetHtmlContent(type);
                viewPage = new WebViewWindow(w.html);
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
                App.Current.Dispatcher.Invoke(() => App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized);

                if (selectedItem != null)
                {
                    selectedItem = null;
                    viewPage.Dispatcher.Invoke(() => viewPage.Close());
                }
                switchPage.Dispatcher.Invoke(() => switchPage.Close());
            }
        }

    }
}
