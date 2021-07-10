using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinMediaBox.Classes.Tools;

namespace WinMediaBox.Classes
{
    public class VLCService: IDisposable
    {
        private LibVLC _libVLC;
        private MediaPlayer _player;
        private List<HotKey> _hotKeys;
        private int _offset = 60000;

        public VLCService()
        {
            Core.Initialize();
            _libVLC = new LibVLC();
        }

        public void PlaySingleVideo(string path)
        {
            Media media = new Media(_libVLC, new Uri(path));
            _player = new MediaPlayer(media);
            media.Dispose();
            _player.Fullscreen = true;
            _player.Play();
            AttachHotKeys();
        }

        private void AttachHotKeys()
        {
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
            if (_player.CanPause)
            {
                _player.Pause();
                return;
            }
            _player.Play();

        }

        private void Rewind(HotKey hotkey)
        {
            _player.Time -= _offset;
        }

        private void Forward(HotKey hotkey)
        {
            _player.Time += _offset;
        }

        public void Dispose()
        {
            if (_hotKeys != null)
            {
                foreach (var i in _hotKeys)
                {
                    i.Dispose();
                }
                _hotKeys.Clear();
            }
            _libVLC.Dispose();
            _player.Dispose();
        }
    }
}
