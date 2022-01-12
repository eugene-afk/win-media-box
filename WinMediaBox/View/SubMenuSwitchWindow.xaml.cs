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
using WinMediaBox.Classes.MediaActions;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel;
using WinMediaBox.ViewModel.MediaActions;
using WinMediaBox.ViewModel.SubMediaActions;

namespace WinMediaBox.View
{
    public partial class SubMenuSwitchWindow : Window
    {
        private SubMenuSwitchViewModel _vm;
        private readonly IMediaAction _ma;
        public SubMenuSwitchWindow(IMediaAction mediaAction)
        {
            InitializeComponent();
            _ma = mediaAction;
            _vm = new SubMenuSwitchViewModel(mediaAction, MainGrid);
            DataContext = _vm;
            SetCardsDataTemplate(mediaAction.cardsType);
        }

        public void UpdateData(SubMenuItems items)
        {
            _vm.items = items;
        }

        private void ListItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    ISubMenuItem item = ((ListViewItem)e.OriginalSource).Content as ISubMenuItem;
                    if(item.type == Types.MediaType.Series)
                    {
                        var ma = (LocalDiskMediaAction)_ma;
                        ma.LoadItemsFromSeriesDirectory(item.option1, item.img);
                        SetFocus();
                        return;
                    }
                    if (item.option1 == "reload")
                    {
                        _vm.ReLoadData();
                        SetFocus();
                        return;
                    }
                    _vm.StartWith(item);
                }
                catch { }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.resizeTool.OnResize(MainGrid.ActualHeight);
            SetFocus();
        }

        private void SetFocus()
        {
            MediaListView.Focus();
            ListViewItem listItem = MediaListView.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
            listItem.Focus();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _vm.resizeTool.OnResize(MainGrid.ActualHeight);
        }

        private void SetCardsDataTemplate(MediaActionCardsType type)
        {
            switch (type)
            {
                case MediaActionCardsType.Thin:
                    DataTemplate dataTemplate = FindResource("ListItemThinTemplate") as DataTemplate;
                    MediaListView.ItemTemplate = dataTemplate;
                    break;
            }
        }

    }
}
