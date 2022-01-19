using BeautifulAdventure.Classes.Game.Settings.ObjectSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects.MovingObjects
{
    public class MovingObject : LiveObject
    {
        public MovingObject(int x, int y, ObjectSetting objectSetting) : base(x, y, objectSetting) { }

        public void Move(int x, int y)
        {
            MainObject.SetLocation(x, y);
            CrackObject.SetLocation(x, y);
        }
    }
}
