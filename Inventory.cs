using System;
using System.Collections.Generic;
using System.Linq;

namespace Zuul
{
    public class Inventory
    {
        public List<Item> Items => _items;
        List<Item> _items = new List<Item>();
        int MaxSize => _maxSize;
        private int _maxSize;

        public Inventory(int size = 1)
        {
            _maxSize = size;
        }

        public string LongDescription()
        {
            string output = "";
            if (_items.Count() > 0)
            {
                output = "Items:";
                foreach(var item in _items)
                {
                    output += $"\n {item.Name}";
                }
            }
            else
            {
                output = "Your backpack is empty.";
            }
            return output;
        }

        public void Add(Item item)
        {
            _items.Add(item);
        }

        public Item Take(Item item)
        {
            if (HasItem(item))
            {
                _items.Remove(item); // perhaps dont remove it, only when specific requirements are met inside the item itself perhaps?
                return item;
            }

            return null;
        }

        public Item Take(string itemName)
        {
            if (HasItem(itemName))
            {
                Item item = _items.Where(i => i.Name == itemName).Single();
                return Take(item);
            }
            return null;
        }

        public bool HasItem(Item item)
        {
            return _items.Contains(item);
        }

        public bool HasItem(string itemName)
        {
            return _items.Where(i => i.Name == itemName).SingleOrDefault() != null;
        }

        public bool HasItems()
        {
            return _items.Any();
        }
    }
}