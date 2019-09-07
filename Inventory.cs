using System.Collections.Generic;
using System.Linq;

namespace Zuul
{
    public class Inventory
    {
        List<Item> _items = new List<Item>();
        int MaxSize => _maxSize;
        private int _maxSize;

        public Inventory(int size = 1)
        {
            _maxSize = size;
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
    }
}