using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinMediaBox.Types;

namespace WinMediaBox.Interfaces
{
    public interface IPlayerSelectable
    {
        void SetPlayer(PlayerType playerType);
    }
}
