
using WinMediaBox.Types;

namespace WinMediaBox.Interfaces
{
    public interface ISubMenuItem
    {
        string option1 { get; set; }
        string img { get; set; }
        string color { get; set; }

        MediaType type { get; set; }    
    }
}
