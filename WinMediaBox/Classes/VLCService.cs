using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMediaBox.Classes
{
    public class VLCService: IDisposable
    {
        private LibVLC _libVLC;
        private MediaPlayer _player;

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
        }

        public void Dispose()
        {
            _libVLC.Dispose();
            _player.Dispose();
        }
    }
}
