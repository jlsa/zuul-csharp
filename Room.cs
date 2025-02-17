using System.Collections.Generic;
using System.Linq;
using Zuul.Enums;

namespace Zuul
{
    public class Room
    {
        public bool Start { get; set; }
        public string Name { get; set; }
        public List<Zuul.Entity.Npc> Npcs = new List<Zuul.Entity.Npc>();
        public List<Zuul.Entity.Monster> Monsters = new List<Zuul.Entity.Monster>();
        public List<Item> Items => _items;
        private List<Item> _items;
        public string LongDescription => _longDescription();
        public string ShortDescription => _shortDescription();
        private string _description { get; set; }
        public List<Exit> Exits = new List<Exit>();

        public Room(string description)
            : this("", description)
        {}

        public Room(string name, string description)
        {
            Name = name;
            _description = description;
            _items = new List<Item>();
        }

        public void AddMonster(Zuul.Entity.Monster monster)
        {
            Monsters.Add(monster);
        }

        public Zuul.Entity.Monster GetMonster(string name)
        {
            return Monsters.Where(m => m.ShortName.ToLower().Equals(name.ToLower()))
                .Select(m => m).FirstOrDefault();
        }

        public void AddNpc(Zuul.Entity.Npc npc)
        {
            Npcs.Add(npc);
        }

        public Zuul.Entity.Npc GetNpc(string npcName)
        {
            return Npcs.Where(n => n.ShortName.ToLower().Equals(npcName.ToLower()))
                .Select(n => n).FirstOrDefault();
        }

        public void AddExit(Exit exit)
        {
            Exits.Add(exit);
        }

        public Exit GetExit(Directions direction)
        {
            return Exits.Where(e => e.Direction == direction).SingleOrDefault();
        }
        
        public string _shortDescription()
        {
            return _description;
        }
        public string _longDescription()
        {
            string description = $"You're {_description}.";
            description += $"\n{_exitString()}";
            description += $"\n{_showItems()}";
            description += $"\n{_showNpcs()}";
            description += $"\n{_showMonsters()}";
            return description;
        }

        public string _showMonsters()
        {
            if (Monsters.Count == 0) {
                return "";
            }
            
            string returnString = "Monsters: ";
            foreach (Zuul.Entity.Monster monster in Monsters)
            {
                returnString += $" {monster.Name} ({monster.ShortName})";
            }
            return returnString;
        }

        private string _showItems()
        {
            var items = _items.Where(i => i.Enabled);
            if (items.Count() == 0) 
            {
                return "There are no items.";
            }
            string returnString = "Items: ";
            foreach (Item item in items)
            {
                returnString += " " + item.Name;
            }

            return returnString;
        }

        private string _showNpcs()
        {
            if (Npcs.Count == 0) {
                return "";
            }
            
            string returnString = "Npcs: ";
            foreach (Zuul.Entity.Npc npc in Npcs)
            {
                returnString += $" {npc.Name} ({npc.ShortName})";
            }
            return returnString;
        }

        private string _exitString()
        {
            string returnString = "Exits: ";
            foreach (Exit exit in Exits) {
                returnString += $" {exit.Direction.ToString()}";
            }
            return returnString;
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public Item GetItem(Item item)
        {
            if (_items.Contains(item)) {
                _items.Remove(item);
                return item;
            }

            return null;
        }

        public Item PeekItem(Item item)
        {
            if (_items.Contains(item))
            {
                return item;
            }

            return null;
        }
    }

}