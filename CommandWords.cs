using System;

namespace Zuul
{
    /*
        Refactor this to use JSON based commands.
     */
    public class CommandWords
    {
        private string[] _validCommands = new string[] {
            "go",
            "quit",
            "help",
            "take",
            "use",
            "inventory",
            "unlock",
            "look",
            // "hit",
            // "drop",
            // "inspect",
            // "lock",
        };

        public CommandWords()
        {
            // nothing to do at the moment...
        }

        public bool IsCommandValid(string cmd)
        {
            for (int i = 0; i < _validCommands.Length; i++)
            {
                if (_validCommands[i].Equals(cmd))
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowAll()
        {
            foreach (string cmd in _validCommands)
            {
                Console.Write($"{cmd.ToUpper()} ");
            }
            // write the usage examples for each command
            // example: go east(e/E) | west(w/W) | north (n/N) | south (s/S)
            
            Console.WriteLine();
        }
    }
}