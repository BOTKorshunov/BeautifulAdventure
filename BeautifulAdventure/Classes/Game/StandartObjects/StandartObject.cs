using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BeautifulAdventure.Classes.Game.StandartObjects
{
    public abstract class StandartObject
    {
        private int _x;
        private int _y;

        public int X => _x;
        public int Y => _y;

        public StandartObject(int x, int y)
        {
            SetLocation(x, y);
        }

        public void SetLocation(int x, int y)
        {
            _x = x;
            _y = y;
            SetGridLocation();
        }

        public abstract void SetGridLocation();
    }
}
