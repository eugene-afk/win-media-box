using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinMediaBox.Classes;

namespace WinMediaBox.ViewModel
{
    public class VLCPlayerViewModel : IDisposable
    {
        public VLCService _vlcService;
        private M3U8Playlist _playlist;

        public VLCPlayerViewModel(M3U8Playlist playlist)
        {
            _playlist = playlist;
            _vlcService = new VLCService();
            SetupPlayer();
        }

        public void SetupPlayer()
        {
            Media media = new Media(_vlcService.libVLC, new Uri(_playlist[0].url));
            _vlcService.player = new MediaPlayer(media);
            media.Dispose();
        }

        public void Dispose()
        {
            _vlcService.Dispose();
        }
    }
}
