using System;
namespace Zuul
{
    public enum ItemType
    {
        USE,
        PICKUP,
        BOTH,
        BROKEN
    }
    public class Item
    {
        public int AmountOfUses { get; set; }
        public string Name => _name;
        private string _name { get; set; }
        public ItemType ItemType => _itemType;
        private ItemType _itemType { get; set; }
        public string Description => _description;
        private string _description { get; set; }

        public bool Enabled => _enabled;
        private bool _enabled { get; set; }

        // maybe add states if used or not used? (or should we make a item type (polymorphism??) that can only be used?)

        public Item(string name, string description, ItemType itemType, int uses = 1)
        {
            _itemType = itemType;
            _name = name;
            _description = description;
            Enable();
            AmountOfUses = uses;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public bool CanBeUsed()
        {
            return _enabled 
                && (_itemType == ItemType.USE || _itemType == ItemType.BOTH);
        }

        public bool CanBeTaken()
        {
            return _enabled 
                && (_itemType == ItemType.PICKUP || _itemType == ItemType.BOTH);
        }

        // use?
        public void Use()
        {
            if (!CanBeUsed()) {
                Console.WriteLine($"{Name} can't be used. Try something else.");
                return;
            }
            AmountOfUses -= 1;

            if (AmountOfUses <= 0)
            {
                Console.WriteLine($"The {Name} is now broken.");
                Disable();
            }
            else
            {
                string amountOfUsesMsg = "";
                if (AmountOfUses > 1) {
                    amountOfUsesMsg = $"{AmountOfUses} more times.";
                } else {
                    amountOfUsesMsg = $"{AmountOfUses} more time.";
                }
                Console.WriteLine($"The {Name} is used. You can use the {Name} {amountOfUsesMsg}");
            }
        }
    }
}