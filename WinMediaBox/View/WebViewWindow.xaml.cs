using System;
using System.Reflection;
using System.Windows;

namespace WinMediaBox.View
{
    public partial class WebViewWindow : Window
    {
        public WebViewWindow(string html)
        {
            InitializeComponent();
            WB.NavigateToString(html);

            //to ignore harmless embed player script errors
            dynamic activeX = WB.GetType().InvokeMember("ActiveXInstance",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, WB, new object[] { });

            activeX.Silent = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WB.Dispose();
        }

    }
}
