using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WinMediaBox.Classes;
using WinMediaBox.ViewModel;

namespace WinMediaBox.View
{
    public partial class VLCPlayerWindow : Window
    {
        VLCPlayerViewModel _vm;

        public VLCPlayerWindow(M3U8Playlist playlist)
        {
            InitializeComponent();
            _vm = new VLCPlayerViewModel(playlist);
            DataContext = _vm;
            videoView.Loaded += VideoView_Loaded;
        }

        void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            videoView.MediaPlayer = _vm.vlcService.player;
            _vm.vlcService.player.Play();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.InputAction(e.Key);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            videoView.Dispose();
            _vm.Dispose();
        }
    }
}
