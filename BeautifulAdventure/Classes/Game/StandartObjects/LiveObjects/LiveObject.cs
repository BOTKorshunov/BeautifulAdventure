using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects
{
    public class LiveObject
    {
        private ImageObject _mainObject;
        private ImageObject _crackObject;
        private readonly int _startStrength;
        private int _strength;

        public ImageObject MainObject => _mainObject;
        public ImageObject CrackObject => _crackObject;
        public int Strength => _strength;

        public LiveObject(ImageObject imageObject, int strength)
        {
            int x = imageObject.X;
            int y = imageObject.Y;
            string imgSource = imageObject.Image.Source.ToString();

            _mainObject = new ImageObject(x, y, imgSource);
            _crackObject = new ImageObject(x, y, null);

            _startStrength = strength;
            _strength = strength;
        }

        public void TakeDamage(int damage)
        {
            _strength -= damage;

            if (_strength > 0) SetCrackImage();
            else Crash();
        }

        private void SetCrackImage()
        {
            double coeficient = (double)_startStrength / _strength;
            int percent = (int)Math.Round(100 / coeficient);
            string imgSource = "/Images/Crack/";
            if (percent < 10) _crackObject.SetImageSource(imgSource + "crack9.png");
            else if (percent < 20) _crackObject.SetImageSource(imgSource + "crack8.png");
            else if (percent < 30) _crackObject.SetImageSource(imgSource + "crack7.png");
            else if (percent < 40) _crackObject.SetImageSource(imgSource + "crack6.png");
            else if (percent < 50) _crackObject.SetImageSource(imgSource + "crack5.png");
            else if (percent < 60) _crackObject.SetImageSource(imgSource + "crack4.png");
            else if (percent < 70) _crackObject.SetImageSource(imgSource + "crack3.png");
            else if (percent < 80) _crackObject.SetImageSource(imgSource + "crack2.png");
            else if (percent < 90) _crackObject.SetImageSource(imgSource + "crack1.png");
            else if (percent < 100) _crackObject.SetImageSource(imgSource + "crack0.png");
        }

        private void Crash()
        {
            _mainObject.Image.Visibility = Visibility.Hidden;
            _crackObject.Image.Visibility = Visibility.Hidden;
        }
    }
}
