using LibVLCSharp.Shared;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WinMediaBox.Classes;

namespace WinMediaBox.ViewModel
{
    public class VLCPlayerViewModel : Base, IDisposable
    {
        public VLCService vlcService;
        private M3U8Playlist _playlist;

        private string _channelName { get; set; } = "UNKNOWN";
        public string channelName
        {
            get { return _channelName; }
            set { _channelName = value; OnPropertyChanged(); }
        }

        private Visibility _infoBlockVisible { get; set; } = Visibility.Collapsed;
        public Visibility infoBlockVisible
        {
            get { return _infoBlockVisible; }
            set { _infoBlockVisible = value; OnPropertyChanged(); }
        }

        private int _channelNumber { get; set; } = 0;
        public int channelNumber
        {
            get { return _channelNumber; }
            set { _channelNumber = value; OnPropertyChanged(); }
        }

        private M3U8Item _currentChannel;
        private int _channelsCount;
        public bool isInfoBlockVisible => infoBlockVisible != Visibility.Collapsed;
        private bool disposed = false;
        private bool forcingChannel = false;

        public VLCPlayerViewModel(M3U8Playlist playlist)
        {
            _playlist = playlist;
            vlcService = new VLCService();
            _channelsCount = _playlist.Count();
            SetupPlayer();
        }

        public void SetupPlayer()
        {
            _currentChannel = _playlist[0];
            channelName = _currentChannel.title;
            channelNumber = _currentChannel.number;
            Media media = new Media(vlcService.libVLC, new Uri(_currentChannel.url));
            vlcService.player = new MediaPlayer(media);
            media.Dispose();
        }

        private void Next()
        {
            int num = _currentChannel.number;
            if(num >= _channelsCount)
            {
                num = 0;
            }
            SetNewMedia(num);
        }

        private void Prev()
        {
            int num = _currentChannel.number - 2;
            if (num < 0)
            {
                num = _channelsCount - 1;
            }
            SetNewMedia(num);
        }

        private async Task ShowInfoBlock(bool force = false)
        {
            if (!isInfoBlockVisible)
            {

                infoBlockVisible = Visibility.Visible;
                await Task.Delay(3000);
                infoBlockVisible = Visibility.Collapsed;
                if (force)
                {
                    if (channelNumber >= 0 && channelNumber <= _channelsCount && channelNumber != _currentChannel.number)
                    {
                        SetNewMedia(channelNumber - 1);
                        forcingChannel = false;
                        return;
                    }
                    forcingChannel = false;
                    channelNumber = _currentChannel.number;
                }

            }
        }

        public void InputAction(Key key)
        {
            if (key == Key.Enter)
            {
                Task.Run(() => ShowInfoBlock());
                return;
            }

            if ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9))
            {
                CheckInfoBlockState();
                forcingChannel = true;
                string num = channelNumber != 0 ? channelNumber.ToString() : "";
                string keyVal = key.ToString().Contains("NumPad") ? key.ToString().Replace("NumPad", "") : key.ToString().Replace("D", "");
                var value = $"{num}{keyVal}";

                channelNumber = Convert.ToInt32(value);
                return;
            }

            if (key == Key.PageUp)
            {
                Next();
                return;
            }

            if (key == Key.PageDown)
            {
                Prev();
            }
        }

        public async Task PlayingObserver()
        {
            while (!disposed)
            {
                await Task.Delay(3000);
                if (!disposed && !vlcService.player.IsPlaying)
                {
                    SetNewMedia(channelNumber - 1);
                }
            }
        }

        private void CheckInfoBlockState()
        {
            if (!isInfoBlockVisible && !forcingChannel)
            {
                channelNumber = 0;
                Task.Run(() => ShowInfoBlock(true));
                return;
            }
            
            if (!forcingChannel)
            {
                channelNumber = 0;
                infoBlockVisible = Visibility.Collapsed;
                Task.Run(() => ShowInfoBlock(true));
            }
        }

        private void SetNewMedia(int num)
        {
            _currentChannel = _playlist[num];
            channelName = _currentChannel.title;
            channelNumber = _currentChannel.number;
            vlcService.player.Media = new Media(vlcService.libVLC, new Uri(_currentChannel.url));
            vlcService.player.Play();
            Task.Run(() => ShowInfoBlock());
        }

        public void Dispose()
        {
            disposed = true;
            vlcService.Dispose();
        }
    }
}
