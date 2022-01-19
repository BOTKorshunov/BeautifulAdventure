using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.Settings.ObjectSettings
{
    public class AllBlocks
    {
        private const string imgSource = "/Images/Blocks/";

        public readonly ObjectSetting dirt = new ObjectSetting("Земля", imgSource + "dirt.png", 10, 0);
        public readonly ObjectSetting stone = new ObjectSetting("Камень", imgSource + "stone.png", 15, 0);
    }
}
