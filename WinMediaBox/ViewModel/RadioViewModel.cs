using System;
using System.IO;
using System.Windows.Controls;

namespace WinMediaBox.ViewModel
{
    public class RadioViewModel : Base
    {
        public string bg { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "images/radio-ani.gif");
        private string _currentStationimg;
        public string currentStationimg
        {
            get { return _currentStationimg; }
            set { _currentStationimg = value; OnPropertyChanged(); }
        }

        private MediaElement _player;

        public RadioViewModel(string stationUri, string imgName, MediaElement player)
        {
            currentStationimg = Path.Combine(Directory.GetCurrentDirectory(), imgName);
            _player = player;
            _player.Source = new Uri(stationUri, UriKind.RelativeOrAbsolute);
            _player.Play();
        }

    }
}
