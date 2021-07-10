using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            videoView.Loaded += VideoView_Loaded;
        }

        void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            videoView.MediaPlayer = _vm._vlcService.player;
            _vm._vlcService.player.Play();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            videoView.Dispose();
            _vm.Dispose();
        }
    }
}
