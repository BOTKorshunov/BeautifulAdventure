using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.Rarties
{
    public class AllRarities
    {
        public readonly Rarity Common = new Rarity("Обычный", 58);
        public readonly Rarity Uncommon = new Rarity("Необычный", 22);
        public readonly Rarity Rare = new Rarity("Редкий", 12);
        public readonly Rarity Mythical = new Rarity("Мифический", 7);
        public readonly Rarity Legendary = new Rarity("Легендарный", 1);
    }
}
