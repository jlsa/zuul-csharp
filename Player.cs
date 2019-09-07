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
        private int _health = 10; // magic number
        public string Name { get; set; }
        private PlayerStats _basePlayerStats {get; set;}
        public PlayerStats Stats {get; set;}

        public Player(string name)
        {
            Name = name;
            _inventory = new Inventory(10); // magic number
        }

        public void EnterRoom(Room room)
        {
            _room = room;
        }

        public Room GetCurrentRoom()
        {
            return _room;
        }

        public bool IsAlive()
        {
            return true;
        }

        public void DoDamage(int amountOfDamage)
        {
            // add checks here
            _health -= amountOfDamage;
        }

        public string _longDescription()
        {
            // display 
            string desc = "";
            
            // You now have # items in your inventory.
            // You have no items in your inventory
            // 
            return desc;
        }
    }
}