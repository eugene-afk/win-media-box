using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using WinMediaBox.ViewModel;

namespace WinMediaBox.View
{
    public partial class RadioViewWindow : Window
    {

        public RadioViewWindow(string uri, string img)
        {
            InitializeComponent();
            DataContext = new RadioViewModel(uri, img, Player);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Player.Close();
        }
    }
}
