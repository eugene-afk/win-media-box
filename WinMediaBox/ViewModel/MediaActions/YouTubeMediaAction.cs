using System.Threading.Tasks;
using WinMediaBox.Interfaces;
using WinMediaBox.View;
using WinMediaBox.ViewModel.MediaActions;
using WinMediaBox.ViewModel.SubMediaActions;
using WinMediaBox.ViewModel.SubMediaActions.Builders;

namespace WinMediaBox.Classes
{
    public class YouTubeMediaAction : MediaActionWebEmbedBase, IMediaAction, IResizable, ISubMediaAction
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Standart;
        public YouTubeMediaAction()
        {
            type = EmbedWebType.YouTube;
            img = "/img/yt.png";
            color = "#C41C20";
            title = "YouTube";
        }

        public void Reload()
        {
            items = new SubMenuItems(new YouTubeBuilder());
            switchPage.UpdateData(items);
        }

        public async Task Start()
        {
            if (!isActive)
            {
                items = new SubMenuItems(new YouTubeBuilder());
                isActive = true;
                await Task.Run(() =>
                {
                    SessionExiting.SetEndCurrentMediaAction(new OnSessionEndidngActions.EndCurrentMediaAction(Stop));
                });
                switchPage = new SubMenuSwitchWindow(this);
                switchPage.Show();
            }
        }

    }
}
