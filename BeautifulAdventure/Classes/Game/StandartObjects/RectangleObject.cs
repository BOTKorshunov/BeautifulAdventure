using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BeautifulAdventure.Classes.Game.StandartObjects
{
    public class RectangleObject : StandartObject
    {
        private Rectangle _rect = new Rectangle();

        public Rectangle Rect => _rect;

        public RectangleObject(int x, int y, Brush brush, double opacity) : base (x, y)
        {
            _rect.Fill = brush;
            _rect.Opacity = opacity;
        }

        public override void SetGridLocation()
        {
            Grid.SetColumn(_rect, X);
            Grid.SetRow(_rect, Y);
        }
    }
}
