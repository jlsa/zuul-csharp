using System;
using System.Collections;
using System.Collections.Generic;

namespace Zuul
{
    public class Room
    {
        public List<Item> Items => _items;
        private List<Item> _items;
        public string LongDescription => _longDescription();
        public string ShortDescription => _shortDescription();

        private string _description { get; set; }
        // hashmap <String, Room> exits; // stores exits of this room.
        private Dictionary<Directions, Room> _exits;

        public Room(string description)
        {
            _description = description;
            _exits = new Dictionary<Directions, Room>();
            _items = new List<Item>();
        }

        public void AddExit(Directions direction, Room neighbor)
        {
            _exits.Add(direction, neighbor);
        }

        public Room GetExit(Directions direction)
        {
            Room room = null;
            if (!_exits.TryGetValue(direction, out room))
            {
                // key doesnt exist.
            }
            return room;
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
            foreach (Item item in _items)
            {
                returnString += " " + item.Name;
            }

            return returnString;
        }

        private string _exitString()
        {
            string returnString = "Exits: ";
            foreach (KeyValuePair<Directions, Room> entry in _exits)
            {
                // entry.Value
                // entry.Key
                returnString += $" {entry.Key}";
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