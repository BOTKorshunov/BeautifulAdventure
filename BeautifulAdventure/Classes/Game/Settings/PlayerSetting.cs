using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.Settings
{
    public class PlayerSetting
    {
        private int _preyPower;

        public int PreyPower => _preyPower;

        public PlayerSetting(int preyPower)
        {
            _preyPower = preyPower;
        }
    }
}
