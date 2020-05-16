using System;

namespace Zuul
{
    class Program
    {
        static void RunGame()
        {
            Game game = new Game();
            game.Run();
        }

        static void Main(string[] args)
        {
            RunGame();
        }
    }
}
