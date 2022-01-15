using System.Windows;
using BeautifulAdventure.Classes.Application;

namespace BeautifulAdventure.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainAppWindow.Window = this;
            MainAppFrame.CustomFrame.Frame = FrmGame;
            //GameFrame.Navigate();
        }
    }
}
