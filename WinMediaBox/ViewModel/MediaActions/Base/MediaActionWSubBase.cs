using WinMediaBox.Classes.MediaActions;
using WinMediaBox.Interfaces;
using WinMediaBox.View;
using WinMediaBox.ViewModel.SubMediaActions;

namespace WinMediaBox.ViewModel.MediaActions
{
    public class MediaActionWSubBase : MediaActionBase
    {
        public SubMenuItems items;
        public ISubMenuItem selectedItem;
        public SubMenuSwitchWindow switchPage;
    }
}
