using Serilog;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WinMediaBox.Classes;
using WinMediaBox.Interfaces;

namespace WinMediaBox.ViewModel.SubMediaActions.Builders
{
    public class LocalBuilder : BuilderBase, ISubMenuBuilder
    {
        readonly string pathBase = @"" + UCommons.moviesBasePath + "";
        readonly string postersPath = @"" + UCommons.postersBasePath + "";
        public int cardWidth { get; set; } = 310;

        public void Build(ObservableCollection<ISubMenuItem> items)
        {
            try
            {
                AddReloadDataItem(items, cardWidth);

                var files = Directory.GetFiles(pathBase);
                var directories = Directory.GetDirectories(pathBase);
                var posters = Directory.GetFiles(postersPath);

                foreach(var i in directories)
                {
                    var dirName = Path.GetFileName(i);
                    if (!dirName.Contains("series_"))
                        continue;
                    var FormattedDirName = dirName.Replace("series_", "");
                    SimpleSubMenuItem item = new();
                    item.option1 = i;
                    item.title = FormattedDirName;
                    string poster = posters.Where(x => Path.GetFileNameWithoutExtension(x) == FormattedDirName).FirstOrDefault();
                    item.img = poster;
                    if (!UCommons.isValidImage(poster))
                    {
                        item.img = Path.Combine(Directory.GetCurrentDirectory(), "images/default.png");
                    }
                    if (string.IsNullOrEmpty(item.color) || !item.color.Contains("#"))
                    {
                        item.color = "#222";
                    }
                    item.cardWidth = cardWidth;
                    items.Add(item);
                }

                foreach (var i in files)
                {
                    SimpleSubMenuItem item = new();
                    item.option1 = i;
                    item.title = Path.GetFileNameWithoutExtension(i);
                    string poster = posters.Where(x => Path.GetFileNameWithoutExtension(x) == Path.GetFileNameWithoutExtension(i)).FirstOrDefault();
                    item.img = poster;
                    if (!UCommons.isValidImage(poster))
                    {
                        item.img = Path.Combine(Directory.GetCurrentDirectory(), "images/default.png");
                    }
                    if (string.IsNullOrEmpty(item.color) || !item.color.Contains("#"))
                    {
                        item.color = "#222";
                    }
                    item.cardWidth = cardWidth;
                    items.Add(item);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Error("*LocalBuilder* msg: " + ex);
            }
        }

    }
}
