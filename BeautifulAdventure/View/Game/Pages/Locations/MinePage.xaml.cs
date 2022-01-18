using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BeautifulAdventure.Classes.Game.ObjectSettings;
using BeautifulAdventure.Classes.Game.Rarties;
using BeautifulAdventure.Classes.Game.StandartObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects;
using BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects.MovingObjects;

namespace BeautifulAdventure.View.Game.Pages.Locations
{
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
            ObjectSetting objectSetting = new ObjectSetting("Игрок", "/Images/Player/steve.png", 20, 1);
            _player = new MovingObject(1, 1, objectSetting);
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
        private StandartObject FindBarrier(Key key)
        {
            int xPlayer = _player.MainObject.X;
            int yPlayer = _player.MainObject.Y;
            ChangeLocation(ref xPlayer, ref yPlayer, key);

            StandartObject findBarrier = 
                _barriers.FirstOrDefault(barrier => barrier.X == xPlayer && barrier.Y == yPlayer);

            return findBarrier;
        }

        /// <summary>
        /// Ищет блок или моба перед игроком
        /// </summary>
        /// <param name="liveObjects"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private LiveObject FindLiveObject(LiveObject[] liveObjects, Key key)
        {
            int xPlayer = _player.MainObject.X;
            int yPlayer = _player.MainObject.Y;
            ChangeLocation(ref xPlayer, ref yPlayer, key);

            LiveObject findLiveObject =
                liveObjects.FirstOrDefault(liveObject => liveObject.MainObject.X == xPlayer
                        && liveObject.MainObject.Y == yPlayer);

            return findLiveObject;
        }

        /// <summary>
        /// Проверкяет будущую позицию игрока на наличие препятствий
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true, если на будущей позиции нет препятствия, false - если есть</returns>
        private bool CheckFuturePlayerLocation(Key key)
        {
            StandartObject findBarrier = FindBarrier(key);
            if (findBarrier != null) return false;

            LiveObject findBlock = FindLiveObject(_blocksOnField.ToArray(), key);
            if (findBlock != null)
            {
                if (findBlock.ObjectSetting.Health > 0)
                {
                    findBlock.TakeDamage(_player.ObjectSetting.AttackPower);
                    return false;
                }
                else _blocksOnField.Remove(findBlock);
            }

            MovingObject findMob = (MovingObject)FindLiveObject(_mobsOnField.ToArray(), key);
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
