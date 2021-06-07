using System;
using System.Windows.Controls;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel.MediaActions;
using WinMediaBox.ViewModel.SubMediaActions;

namespace WinMediaBox.ViewModel
{
    public class SubMenuSwitchViewModel : Base
    {
        private SubMenuItems _items;
        public SubMenuItems items 
        { 
            get { return _items; }
            set { _items = value; OnPropertyChanged(); }
        }
        private MediaActionWSubBase _mediaAction;
        private ISubMediaAction _subMediaAction;
        public ResizeTool resizeTool { get; set; }

        public SubMenuSwitchViewModel(IMediaAction mediaAction, Grid mainGrid)
        {
            _mediaAction = (MediaActionWSubBase)mediaAction;
            _subMediaAction = (ISubMediaAction)mediaAction;
            items = _mediaAction.items;
            var corrections = GetCorrections(mediaAction.cardsType);
            resizeTool = new ResizeTool(mainGrid, items.cardWidth, corrections.Item1, corrections.Item2);
            for (int i = 0; i < items.Count; i++)
            {
                resizeTool.items.Add((IResizable)items[i]);
            }
        }

        public void StartWith(ISubMenuItem item)
        {
            _mediaAction.selectedItem = item;
            _subMediaAction.StartWith();
        }

        public void ReLoadData()
        {
            _subMediaAction.Reload();
        }

        private Tuple<int, int> GetCorrections(MediaActionCardsType type)
        {
            var res = new Tuple<int, int>(23, 14);
            switch (type)
            {
                case MediaActionCardsType.Thin:
                    res = new Tuple<int, int>(20, 5);
                    break;
            }

            return res;
        }

    }
}
