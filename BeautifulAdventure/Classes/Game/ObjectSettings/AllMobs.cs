using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.ObjectSettings
{
    public class AllMobs
    {
        private const string imgSource = "/Images/Mobs/";

        public readonly ObjectSetting zombie = new ObjectSetting("Зомби", imgSource + "zombie.png", 10, 2);
        public readonly ObjectSetting skeleton = new ObjectSetting("Скелет", imgSource + "skeleton.png", 15, 4);
    }
}
