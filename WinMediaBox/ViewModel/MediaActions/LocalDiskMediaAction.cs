using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.View;
using WinMediaBox.ViewModel.MediaActions;
using WinMediaBox.ViewModel.SubMediaActions;
using WinMediaBox.ViewModel.SubMediaActions.Builders;

namespace WinMediaBox.Classes.MediaActions
{
    public class LocalDiskMediaAction : MediaActionWSubBase, IMediaAction, IResizable, ISubMediaAction
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Thin;
        private Process _proc;
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

        public async void StartWith()
        {
            //In this case video files opens in default win10 Movies&TV App 
            //with always fullscreen settings (https://winaero.com/make-movies-tv-always-play-fullscreen-windows-10/)
            if (selectedItem != null)
            {
                switchPage.Close();

                await Task.Run(async () =>
                {
                    SimpleSubMenuItem s = (SimpleSubMenuItem)selectedItem;
                    _proc = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = @"" + s.option1 + "",
                            CreateNoWindow = false,
                            UseShellExecute = true
                        }
                    };
                    try
                    {
                        _proc.Start();
                    }
                    catch(Exception ex)
                    {
                        Log.Logger.Error("*LocalDiskMediaAction Start Process* msg: " + ex);
                        return;
                    }
                    //if used not defaul windows player
                    try
                    {
                        SendKeys.SetForegroundWindow(_proc.ProcessName);
                    }
                    catch(Exception ex)
                    {
                        //try with default windows player
                        try
                        {
                            SendKeys.SetForegroundWindow("Video.UI");
                            await SendKeys.SendMouseLeft();
                        }
                        catch(Exception e)
                        {
                            Log.Logger.Error("*StartWithLocalItem SetForegroundWindow Video.UI* msg: " + e);
                        }
                        Log.Logger.Error("*StartWithLocalItem SetForegroundWindow Not Default* msg: " + ex);
                    }
                });
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
            }
        }

    }
}
