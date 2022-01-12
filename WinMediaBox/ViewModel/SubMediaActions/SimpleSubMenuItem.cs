using Newtonsoft.Json;
using WinMediaBox.Classes;
using WinMediaBox.Interfaces;
using WinMediaBox.Types;

namespace WinMediaBox.ViewModel.SubMediaActions
{
    public class SimpleSubMenuItem : ActionBase, ISubMenuItem, IResizable
    {
        [JsonProperty("uri")]
        public string option1 { get; set; }
        [JsonIgnore]
        public MediaType type { get; set; } = MediaType.Default;
        [JsonIgnore]
        public string typeImage 
        { 
            get 
            {
                if (option1 == "reload" || option1 == "return")
                    return "";
                switch (type)
                {
                    case MediaType.Series:
                        return "/img/folder.png";
                    case 0:
                        return "/img/file.png";
                }
                return "/img/file.png";
            }
        } 
    }
}
