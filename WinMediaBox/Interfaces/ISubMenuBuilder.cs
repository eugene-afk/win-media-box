using System.Collections.ObjectModel;

namespace WinMediaBox.Interfaces
{
    public interface ISubMenuBuilder
    {
        public void Build(ObservableCollection<ISubMenuItem> items);
        public int cardWidth { get; set; }
    }
}
