using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.Types;
using WinMediaBox.View;
using WinMediaBox.ViewModel.MediaActions;
using WinMediaBox.ViewModel.SubMediaActions;
using WinMediaBox.ViewModel.SubMediaActions.Builders;

namespace WinMediaBox.Classes.MediaActions
{
    public class LocalDiskMediaAction : MediaActionWSubBase, IMediaAction, IResizable, ISubMediaAction, IPlayerSelectable
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Thin;
        private Process _proc;
        private VLCService _vlcService;
        private bool _isVLC = false;
        private bool _isSeries = false;
        private int _seriesIndex = 0;
        private PlayerSelectionWindow _selectionWindow;
        private SimpleSubMenuItem _simpleSelectedItem;

        public LocalDiskMediaAction()
        {
            
            img = "/img/localstorage.png";
            color = "#303f9f";
            title = "Local Storage";
        }

        public void Reload()
        {
            _isSeries = false;
            items = new SubMenuItems(new LocalBuilder());
            switchPage.UpdateData(items);
        }

        public void LoadItemsFromSeriesDirectory(string path, string posterPath)
        {
            _isSeries = true;   
            var files = Directory.GetFiles(path);
            items.Clear();
            AddReturnDataItem(310);
            foreach (var i in files)
            {
                SimpleSubMenuItem item = new();
                item.option1 = i;
                item.title = Path.GetFileNameWithoutExtension(i);
                item.img = posterPath;
                if (!UCommons.isValidImage(posterPath))
                {
                    item.img = Path.Combine(Directory.GetCurrentDirectory(), "images/default.png");
                }
                if (string.IsNullOrEmpty(item.color) || !item.color.Contains("#"))
                {
                    item.color = "#222";
                }
                item.cardWidth = 310;
                items.Add(item);
            }
        }

        public async Task Start()
        {
            if (!isActive)
            {
                items = new SubMenuItems(new LocalBuilder());
                isActive = true;
                await Task.Run(() =>
                {
                    SessionExiting.SetEndCurrentMediaAction(new OnSessionEndidngActions.EndCurrentMediaAction(Stop));
                });
                switchPage = new SubMenuSwitchWindow(this);
                switchPage.Show();
            }
        }

        public void SetPlayer(PlayerType type)
        {
            _selectionWindow.Close();
            _selectionWindow = null;
            switch (type)
            {
                case PlayerType.Default:
                    StartWithDefaultPlayer();
                    break;
                case PlayerType.VLC:
                    StartWithVLCPlayer();
                    break;
                default:
                    StartWithDefaultPlayer();
                    break;
            }
        }

        public void StartWithVLCPlayer()
        {
            try
            {
                //if vlc doesn't work - going next try with default user player and default windows player
                _vlcService = new VLCService();
                _isVLC = true;
                if (_isSeries)
                {
                    _vlcService.PlayMultipleVideos(@"" + _simpleSelectedItem.option1 + "", PlayNextVideo);
                    _seriesIndex = items.IndexOf(_simpleSelectedItem);
                    return;
                }
                _vlcService.PlaySingleVideo(@"" + _simpleSelectedItem.option1 + "");
                return;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("*_vlcService.PlaySingleVideo* msg: " + ex);
            }
        }

        private void PlayNextVideo()
        {
            try
            {
                _simpleSelectedItem = (SimpleSubMenuItem)items[_seriesIndex + 1];
            }
            catch 
            {
                _ = SendKeys.Send(AppDomain.CurrentDomain.FriendlyName, WinKeysCodes.escWinKeyCode, false, 0);
                return;
            }
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                _vlcService.ChangeVideo(@"" + _simpleSelectedItem.option1 + "");
            });
        }

        public async void StartWithDefaultPlayer()
        {
            //In this case video files opens in default win10 Movies&TV App 
            //with always fullscreen settings (https://winaero.com/make-movies-tv-always-play-fullscreen-windows-10/)
            await Task.Run(async () =>
            {
                _proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = @"" + _simpleSelectedItem.option1 + "",
                        CreateNoWindow = false,
                        UseShellExecute = true
                    }
                };

                try
                {
                    _proc.Start();
                }
                catch (Exception ex)
                {
                    Log.Logger.Error("*LocalDiskMediaAction Start Process* msg: " + ex);
                    return;
                }

                //if used not defaul windows player
                try
                {
                    SendKeys.SetForegroundWindow(_proc.ProcessName);
                }
                catch (Exception ex)
                {
                    //try with default windows player
                    try
                    {
                        SendKeys.SetForegroundWindow("Video.UI");
                        await SendKeys.SendMouseLeft();
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error("*StartWithLocalItem SetForegroundWindow Video.UI* msg: " + e);
                    }
                    Log.Logger.Error("*StartWithLocalItem SetForegroundWindow Not Default* msg: " + ex);
                }
            });
        }

        public void StartWith()
        {
            if (selectedItem != null)
            {
                switchPage.Close();
                _simpleSelectedItem = (SimpleSubMenuItem)selectedItem;
                _selectionWindow = new PlayerSelectionWindow(this);
                _selectionWindow.Show();
            }
        }

        private void AddReturnDataItem(int cardWidth)
        {
            items.Add(new SimpleSubMenuItem()
            {
                option1 = "reload",
                title = "Return",
                img = "/img/return.png",
                color = "#222",
                cardWidth = cardWidth
            });
        }

        public void Stop()
        {
            if (isActive)
            {
                _isSeries = false;
                items = null;
                isActive = false;

                SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                App.Current.Dispatcher.Invoke(() => App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized);

                if (selectedItem != null)
                {
                    selectedItem = null;

                    if (_selectionWindow != null)
                    {
                        _selectionWindow.Dispatcher.Invoke(() => _selectionWindow.Close());
                        _selectionWindow = null;
                        return;
                    }

                    if (_isVLC)
                    {
                        _vlcService.Dispose();
                        _isVLC = false;
                        return;
                    }

                    //try to kill process if not default windows player
                    try
                    {
                        _proc.Kill();
                        _proc.WaitForExit();
                        _proc.Dispose();
                    }
                    catch(Exception ex)
                    {
                        //It's solution to kill default win10 Movies&TV App cause started earlier process not associated with any app
                        var procs = Process.GetProcessesByName("Video.UI");
                        try
                        {
                            procs[0].Kill();
                            procs[0].WaitForExit();
                            procs[0].Dispose();
                        }
                        catch (Exception e)
                        {
                            Log.Logger.Error("*LocalDiskMediaAction Stop Process Video.UI* msg: " + e);
                        }
                        Log.Logger.Error("*LocalDiskMediaAction Stop Process Not Default* msg: " + ex);
                    }
                    return;
                }
                switchPage.Dispatcher.Invoke(() => switchPage.Close());
                switchPage = null;
            }
        }

    }
}
