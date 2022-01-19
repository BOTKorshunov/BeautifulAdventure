using BeautifulAdventure.Classes.Game.Rarties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.Settings.ObjectSettings
{
    public class ObjectSettingWithRarity
    {
        private ObjectSetting _objectSetting;
        private Rarity _rarity;

        public ObjectSetting ObjectSetting => _objectSetting;
        public Rarity Rarity => _rarity;

        public ObjectSettingWithRarity(ObjectSetting objectSetting, Rarity rarity)
        {
            _objectSetting = objectSetting;
            _rarity = rarity;
        }
    }
}
