using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeautifulAdventure.Classes.Game.StandartObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects;

namespace BeautifulAdventure.View.Game.Pages.Locations
{
    /// <summary>
    /// Логика взаимодействия для Mine.xaml
    /// </summary>
    public partial class MinePage : Page
    {
        List<StandartObject> barriers = new List<StandartObject>();
        List<LiveObject> liveObjects = new List<LiveObject>();
        MovingObject Player;

        public MinePage()
        {
            InitializeComponent();

            GridMineField.Focus();
            CreateField(10, 10);
            CreatePlayer();
        }

        private void CreateField(int columns, int rows)
        {
            for (int i = 0; i < columns; i++)
                GridMineField.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rows; i++)
                GridMineField.RowDefinitions.Add(new RowDefinition());

            CreateFloor(columns, rows);
            CreateBarriers(columns, rows);
            CreateLiveObjects(columns, rows);
        }

        private void CreateFloor(int columns, int rows)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    string imgSource = "/Images/Block/";
                    string[] blocks = { "stone.png", "dirt.png" };

                    Random random = new Random();
                    int randNum = random.Next(blocks.Length);

                    ImageObject floor = new ImageObject(column, row, imgSource + blocks[randNum]);
                    GridMineField.Children.Add(floor.Image);

                    RectangleObject shadow = new RectangleObject(column, row, Brushes.Black, 0.4);
                    GridMineField.Children.Add(shadow.Rect);

                    Thread.Sleep(1);
                }
            }
        }

        private void CreateBarriers(int columns, int rows)
        {
            string imgSource = "/Images/Block/obsidian.png";

            for (int i = 0; i < columns; i++)
            {
                ImageObject barrierUp = new ImageObject(i, 0, imgSource);
                barriers.Add(barrierUp);
                GridMineField.Children.Add(barrierUp.Image);

                ImageObject barrierDown = new ImageObject(i, rows - 1, imgSource);
                barriers.Add(barrierDown);
                GridMineField.Children.Add(barrierDown.Image);
            }
                
            for (int i = 1; i < rows - 1; i++)
            {
                ImageObject barrierUp = new ImageObject(0, i, imgSource);
                barriers.Add(barrierUp);
                GridMineField.Children.Add(barrierUp.Image);

                ImageObject barrierDown = new ImageObject(columns - 1, i, imgSource);
                barriers.Add(barrierDown);
                GridMineField.Children.Add(barrierDown.Image);
            }
        }

        private void CreateLiveObjects(int columns, int rows)
        {
            for (int row = 2; row < rows - 1; row++)
            {
                for (int column = 1; column < columns - 1; column++)
                {
                    ImageObject objectOnField = new ImageObject(column, row, "/Images/Block/dirt.png");
                    LiveObject liveObject = new LiveObject(objectOnField, 10);
                    liveObjects.Add(liveObject);
                    GridMineField.Children.Add(liveObject.MainObject.Image);
                    GridMineField.Children.Add(liveObject.CrackObject.Image);
                }
            }
        }

        private void CreatePlayer()
        {
            ImageObject imageObject = new ImageObject(1, 1, "/Images/Player/steve.png");
            Player = new MovingObject(imageObject, 20);
            GridMineField.Children.Add(Player.MainObject.Image);
            GridMineField.Children.Add(Player.CrackObject.Image);
        }

        private void ChangeCoordinates(ref int x, ref int y, Key key)
        {
            switch (key)
            {
                case Key.Left: x--; break;
                case Key.Up: y--; break;
                case Key.Right: x++; break;
                case Key.Down: y++; break;
            }
        }

        private StandartObject FindBarrier(Key key)
        {
            int xPlayer = Player.MainObject.X;
            int yPlayer = Player.MainObject.Y;
            ChangeCoordinates(ref xPlayer, ref yPlayer, key);

            StandartObject findBarrier = 
                barriers.FirstOrDefault(barrier => barrier.X == xPlayer && barrier.Y == yPlayer);

            return findBarrier;
        }

        private LiveObject FindLiveObject(Key key)
        {
            int xPlayer = Player.MainObject.X;
            int yPlayer = Player.MainObject.Y;
            ChangeCoordinates(ref xPlayer, ref yPlayer, key);

            LiveObject findLiveObject = 
                liveObjects.FirstOrDefault(liveObject => liveObject.MainObject.X == xPlayer
                        && liveObject.MainObject.Y == yPlayer);

            return findLiveObject;
        }

        private bool CheckFuturePlayerLocation(Key key)
        {
            StandartObject findBarrier = FindBarrier(key);
            if (findBarrier != null) return false;

            LiveObject findLiveObject = FindLiveObject(key);
            if (findLiveObject != null && findLiveObject.Strength > 0)
            {
                findLiveObject.TakeDamage(1);
                return false;
            }

            return true;
        }

        private void GridMineField_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (CheckFuturePlayerLocation(e.Key))
            {
                int xPlayer = Player.MainObject.X;
                int yPlayer = Player.MainObject.Y;
                ChangeCoordinates(ref xPlayer, ref yPlayer, e.Key);

                Player.Move(xPlayer, yPlayer);
            }

            e.Handled = true;
        }
    }
}
