using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.Rarties
{
    public class Rarity
    {
        private string _name;
        private int _chance;

        public string Name => _name;
        public int Chance => _chance;

        public Rarity(string name, int chance)
        {
            _name = name;
            _chance = chance;
        }
    }
}
