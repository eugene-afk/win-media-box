using Newtonsoft.Json;
using WinMediaBox.Classes;
using WinMediaBox.Interfaces;
using WinMediaBox.Types;

namespace WinMediaBox.ViewModel.SubMediaActions
{
    public class EmbedSubMenuItem : ActionBase, ISubMenuItem, IResizable
    {
        private string _htmlContent;
        [JsonProperty("channelID")]
        public string option1 { get; set; }
        [JsonIgnore]
        public MediaType type { get; set; } = MediaType.Default;
        
        public string html
        {
            get
            {
                return "<html><head><meta content='IE=Edge' http-equiv='X-UA-Compatible'/><meta content='IE=Edge' http-equiv='X-UA-Compatible'/>" + "<style>" +
                        ".videoContainer { " +
                        "position: absolute;" +
                        "overflow: hidden; " +
                        "width: 100 %; " +
                        "height: 100 %; " +
                        "top: 0; " +
                        "left: 0; " +
                        "bottom: 0; " +
                        "right: 0; " +
                        "display: flex; " +
                        "flex - direction: column; " +
                        "justify - content: center; " +
                        "align - items: center; " +
                        "} " +
                        "iframe { " +
                        "width: 100%; " +
                        "height: 100%; " +
                        "} " +
                        "</style>" +
                        "<div class='videoContainer'>" + _htmlContent + "</div>" +
                        "</body></html>";
            }
        }

        public void SetHtmlContent(EmbedWebType type)
        {
            switch (type)
            {
                case EmbedWebType.YouTube:
                    YouTubeSetup();
                    break;
                case EmbedWebType.Twitch:
                    TwitchSetup();
                    break;
            }
        }

        private void YouTubeSetup()
        {

            _htmlContent = "<iframe id='video' " +
                        "src= 'https://www.youtube.com/embed/" + option1 + "?" +
                        "showinfo=0&rel=0&autoplay=1&loop=1&autohide=1&mute=0&playlist=" + option1 + "&playsinline=1' " +
                        "frameborder='0' allowFullScreen=1></iframe>";
        }

        private void TwitchSetup()
        {

            _htmlContent = "<iframe id='video'" +
                    "src = 'https://player.twitch.tv/?channel=" + option1 + "&parent=localhost&muted=false'" +
                    "allowfullscreen = 'true'>" +
                    "</iframe>";
        }

    }
}
