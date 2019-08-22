using System;

namespace Zuul
{

    /* 
        For when I refactor this.
        Tokenizer tips
        https://codereview.stackexchange.com/a/129678
     */
    
    public class Parser
    {
        private CommandWords _commands; // holds all valid command words

        public Parser()
        {
            _commands = new CommandWords();
        }

        public void ShowCommands()
        {
            _commands.ShowAll();
        }

        public Command GetCommand()
        {
            string phrase = ""; // will hold the full input line
            string command = null;
            string arg0 = null;

            Console.Write("> "); // print prompt

            // get input
            phrase = Console.ReadLine();

            // split it up into words
            // split by spaces
            string[] words = phrase.Split(" ");

            if (words.Length > 0) {
                command = words[0];

                if (words.Length > 1) {
                    arg0 = words[1];
                }
            }

            // note: we just ignore the rest of the input line.

            // now check whether this word is known. If so, create a command
            // with it. If not, create a "null" command (for unknown command).
            if (_commands.IsCommandValid(command))
            {
                return new Command(command, arg0);
            }
            else
            {
                return new Command(null, arg0);
            }
        }
    }
}