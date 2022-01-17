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
using BeautifulAdventure.Classes.Game.ObjectSettings;
using BeautifulAdventure.Classes.Game.Rarties;
using BeautifulAdventure.Classes.Game.StandartObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects.MovingObjects;

namespace BeautifulAdventure.View.Game.Pages.Locations
{
    /// <summary>
    /// Логика взаимодействия для Mine.xaml
    /// </summary>
    public partial class MinePage : Page
    {
        private static Random _rnd = new Random();

        private ObjectSettingWithRarity[] _possibleBlocks;
        private ObjectSettingWithRarity[] _possibleMobs;
        private List<ImageObject> _barriers = new List<ImageObject>();
        private List<LiveObject> _blocksOnField = new List<LiveObject>();
        private List<MovingObject> _mobsOnField = new List<MovingObject>();
        private MovingObject _player;

        public MinePage()
        {
            InitializeComponent();

            GridMineField.Focus();
            SetPossibleBlocks();
            SetPossibleMobs();
            CreateField(15, 15);
            CreatePlayer();
        }

        private void SetPossibleBlocks()
        {
            AllBlocks allBlocks = new AllBlocks();
            AllRarities allRarities = new AllRarities();

            _possibleBlocks = new ObjectSettingWithRarity[]
            {
                new ObjectSettingWithRarity(allBlocks.dirt, allRarities.Common),
                new ObjectSettingWithRarity(allBlocks.stone, allRarities.Uncommon)
            };
        }

        private void SetPossibleMobs()
        {
            AllMobs allMobs = new AllMobs();
            AllRarities allRarities = new AllRarities();

            _possibleMobs = new ObjectSettingWithRarity[]
            {
                new ObjectSettingWithRarity(allMobs.zombie, allRarities.Common),
                new ObjectSettingWithRarity(allMobs.skeleton, allRarities.Uncommon)
            };
        }

        private void CreateField(int columns, int rows)
        {
            for (int i = 0; i < columns; i++)
                GridMineField.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < rows; i++)
                GridMineField.RowDefinitions.Add(new RowDefinition());

            CreateFloor(columns, rows);
            CreateBarriers(columns, rows);
            CreateBlocksAndMobs(columns, rows);
        }

        private void CreateFloor(int columns, int rows)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    ObjectSetting block = GetRandomObjectSetting(_possibleBlocks);

                    ImageObject floor = new ImageObject(column, row, block.ImgSource);
                    GridMineField.Children.Add(floor.Image);

                    RectangleObject shadow = new RectangleObject(column, row, Brushes.Black, 0.4);
                    GridMineField.Children.Add(shadow.Rect);
                }
            }
        }

        private void CreateBarriers(int columns, int rows)
        {
            string imgSource = "/Images/Blocks/obsidian.png";

            for (int i = 0; i < columns; i++)
            {
                ImageObject barrierUp = new ImageObject(i, 0, imgSource);
                _barriers.Add(barrierUp);
                GridMineField.Children.Add(barrierUp.Image);

                ImageObject barrierDown = new ImageObject(i, rows - 1, imgSource);
                _barriers.Add(barrierDown);
                GridMineField.Children.Add(barrierDown.Image);
            }
                
            for (int i = 1; i < rows - 1; i++)
            {
                ImageObject barrierUp = new ImageObject(0, i, imgSource);
                _barriers.Add(barrierUp);
                GridMineField.Children.Add(barrierUp.Image);

                ImageObject barrierDown = new ImageObject(columns - 1, i, imgSource);
                _barriers.Add(barrierDown);
                GridMineField.Children.Add(barrierDown.Image);
            }
        }

        private void CreateBlocksAndMobs(int columns, int rows)
        {
            Rarity[] objectRarities =
            {
                new Rarity("Блок", 60),
                new Rarity("Моб", 5),
                new Rarity("Ничего", 35)
            };

            int objectRaririesSum = objectRarities.Select(rarity => rarity.Chance).Sum();
            for (int row = 2; row < rows - 1; row++)
            {
                for (int column = 1; column < columns - 1; column++)
                {
                    int randNum = _rnd.Next(objectRaririesSum);
                    int sum = 0;
                    Rarity selectRarity = null;

                    foreach (var rarity in objectRarities)
                    {
                        if (randNum >= sum && randNum <= sum + rarity.Chance)
                        {
                            selectRarity = rarity;
                            break;
                        }
                        else
                            sum += rarity.Chance;
                    }

                    if (selectRarity == objectRarities[0]) CreateBlock(column, row); 
                    else if (selectRarity == objectRarities[1]) CreateMob(column, row);
                }
            }

            foreach (var block in _blocksOnField)
            {
                GridMineField.Children.Add(block.MainObject.Image);
                GridMineField.Children.Add(block.CrackObject.Image);
            }

            foreach (var mob in _mobsOnField)
            {
                GridMineField.Children.Add(mob.MainObject.Image);
                GridMineField.Children.Add(mob.CrackObject.Image);
            }
        }

        private ObjectSetting GetRandomObjectSetting(ObjectSettingWithRarity[] objectSettingWithRarities)
        {
            int raritiesSum = objectSettingWithRarities.Select
                (objectSettingWithRarity => objectSettingWithRarity.Rarity.Chance).Sum();
            int randNum = _rnd.Next(raritiesSum);

            raritiesSum = 0;
            foreach (var objectSettingWithRarity in objectSettingWithRarities)
            {
                if (randNum >= raritiesSum &&
                    randNum <= raritiesSum + objectSettingWithRarity.Rarity.Chance)
                {
                    string name = objectSettingWithRarity.ObjectSetting.Name;
                    string imgSource = objectSettingWithRarity.ObjectSetting.ImgSource;
                    int health = objectSettingWithRarity.ObjectSetting.Health;
                    int attackPower = objectSettingWithRarity.ObjectSetting.AttackPower;
                    Thread.Sleep(1);
                    return new ObjectSetting(name, imgSource, health, attackPower);
                }
                else
                    raritiesSum += objectSettingWithRarity.Rarity.Chance;
            }

            return null;
        }

        private void CreateBlock(int x, int y)
        {
            ObjectSetting blockSetting = GetRandomObjectSetting(_possibleBlocks);
            LiveObject liveObject = new LiveObject(x, y, blockSetting);
            _blocksOnField.Add(liveObject);
        }

        private void CreateMob(int x, int y)
        {
            ObjectSetting mobSetting = GetRandomObjectSetting(_possibleMobs);
            MovingObject movingObject = new MovingObject(x, y, mobSetting);
            _mobsOnField.Add(movingObject);
        }

        private void CreatePlayer()
        {
            ObjectSetting objectSetting = new ObjectSetting("Игрок", "/Images/Player/steve.png", 20, 1);
            _player = new MovingObject(1, 1, objectSetting);
            GridMineField.Children.Add(_player.MainObject.Image);
            GridMineField.Children.Add(_player.CrackObject.Image);
        }

        private void ChangeLocation(ref int x, ref int y, Key key)
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
            int xPlayer = _player.MainObject.X;
            int yPlayer = _player.MainObject.Y;
            ChangeLocation(ref xPlayer, ref yPlayer, key);

            StandartObject findBarrier = 
                _barriers.FirstOrDefault(barrier => barrier.X == xPlayer && barrier.Y == yPlayer);

            return findBarrier;
        }

        private LiveObject FindLiveObject(Key key)
        {
            int xPlayer = _player.MainObject.X;
            int yPlayer = _player.MainObject.Y;
            ChangeLocation(ref xPlayer, ref yPlayer, key);

            LiveObject findLiveObject = 
                _blocksOnField.FirstOrDefault(liveObject => liveObject.MainObject.X == xPlayer
                        && liveObject.MainObject.Y == yPlayer);

            return findLiveObject;
        }

        private MovingObject FindMovingObject(Key key)
        {
            int xPlayer = _player.MainObject.X;
            int yPlayer = _player.MainObject.Y;
            ChangeLocation(ref xPlayer, ref yPlayer, key);

            MovingObject findMovingObject =
                _mobsOnField.FirstOrDefault(movingObject => movingObject.MainObject.X == xPlayer
                        && movingObject.MainObject.Y == yPlayer);

            return findMovingObject;
        }

        private bool CheckFuturePlayerLocation(Key key)
        {
            StandartObject findBarrier = FindBarrier(key);
            if (findBarrier != null) return false;

            LiveObject findLiveObject = FindLiveObject(key);
            if (findLiveObject != null && findLiveObject.ObjectSetting.Health > 0)
            {
                findLiveObject.TakeDamage(_player.ObjectSetting.AttackPower);
                return false;
            }

            MovingObject findMovingObject = FindMovingObject(key);
            if (findMovingObject != null && findMovingObject.ObjectSetting.Health > 0)
            {
                findMovingObject.TakeDamage(_player.ObjectSetting.AttackPower);
                return false;
            }

            return true;
        }

        private void GridMineField_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (CheckFuturePlayerLocation(e.Key))
            {
                int xPlayer = _player.MainObject.X;
                int yPlayer = _player.MainObject.Y;
                ChangeLocation(ref xPlayer, ref yPlayer, e.Key);

                _player.Move(xPlayer, yPlayer);
            }

            e.Handled = true;
        }
    }
}
