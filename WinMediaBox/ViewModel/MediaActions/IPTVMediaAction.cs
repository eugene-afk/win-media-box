using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WinMediaBox.Classes.MediaActions;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel.MediaActions;

namespace WinMediaBox.Classes
{
    public class IPTVMediaAction : MediaActionBase, IMediaAction, IResizable
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Standart;
        private Process _proc;
        private HotKey _hotkeyF;
        public IPTVMediaAction()
        {
            img = "/img/iptv.png";
            title = "TV";
            color = "#5697C3";
        }

        public async Task Start()
        {
            if (!isActive)
            {
                isActive = true;
                ProcessStartInfo info = new ProcessStartInfo(@"" + UCommons.ipTVPlayerPath + "");
                info.CreateNoWindow = false;
                info.UseShellExecute = true;
                info.WindowStyle = ProcessWindowStyle.Maximized;
                try
                {
                    _proc = Process.Start(info);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("*IPTVMediaAction Start Process* msg: " + ex);
                    return;
                }
                if (_proc == null || String.IsNullOrEmpty(_proc.ProcessName))
                {
                    Stop();
                    return;
                }
                _hotkeyF = new HotKey(Key.Up, KeyModifier.None, ToggleFullScreen);
                SessionExiting.SetEndCurrentMediaAction(new OnSessionEndidngActions.EndCurrentMediaAction(Stop));

                //wait until player init
                await Task.Delay(10000);
                //sending F for fullscreen
                await SendKeys.Send(_proc.ProcessName, 0x46);

                //sending 75 as default channel
                await SendKeys.Send(_proc.ProcessName, 0x37);
                await SendKeys.Send(_proc.ProcessName, 0x35);
            }
        }

        public void Stop()
        {
            if (isActive)
            {
                isActive = false;

                SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;

                if(_hotkeyF == null)
                {
                    return;
                }
                _hotkeyF.Dispose();
                _hotkeyF = null;

                try
                {
                    _proc.Kill();
                    _proc.WaitForExit();
                    _proc.Dispose();
                }
                catch(Exception ex)
                {
                    Log.Logger.Error("*IPTVMediaAction Stop Process* msg: " + ex);
                }
            }
        }

        private async void ToggleFullScreen(HotKey hotKey)
        {
            await SendKeys.Send(_proc.ProcessName, 0x46);
        }

    }
}
