using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel;

namespace WinMediaBox.Classes.Tools
{
    public class ResizeTool : Base
    {
        public Grid _mainGrid;
        public ObservableCollection<IResizable> items;

        public double frameWidth
        {
            get
            {
                double res = 0;
                if(_mainGrid.ActualWidth != 0)
                {
                    res = _mainGrid.ActualWidth - _correction;
                }
                return res;
            }
        }
        public double _frameHeight = 0;
        public double frameHeight
        {
            get
            {
                return _frameHeight;
            }
            set
            {
                _frameHeight = value;
                OnPropertyChanged();
            }
        }

        public int cardWidth
        {
            get
            {
                int res = _originalCardWidth;
                double awidth = frameWidth;
                int cnt = Convert.ToInt32(Math.Truncate(awidth / (res + 1)));
                if (cnt >= 1)
                {
                    res = Convert.ToInt32(Math.Round(awidth / cnt, 0) - 1);
                }
                return res - _marginCorrection;
            }
        }

        private int _originalCardWidth;
        private double _correction;
        private int _marginCorrection;

        public ResizeTool(Grid mainGrid, int originalCardWidth, double correction, int marginCorrection = 0)
        {
            _mainGrid = mainGrid;
            _marginCorrection = marginCorrection;
            _correction = correction;
            _originalCardWidth = originalCardWidth;
            items = new ObservableCollection<IResizable>();
        }

        public void OnResize(double actualHeight)
        {
            OnPropertyChanged("frameWidth");
            frameHeight = actualHeight - 20;
            OnPropertyChanged("cardWidth");
            double cw = cardWidth;
            foreach (var i in items)
            {
                i.cardWidth = cw;
            }
        }

    }
}
