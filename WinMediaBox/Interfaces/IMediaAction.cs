using System.Threading.Tasks;
using WinMediaBox.ViewModel.MediaActions;

namespace WinMediaBox.Interfaces
{
    public interface IMediaAction
    {
        Task Start();
        void Stop();
        bool isActive { get; set; }
        string img { get; set; }
        string title { get; set; }
        string color { get; set; }
        double cardWidth { get; set; }
        MediaActionCardsType cardsType { get; set; }
    }
}
