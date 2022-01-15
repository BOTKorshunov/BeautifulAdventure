using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BeautifulAdventure.Classes.Game;

namespace BeautifulAdventure.View.Game.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();

            GameFieldFrame.CustomFrame.Frame = FrmGameField;
        }
    }
}
