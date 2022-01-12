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
    }
}
