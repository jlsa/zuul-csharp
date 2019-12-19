using Zuul;

namespace Zuul.Extensions
{
    public static class RoomExtension
    {
        public static void AddItem(this Room room, Item item)
        {
            room.AddItem(item);
        }
    }
}