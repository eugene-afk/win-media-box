using System.Collections.ObjectModel;
using WinMediaBox.Interfaces;

namespace WinMediaBox.ViewModel.SubMediaActions
{
    public class SubMenuItems : ObservableCollection<ISubMenuItem>
    {
        public int cardWidth;

        public SubMenuItems(ISubMenuBuilder builder)
        {
            builder.Build(this);
            cardWidth = builder.cardWidth;
        }

    }
}
