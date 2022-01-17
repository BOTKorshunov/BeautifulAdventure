using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects
{
    public class MovingObject : LiveObject
    {
        public MovingObject(ImageObject imageObject, int strength) : base(imageObject, strength) { }

        public void Move(int x, int y)
        {
            MainObject.SetLocation(x, y);
            CrackObject.SetLocation(x, y);
        }
    }
}
