using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;

namespace WinMediaBox.Classes
{
    public class DefaultHotKeys
    {
        private HotKey _back;
        private HotKey _backBrowser;
        private HotKey _killall;
        private ObservableCollection<IMediaAction> _mediaActions;

        public DefaultHotKeys(ObservableCollection<IMediaAction> mediaActions)
        {
            _mediaActions = mediaActions;
            //uncomment this string if you need close all proccesses except this app button
            //_killall = new HotKey(Key.Delete, KeyModifier.None, OnCancellAllHandler);
        }

        public void InitBackHotKey()
        {
            if (_back == null)
            {
                //if you use air mouse or other remote controller change Key here to comfort Key for you,
                //I prefer BrowserBack, it's often "Back" button on remote controller
                _back = new HotKey(Key.Escape, KeyModifier.None, OnBackHotKeyHandler);

            }
            if(_backBrowser == null)
            {
                _backBrowser = new HotKey(Key.BrowserBack, KeyModifier.None, OnBackHotKeyHandler);
            }
            SessionExiting.hotKeys.Add(this._back);
            SessionExiting.hotKeys.Add(this._backBrowser);
        }

        private void OnBackHotKeyHandler(HotKey hotKey)
        {
            SessionExiting.hotKeys.Remove(this._back);
            //_hotKey.Unregister();
            _back.Dispose();
            _back = null;
            _backBrowser.Dispose();
            _backBrowser = null;
            try
            {
                var ma = _mediaActions.Where(i => i.isActive == true).First();
                ma.Stop();

            }
            catch (Exception ex)
            {
                Log.Logger.Error("*OnBackHotKeyHandler* msg " + ex);
            }
        }

        private void OnCancellAllHandler(HotKey hotkey)
        {
            try
            {
                var procs = Process.GetProcesses().Where(p => p.MainWindowHandle != (IntPtr)0).ToArray();
                for (int i = 0; i < procs.Length; i++)
                {
                    if (procs[i].ProcessName != AppDomain.CurrentDomain.FriendlyName)
                    {
                        procs[i].Kill();
                        procs[i].WaitForExit();
                        procs[i].Dispose();
                    }
                }
                SessionExiting.EndAll();
                SendKeys.SetForegroundWindow(AppDomain.CurrentDomain.FriendlyName);
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("*OnCancellAllHandler* msg " + ex);
            }
        }

    }
}
