using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinMediaBox.Classes;
using WinMediaBox.Interfaces;
using WinMediaBox.Types;
using WinMediaBox.View;

namespace WinMediaBox.ViewModel
{
    public class PlayerSelectionViewModel
    {
        public ICommand DefaultCommand { protected set; get; }
        public ICommand VLCCommand { protected set; get; }
        private IPlayerSelectable _playerSelectable;
        private PlayerSelectionWindow _page;


        public PlayerSelectionViewModel(IPlayerSelectable playerSelectable, PlayerSelectionWindow page)
        {
            _page = page;
            _playerSelectable = playerSelectable;
            DefaultCommand = new RelayCommand((a) => SetDefault());
            VLCCommand = new RelayCommand((a) => SetVLC());
        }

        private void SetDefault()
        {
            _playerSelectable.SetPlayer(PlayerType.Default);
            _page.Close();
        }

        private void SetVLC()
        {
            _playerSelectable.SetPlayer(PlayerType.VLC);
            _page.Close();
        }

    }
}
