using System.Threading.Tasks;
using WinMediaBox.Classes;
using WinMediaBox.Interfaces;
using WinMediaBox.View;
using WinMediaBox.ViewModel.SubMediaActions;
using WinMediaBox.ViewModel.SubMediaActions.Builders;

namespace WinMediaBox.ViewModel.MediaActions
{
    public class TwitchMediaAction : MediaActionWebEmbedBase, IMediaAction, IResizable, ISubMediaAction
    {
        public MediaActionCardsType cardsType { get; set; } = MediaActionCardsType.Standart;

        public TwitchMediaAction()
        {
            type = EmbedWebType.Twitch;
            img = "/img/twitch.png";
            color = "#61429E";
            title = "Twitch";
        }

        public void Reload()
        {
            items = new SubMenuItems(new TwitchBuilder());
            switchPage.UpdateData(items);
        }

        public async Task Start()
        {
            if (!isActive)
            {
                items = new SubMenuItems(new TwitchBuilder());
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
