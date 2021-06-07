using System.Collections.ObjectModel;
using System.IO;
using WinMediaBox.Classes;
using WinMediaBox.Interfaces;

namespace WinMediaBox.ViewModel.SubMediaActions.Builders
{
    public class BuilderBase
    {
        public void AddSubItemJson(ISubMenuItem item, ObservableCollection<ISubMenuItem> items)
        {
            if (string.IsNullOrEmpty(item.option1))
            {
                return;
            }
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), item.img);
            if (!UCommons.isValidImage(item.img))
            {
                imgPath = Path.Combine(Directory.GetCurrentDirectory(), "images/default.png");
            }
            item.img = imgPath;
            if (string.IsNullOrEmpty(item.color) || !item.color.Contains("#"))
            {
                item.color = "#222";
            }
            items.Add(item);
        }

        public void AddReloadDataItem(ObservableCollection<ISubMenuItem> items, int cardWidth)
        {
            items.Add(new SimpleSubMenuItem()
            {
                option1 = "reload",
                title = "Update",
                img = Path.Combine(Directory.GetCurrentDirectory(), "images/refresh.png"),
                color = "#222",
                cardWidth = cardWidth
            });
        }

    }
}
