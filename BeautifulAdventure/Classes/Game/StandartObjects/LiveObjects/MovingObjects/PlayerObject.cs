using BeautifulAdventure.Classes.Game.Settings;
using BeautifulAdventure.Classes.Game.Settings.ObjectSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.StandartObjects.LiveObjects.MovingObjects
{
    public class PlayerObject : MovingObject
    {
        private PlayerSetting _playerSetting;

        public PlayerSetting PlayerSetting => _playerSetting;

        public PlayerObject(int x, int y, ObjectSetting objectSetting, PlayerSetting playerSetting)
            : base(x, y, objectSetting)
        {
            _playerSetting = playerSetting;
        }
    }
}
