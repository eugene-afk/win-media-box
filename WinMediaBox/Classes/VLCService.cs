using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using WinMediaBox.Classes.Tools;

namespace WinMediaBox.Classes
{
    public class VLCService: IDisposable
    {
        public LibVLC libVLC;
        public MediaPlayer player;
        private List<HotKey> _hotKeys;
        private int _offset = 60000;

        public VLCService()
        {
            Core.Initialize();
            libVLC = new LibVLC();
        }

        public void PlaySingleVideo(string path)
        {
            Media media = new Media(libVLC, new Uri(path));
            player = new MediaPlayer(media);
            player.SetVideoTitleDisplay(Position.TopLeft, 1000);
            media.Dispose();
            player.Fullscreen = true;
            player.Play();
            AttachHotKeys();
        }

        public void PlayMultipleVideos(string path, Action action)
        {
            PlaySingleVideo(path);
            player.EndReached += (s, e) => action();
        }

        public void ChangeVideo(string path)
        {
            var media = new Media(libVLC, new Uri(path));
            player.Media = media;
            media.Dispose();
            player.Play();
        }

        private void AttachHotKeys()
        {
            ClearHotkeys();
            _hotKeys = new List<HotKey>();
            HotKey _hotKeyRewind = new HotKey(Key.Left, KeyModifier.None, Rewind);
            HotKey _hotKeyForward = new HotKey(Key.Right, KeyModifier.None, Forward);
            HotKey _hotKeyPauseKeyboard = new HotKey(Key.Space, KeyModifier.None, PausePlayer);
            HotKey _hotKeyPauseButton = new HotKey(Key.MediaPlayPause, KeyModifier.None, PausePlayer);
            _hotKeys.Add(_hotKeyPauseKeyboard);
            _hotKeys.Add(_hotKeyPauseButton);
            _hotKeys.Add(_hotKeyRewind);
            _hotKeys.Add(_hotKeyForward);
        }

        private void PausePlayer(HotKey hotKey)
        {
            if (player == null)
            {
                //TODO: add sendkeys for space and mediaplaypause buttons
                ClearHotkeys();
                return;
            }

            if (player.CanPause)
            {
                player.Pause();
                return;
            }
            player.Play();

        }

        private void Rewind(HotKey hotkey)
        {
            if (player == null)
            {
                ClearHotkeys();
                SendKeys.SendWithoutProc(WinKeysCodes.leftArrowWinKeyCode);
                return;
            }

            player.Time -= _offset;
        }

        private void Forward(HotKey hotkey)
        {
            if (player == null)
            {
                ClearHotkeys();
                SendKeys.SendWithoutProc(WinKeysCodes.rightArrowWinKeyCode);
                return;
            }

            player.Time += _offset;
        }

        private void ClearHotkeys()
        {
            if (_hotKeys != null)
            {
                if(_hotKeys.Count > 0)
                {
                    foreach (var i in _hotKeys)
                    {
                        i.Dispose();
                    }
                    _hotKeys.Clear();
                }
            }
        }

        public void Dispose()
        {
            if (!SessionExiting.isExiting)
                ClearHotkeys();

            libVLC.Dispose();
            player.Dispose();
            player = null;
        }
    }
}
