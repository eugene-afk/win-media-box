﻿using Serilog;
using System;
using System.Diagnostics;
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
            items = new SubMenuItems(new LocalBuilder());
            switchPage.UpdateData(items);
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
                _vlcService.PlaySingleVideo(@"" + _simpleSelectedItem.option1 + "");
                _isVLC = true;
                return;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("*_vlcService.PlaySingleVideo* msg: " + ex);
            }
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

                    if (_selectionWindow != null)
                    {
                        _selectionWindow.Close();
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
                switchPage.Close();
                switchPage = null;
            }
        }

    }
}
