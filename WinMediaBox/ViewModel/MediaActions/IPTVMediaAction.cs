using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WinMediaBox.Classes.MediaActions;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.Types;
using WinMediaBox.View;
using WinMediaBox.ViewModel.MediaActions;

namespace WinMediaBox.Classes
{
    public class IPTVMediaAction : MediaActionBase, IMediaAction, IResizable, IPlayerSelectable
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Standart;
        private Process _proc;
        private HotKey _hotkeyF;
        private bool _isVLC;
        private VLCPlayerWindow _playerWindow;
        private M3U8Playlist _playlist;
        private PlayerSelectionWindow _selectionWindow;

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
                await Task.Run(() => SessionExiting.SetEndCurrentMediaAction(new OnSessionEndidngActions.EndCurrentMediaAction(Stop)));
                if (UCommons.ipTVPlayerPath != "") 
                {
                    _selectionWindow = new PlayerSelectionWindow(this);
                    _selectionWindow.Show();
                    return;
                }
                StartWithVLCPlayer();
                _isVLC = true;
            }
        }

        public async void SetPlayer(PlayerType type)
        {
            _selectionWindow.Close();
            _selectionWindow = null;
            switch (type)
            {
                case PlayerType.Default:
                    await StartWithDefaultPlayer();
                    break;
                case PlayerType.VLC:
                    StartWithVLCPlayer();
                    break;
                default:
                    await StartWithDefaultPlayer();
                    break;
            }
        }

        public async Task StartWithDefaultPlayer()
        {
            ProcessStartInfo  info = new ProcessStartInfo(@"" + UCommons.ipTVPlayerPath + "");
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

            await DefaultPlayersDelayAndSendKeys();
        }

        private async Task DefaultPlayersDelayAndSendKeys()
        {
            //wait until player init
            await Task.Delay(10000);
            //sending F for fullscreen
            await SendKeys.Send(_proc.ProcessName, 0x46);

            //sending 75 as default channel
            await SendKeys.Send(_proc.ProcessName, 0x37);
            await SendKeys.Send(_proc.ProcessName, 0x35);
        }

        private void StopVLC()
        {
            _playerWindow.Close();
            _playerWindow = null;
        }
        
        public void StartWithVLCPlayer()
        {
            _isVLC = true;
            if (_playlist == null)
            {
                _playlist = new M3U8Playlist(UCommons.ipTVPlaylist);
            }
            _playerWindow = new VLCPlayerWindow(_playlist);
            _playerWindow.Show();
        }

        private void StopCustomPlayer()
        {
            if (_hotkeyF == null)
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
            catch (Exception ex)
            {
                Log.Logger.Error("*IPTVMediaAction Stop Process* msg: " + ex);
            }
        }

        public void Stop()
        {
            if (isActive)
            {
                isActive = false;

                SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;

                if(_selectionWindow != null)
                {
                    _selectionWindow.Close();
                    _selectionWindow = null;
                    return;
                }
                if (!_isVLC)
                {
                    StopCustomPlayer();
                    return;
                }
                StopVLC();
                _isVLC = false;
            }
        }

        private async void ToggleFullScreen(HotKey hotKey)
        {
            await SendKeys.Send(_proc.ProcessName, 0x46);
        }

    }
}
