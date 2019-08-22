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
        public string Name => _name;
        private string _name { get; set; }
        public ItemType ItemType => _itemType;
        private ItemType _itemType { get; set; }
        public string Description => _description;
        private string _description { get; set; }

        public Item(string name, string description, ItemType itemType)
        {
            _itemType = itemType;
            _name = name;
            _description = description;
        }

        public bool CanBeUsed()
        {
            return _itemType == ItemType.USE || _itemType == ItemType.BOTH;
        }

        public bool CanBeTaken()
        {
            return _itemType == ItemType.PICKUP || _itemType == ItemType.BOTH;
        }

        // use?
    }
}