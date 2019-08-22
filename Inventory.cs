using System.Collections.Generic;

namespace Zuul
{
    public class Inventory
    {
        List<Item> _items = new List<Item>();
        int MaxSize => _maxSize;

        private int _maxSize;

        public Inventory(int size)
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
                _items.Remove(item);
                return item;
            }

            return null;
        }

        public bool HasItem(Item item)
        {
            return _items.Contains(item);
        }
    }
}