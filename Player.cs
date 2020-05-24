using System;

namespace Zuul
{
    public class PlayerStats
    {
        public int InventorySize {get; set;}
        public int HealthPoints {get; set;}
        public int Strength {get; set;}
        public int Intellect {get; set;}
        public int Agility {get; set;}
        public int Sight {get; set;}
    }
    
    public class Player
    {
        public Inventory Inventory => _inventory;
        private Inventory _inventory;
        public string LongDescription => _longDescription();
        public Room Room { get => _room; }
        private Room _room { get; set; }
        public int Health { get => _health; }
        private int _health;
        public string Name { get; set; }
        private PlayerStats _basePlayerStats {get; set;}
        public PlayerStats Stats => _basePlayerStats;
        public Zuul.Entity.Npc Npc;
        public Zuul.Entity.Monster Monster;

        public Player(string name)
        {
            Name = name;
            _basePlayerStats = new PlayerStats {
                InventorySize = 10,
                HealthPoints = 100,
                Strength = 25,
                Intellect = 25,
                Agility = 25,
                Sight = 25
            };  // magic numbers
            _health = _basePlayerStats.HealthPoints;
            _inventory = new Inventory(_basePlayerStats.InventorySize);
        }

        public void EnterRoom(Room room)
        {
            _room = room;
            _untalkToNpc();
            _unfightMonster();
        }

        public Room GetCurrentRoom()
        {
            return _room;
        }

        public bool IsAlive()
        {
            return true;
        }

        public void Hurt(int damage)
        {
            // add checks here
            _health -= damage;
        }

        public string _longDescription()
        {
            // display 
            string desc = "";
            desc += Inventory.LongDescription();
            
            // You now have # items in your inventory.
            // You have no items in your inventory
            // 
            return desc;
        }

        public void ChatToNpc(Zuul.Entity.Npc npc)
        {
            this.Npc = npc;
        }

        private void _untalkToNpc()
        {
            this.Npc = null;
        }

        private void _unfightMonster()
        {
            this.Monster = null;
        }

        public void FightMonster(Zuul.Entity.Monster monster)
        {
            this.Monster = monster;
        }
    }
}