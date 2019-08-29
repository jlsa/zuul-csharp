using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zuul
{
    public class Exit
    {
        public Directions Direction {get; set;}
        public Room Room {get; set;}
        public bool Locked { get; set; }

        public void Unlock(Item key)
        {
            Locked = false;
        }

        public void Lock(Item key)
        {
            Locked = true;
        }
    }

    public class Room
    {
        public List<Item> Items => _items;
        private List<Item> _items;
        public string LongDescription => _longDescription();
        public string ShortDescription => _shortDescription();
        private string _description { get; set; }

        public List<Exit> Exits = new List<Exit>();

        public Room(string description)
        {
            _description = description;
            _items = new List<Item>();
        }

        public void AddExit(Exit exit)
        {
            Exits.Add(exit);
        }

        public Exit GetExit(Directions direction)
        {
            return Exits.Where(e => e.Direction == direction).SingleOrDefault();
            // foreach (Exit exit in Exits)
            // {
            //     if (exit.Direction == direction)
            //     {
            //         return exit;
            //     }
            // }
            // return null;
        }
        
        public string _shortDescription()
        {
            return _description;
        }
        public string _longDescription()
        {
            return $"You're {_description}.\n{_exitString()}\n{_showItems()}";
        }

        private string _showItems()
        {
            string returnString = "Items: ";
            foreach (Item item in _items.Where(i => i.Enabled))
            {
                returnString += " " + item.Name;
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