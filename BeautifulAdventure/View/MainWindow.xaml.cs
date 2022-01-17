using System.Windows;
using BeautifulAdventure.Classes.Application;
using BeautifulAdventure.View.Game.Pages;

namespace BeautifulAdventure.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainAppWindow.Window = this;
            MainAppFrame.CustomFrame.Frame = FrmGame;
            FrmGame.Navigate(new GamePage());
        }
    }
}
