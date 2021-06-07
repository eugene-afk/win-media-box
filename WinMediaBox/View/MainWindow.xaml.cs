using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinMediaBox.Classes.Tools;
using WinMediaBox.Interfaces;
using WinMediaBox.ViewModel;

namespace WinMediaBox.View
{
    public partial class MainWindow : Window
    {
        MainPageViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainPageViewModel(MainGrid);
            DataContext = _vm;
        }

        private async void ListItem_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                IMediaAction ma  = ((ListViewItem)e.OriginalSource).Content as IMediaAction;
                await ma.Start();
                _vm.defaultHotKeys.InitBackHotKey();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _vm.resizeTool.OnResize(MainGrid.ActualHeight);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.resizeTool.OnResize(MainGrid.ActualHeight);

            //On app start focus not shown visual effect
            //MediaListView.Focus();
            //ListViewItem item = MediaListView.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
            //item.Focus();

            //Sending Tab to correct focus
            await SendKeys.Send(AppDomain.CurrentDomain.FriendlyName, 0x09, false, 0);
        }

    }
}
