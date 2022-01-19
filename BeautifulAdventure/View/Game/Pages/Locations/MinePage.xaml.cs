using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BeautifulAdventure.Classes.Game;
using BeautifulAdventure.Classes.Game.Settings.ObjectSettings;
using BeautifulAdventure.Classes.Game.Rarties;
using BeautifulAdventure.Classes.Game.StandartObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects.MovingObjects;
using BeautifulAdventure.Classes.Game.Settings;

namespace BeautifulAdventure.View.Game.Pages.Locations
{
    public partial class MinePage : Page
    {
        private static Random _rnd = new Random();
        private static Key[] _possibleKeys = { Key.Left, Key.Up, Key.Right, Key.Down };

        private ObjectSettingWithRarity[] _possibleBlocks;
        private ObjectSettingWithRarity[] _possibleMobs;
        private List<ImageObject> _barriers = new List<ImageObject>();
        private List<LiveObject> _blocksOnField = new List<LiveObject>();
        private List<MovingObject> _mobsOnField = new List<MovingObject>();
        private PlayerObject _player;

        public MinePage()
        {
            InitializeComponent();

            GridMineField.Focus();
            SetPossibleBlocks();
            SetPossibleMobs();
            CreateField(15, 15);
            CreatePlayer();
        }

        /// <summary>
        /// Устанавливает возможные настройки блоков
        /// </summary>
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

        /// <summary>
        /// Устанавливает возможные настройки мобов
        /// </summary>
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

        /// <summary>
        /// Добавляет объект на игровое поле
        /// </summary>
        /// <param name="element"></param>
        private void AddObjectInGameField(FrameworkElement element)
        {
            GridMineField.Children.Add(element);
        }

        /// <summary>
        /// Создаёт игровое поле
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
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

        /// <summary>
        /// Создаёт поверхность (пол) на игрвом поле
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        private void CreateFloor(int columns, int rows)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    ObjectSettingWithRarity randomObjectSettingWithRarity = GetRandomObjectSettingWithRarity(_possibleBlocks);
                    ObjectSetting blockSetting = GetNewObjectSetting(randomObjectSettingWithRarity);

                    ImageObject floor = new ImageObject(column, row, blockSetting.ImgSource);
                    GridMineField.Children.Add(floor.Image);

                    RectangleObject shadow = new RectangleObject(column, row, Brushes.Black, 0.4);
                    GridMineField.Children.Add(shadow.Rect);
                }
            }
        }

        /// <summary>
        /// Создаёт барьеры (неломаемые объекты) на игровом поле
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        private void CreateBarriers(int columns, int rows)
        {
            string imgSource = "/Images/Blocks/obsidian.png";

            for (int i = 0; i < columns; i++)
            {
                ImageObject barrierUp = new ImageObject(i, 0, imgSource);
                _barriers.Add(barrierUp);
                AddObjectInGameField(barrierUp.Image);

                ImageObject barrierDown = new ImageObject(i, rows - 1, imgSource);
                _barriers.Add(barrierDown);
                AddObjectInGameField(barrierDown.Image);
            }
                
            for (int i = 1; i < rows - 1; i++)
            {
                ImageObject barrierUp = new ImageObject(0, i, imgSource);
                _barriers.Add(barrierUp);
                AddObjectInGameField(barrierUp.Image);

                ImageObject barrierDown = new ImageObject(columns - 1, i, imgSource);
                _barriers.Add(barrierDown);
                GridMineField.Children.Add(barrierDown.Image);
            }
        }

        /// <summary>
        /// Создаёт блоки и мобов на игровом поле
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        private void CreateBlocksAndMobs(int columns, int rows)
        {
            ObjectSettingWithRarity[] objectSettingWithRarities =
            {
                new ObjectSettingWithRarity(null, new Rarity("Блок", 60)),
                new ObjectSettingWithRarity(null, new Rarity("Моб", 5)),
                new ObjectSettingWithRarity(null, new Rarity("Ничего", 35)),
            };

            for (int row = 2; row < rows - 1; row++)
            {
                for (int column = 1; column < columns - 1; column++)
                {
                    ObjectSettingWithRarity randomObjectSettingWithRarity = GetRandomObjectSettingWithRarity(objectSettingWithRarities);

                    if (randomObjectSettingWithRarity == objectSettingWithRarities[0]) CreateBlock(column, row); 
                    else if (randomObjectSettingWithRarity == objectSettingWithRarities[1]) CreateMob(column, row);
                }
            }

            foreach (var block in _blocksOnField)
            {
                AddObjectInGameField(block.MainObject.Image);
                AddObjectInGameField(block.CrackObject.Image);
            }

            foreach (var mob in _mobsOnField)
            {
                AddObjectInGameField(mob.MainObject.Image);
                AddObjectInGameField(mob.CrackObject.Image);
            }
        }

        /// <summary>
        /// Выбирает случайную настройку объекта с редкостью в зависимости от редкости
        /// </summary>
        /// <param name="objectSettingWithRarities"></param>
        /// <returns>Случайная настройка объекта с редкостью</returns>
        private ObjectSettingWithRarity GetRandomObjectSettingWithRarity(ObjectSettingWithRarity[] objectSettingWithRarities)
        {
            int raritiesSum = objectSettingWithRarities.Select
                (objectSettingWithRarity => objectSettingWithRarity.Rarity.Chance).Sum();
            int randNum = _rnd.Next(raritiesSum);

            raritiesSum = 0;
            ObjectSettingWithRarity randomObjectSettingWithRarity = null;
            foreach (var objectSettingWithRarity in objectSettingWithRarities)
            {
                if (randNum >= raritiesSum &&
                    randNum <= raritiesSum + objectSettingWithRarity.Rarity.Chance)
                {
                    randomObjectSettingWithRarity = objectSettingWithRarity;
                    break;
                }
                else
                    raritiesSum += objectSettingWithRarity.Rarity.Chance;
            }

            return randomObjectSettingWithRarity;
        }

        /// <summary>
        /// Создаёт новую настройку объекта на основе входной настройки объекта с редкостью
        /// </summary>
        /// <param name="objectSettingWithRarity"></param>
        /// <returns>Новая настройка объекта</returns>
        private ObjectSetting GetNewObjectSetting(ObjectSettingWithRarity objectSettingWithRarity)
        {
            string name = objectSettingWithRarity.ObjectSetting.Name;
            string imgSource = objectSettingWithRarity.ObjectSetting.ImgSource;
            int health = objectSettingWithRarity.ObjectSetting.Health;
            int attackPower = objectSettingWithRarity.ObjectSetting.AttackPower;
            return new ObjectSetting(name, imgSource, health, attackPower);
        }

        /// <summary>
        /// Создаёт блок
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CreateBlock(int x, int y)
        {
            ObjectSettingWithRarity randomObjectSettingWithRarity = GetRandomObjectSettingWithRarity(_possibleBlocks);
            ObjectSetting blockSetting = GetNewObjectSetting(randomObjectSettingWithRarity);
            LiveObject block = new LiveObject(x, y, blockSetting);
            _blocksOnField.Add(block);
        }

        /// <summary>
        /// Создаёт моба
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CreateMob(int x, int y)
        {
            ObjectSettingWithRarity randomObjectSettingWithRarity = GetRandomObjectSettingWithRarity(_possibleMobs);
            ObjectSetting mobSetting = GetNewObjectSetting(randomObjectSettingWithRarity);
            MovingObject movingObject = new MovingObject(x, y, mobSetting);
            _mobsOnField.Add(movingObject);
        }

        /// <summary>
        /// Создаёт игрока
        /// </summary>
        private void CreatePlayer()
        {
            ObjectSetting objectSetting = new ObjectSetting("Игрок", "/Images/Player/steve.png", 20, 2);
            PlayerSetting playerSetting = new PlayerSetting(3);
            _player = new PlayerObject(1, 1, objectSetting, playerSetting);
            GridMineField.Children.Add(_player.MainObject.Image);
            GridMineField.Children.Add(_player.CrackObject.Image);
        }

        /// <summary>
        /// Меняет значение x или y в зависимости от направления нажатой клавиши
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="key"></param>
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

        /// <summary>
        /// Ищет барьер перед игроком
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Найденный барьер</returns>
        private ImageObject FindBarrier(int x, int y)
        {
            ImageObject findBarrier = _barriers.FirstOrDefault(barrier => barrier.X == x && barrier.Y == y);

            return findBarrier;
        }

        /// <summary>
        /// Ищет блок или моба перед игроком
        /// </summary>
        /// <param name="liveObjects"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private LiveObject FindLiveObject(LiveObject[] liveObjects, int x, int y)
        {
            LiveObject findLiveObject =
                liveObjects.FirstOrDefault(liveObject => liveObject.MainObject.X == x
                        && liveObject.MainObject.Y == y);

            return findLiveObject;
        }

        /// <summary>
        /// Двигает всех мобов на игровом поле
        /// </summary>
        private void AllMobsMove()
        {
            foreach (var mob in _mobsOnField)
            {
                List<Key> ways = new List<Key>();
                int xMob = mob.MainObject.X;
                int yMob = mob.MainObject.Y;

                foreach (var key in _possibleKeys)
                {
                    int x = xMob;
                    int y = yMob;
                    ChangeLocation(ref x, ref y, key);

                    ImageObject findBarrier = FindBarrier(x, y);
                    if (findBarrier != null) continue;

                    LiveObject findBlock = FindLiveObject(_blocksOnField.ToArray(), x, y);
                    if (findBlock != null) continue;

                    LiveObject findMob = FindLiveObject(_mobsOnField.ToArray(), x, y);
                    if (findMob != null) continue;

                    if (x == _player.MainObject.X && y == _player.MainObject.Y) continue;
                        
                    ways.Add(key);
                }

                if (ways.Count > 0)
                {
                    int rndNum = _rnd.Next(ways.Count * 2);
                    if (rndNum < ways.Count)
                    {
                        Key way = ways[rndNum];
                        ChangeLocation(ref xMob, ref yMob, way);
                        mob.Move(xMob, yMob);
                    }
                }
            }
        }

        /// <summary>
        /// Производит атаку всех мобов (если находятся рядом с игроком)
        /// </summary>
        private void AllMobsAttack()
        {
            foreach (var mob in _mobsOnField)
            {
                int xMob = mob.MainObject.X;
                int yMob = mob.MainObject.Y;

                foreach (var key in _possibleKeys)
                {
                    int x = xMob;
                    int y = yMob;
                    ChangeLocation(ref x, ref y, key);

                    if (x == _player.MainObject.X && y == _player.MainObject.Y)
                    {
                        _player.TakeDamage(mob.ObjectSetting.AttackPower);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет будущую позицию игрока на наличие препятствий
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true, если на будущей позиции нет препятствия, false - если есть</returns>
        private bool CheckFuturePlayerLocation(Key key)
        {
            int x = _player.MainObject.X;
            int y = _player.MainObject.Y;
            ChangeLocation(ref x, ref y, key);

            ImageObject findBarrier = FindBarrier(x, y);
            if (findBarrier != null) return false;

            LiveObject findBlock = FindLiveObject(_blocksOnField.ToArray(), x, y);
            if (findBlock != null)
            {
                if (findBlock.ObjectSetting.Health > 0)
                {
                    findBlock.TakeDamage(_player.PlayerSetting.PreyPower);
                    return false;
                }
                else _blocksOnField.Remove(findBlock);
            }

            MovingObject findMob = (MovingObject)FindLiveObject(_mobsOnField.ToArray(), x, y);
            if (findMob != null)
            {
                if (findMob.ObjectSetting.Health > 0)
                {
                    findMob.TakeDamage(_player.ObjectSetting.AttackPower);
                    return false;
                }
                else _mobsOnField.Remove(findMob);
            }

            return true;
        }

        private void GridMineField_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void GridMineField_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Key selectWay = _possibleKeys.FirstOrDefault(way => way == e.Key);

            if (selectWay != Key.None)
            {
                if (CheckFuturePlayerLocation(e.Key))
                {
                    int xPlayer = _player.MainObject.X;
                    int yPlayer = _player.MainObject.Y;
                    ChangeLocation(ref xPlayer, ref yPlayer, e.Key);

                    _player.Move(xPlayer, yPlayer);
                    AllMobsMove();
                }

                AllMobsAttack();

                if (_player.ObjectSetting.Health <= 0)
                {
                    MessageBox.Show("Вы умерли!");
                    GameFieldFrame.CustomFrame.Navigate(new MinePage());
                }
            }

            e.Handled = true;
        }
    }
}
