using BeautifulAdventure.Classes.Game.Rarties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautifulAdventure.Classes.Game.ObjectSettings
{
    public class ObjectSetting
    {
        private string _name;
        private string _imgSource;
        private readonly int _startHealth;
        private int _health;
        private int _attackPower;
        // возможный дроп

        public string Name => _name;
        public string ImgSource => _imgSource;
        public int StartHealth => _startHealth;
        public int Health => _health;
        public int AttackPower => _attackPower;

        public ObjectSetting(string name, string imgSource, int health, int attackPower)
        {
            _name = name;
            _imgSource = imgSource;
            _startHealth = health;
            _health = health;
            _attackPower = attackPower;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }
}
