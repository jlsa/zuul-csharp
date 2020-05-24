using Zuul.Enums;

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
}