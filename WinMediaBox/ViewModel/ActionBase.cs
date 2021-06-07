using WinMediaBox.ViewModel;

namespace WinMediaBox.Classes
{
    public class ActionBase : Base
    {
        public string img { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        private double _cardWidth = 620;
        public double cardWidth
        {
            get { return _cardWidth; }
            set { _cardWidth = value; OnPropertyChanged(); }
        }

    }
}
