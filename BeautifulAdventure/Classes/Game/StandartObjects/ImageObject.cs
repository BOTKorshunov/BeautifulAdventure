using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BeautifulAdventure.Classes.Game.StandartObjects
{
    public class ImageObject : StandartObject
    {
        private Image _image = new Image();

        public Image Image => _image;

        public ImageObject(int x, int y, string imgSource) : base(x, y)
        {
            _image.Stretch = Stretch.Fill;
            SetImageSource(imgSource);
        }

        public void SetImageSource(string imgSource)
        {
            if (imgSource != null)
                _image.Source = new BitmapImage(new Uri(imgSource, UriKind.Relative));
        }

        public override void SetGridLocation()
        {
            Grid.SetColumn(_image, X);
            Grid.SetRow(_image, Y);
        }
    }
}
