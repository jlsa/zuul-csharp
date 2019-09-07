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

        public Item(string name, string description, ItemType itemType)
        {
            _itemType = itemType;
            _name = name;
            _description = description;
            Enable();
            AmountOfUses = 100;
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
                return;
            }
            AmountOfUses -= 1;
            if (AmountOfUses <= 0)
            {
                Disable();
            }
        }
    }
}