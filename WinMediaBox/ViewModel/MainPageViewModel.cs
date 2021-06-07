using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using WinMediaBox.Classes;
using WinMediaBox.Classes.MediaActions;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel.MediaActions;

namespace WinMediaBox.ViewModel
{
    public class MainPageViewModel : Base
    {
        #region Properties
        public ObservableCollection<IMediaAction> mediaActions { get; set; }
        public string bg { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "images/bg.jpg");
        private const int cardWidth = 620;
        public ResizeTool resizeTool { get; set; }
        public DefaultHotKeys defaultHotKeys;

        #endregion

        public MainPageViewModel(Grid mainGrid)
        {
            resizeTool = new ResizeTool(mainGrid, cardWidth, 22, 15);
            Init();
        }

        #region Methods
        private void Init()
        {
            mediaActions = new ObservableCollection<IMediaAction>();
            mediaActions.Add(new IPTVMediaAction());
            mediaActions.Add(new YouTubeMediaAction());
            mediaActions.Add(new TwitchMediaAction());
            mediaActions.Add(new LocalDiskMediaAction());
            mediaActions.Add(new RadioMediaAction());
            mediaActions.Add(new PowerControlAction());
            foreach(var i in mediaActions)
            {
                resizeTool.items.Add((IResizable)i);
            }
            //_hotKey = new HotKey(Key.F9, KeyModifier.Shift | KeyModifier.Win, OnHotKeyHandler);
            defaultHotKeys = new DefaultHotKeys(mediaActions);
        }

        #endregion
    }
}
