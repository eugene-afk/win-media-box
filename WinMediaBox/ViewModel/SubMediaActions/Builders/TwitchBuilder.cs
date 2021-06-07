using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WinMediaBox.Interfaces;

namespace WinMediaBox.ViewModel.SubMediaActions.Builders
{
    public class TwitchBuilder : BuilderBase, ISubMenuBuilder
    {
        readonly string path = "data/TwitchChannels.json";
        public int cardWidth { get; set; } = 620;

        public void Build(ObservableCollection<ISubMenuItem> items)
        {
            try
            {
                AddReloadDataItem(items, cardWidth);
                List<EmbedSubMenuItem> list = JsonConvert.DeserializeObject<List<EmbedSubMenuItem>>(File.ReadAllText(path));
                if(list != null)
                {
                    foreach (var i in list)
                    {
                        AddSubItemJson(i, items);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("*TwitchBuilder* msg: " + ex);
            }
        }

    }
}
