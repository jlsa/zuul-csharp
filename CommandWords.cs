using System;
using System.Collections.Generic;
using System.Linq;

namespace Zuul
{
    /*
        Refactor this to use JSON based commands.
     */
    public class CommandWord
    {
        public string Command {get; set;}
        public List<string> Options {get; set;}
        public string Description {get; set;}
        public string Usage {get; set;}
    }

    public class CommandWords
    {
        public List<CommandWord> ValidCommands = new List<CommandWord>();
        // private string[] _validCommands = new string[] {
        //     "go",
        //     "quit",
        //     "help",
        //     "take",
        //     "use",
        //     "inventory",
        //     "unlock",
        //     "look",
        //     // "hit",
        //     // "drop",
        //     // "inspect",
        //     // "lock",
        // };

        public CommandWords()
        {
            _init();
        }

        public string GetRootCommand(string cmd)
        {
            return ValidCommands.Where(vc => vc.Options.Contains(cmd)).SingleOrDefault().Command;
        }
        
        public bool IsCommandValid(string cmd)
        {
            return ValidCommands.Where(vc => vc.Options.Contains(cmd)).SingleOrDefault() != null;
            // for (int i = 0; i < _validCommands.Length; i++)
            // {
            //     if (_validCommands[i].Equals(cmd))
            //     {
            //         return true;
            //     }
            // }
            // return false;
        }

        public void ShowAll()
        {
            foreach (CommandWord cmdWord in ValidCommands)
            {
                foreach (string option in cmdWord.Options)
                {
                    Console.Write($"\"{option}\", ");
                }
            }
            // write the usage examples for each command
            // example: go east(e/E) | west(w/W) | north (n/N) | south (s/S)
            
            Console.WriteLine();
        }

        public void Show(string cmd)
        {
            var command = ValidCommands.Where(vc => vc.Options.Contains(cmd)).SingleOrDefault();
            if (command == null)
            {
                Console.WriteLine("No command was found.");
            }

            Console.WriteLine($">> {command.Command}");
            Console.WriteLine($"> {command.Description}");
            Console.WriteLine($"---------------------------");
            Console.WriteLine($"> {command.Usage}");
        }

        private void _init()
        {
            ValidCommands.Add(new CommandWord {
                Command = "go",
                Options = new List<string> {"go", "walk", "move", "advance"},
                Description = "Enables you to move from place to place.",
                Usage = "\"go east\", \"climb up\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "inventory",
                Options = new List<string> {"inventory", "inv", "backpack"},
                Description = "Lists the items in your inventory.",
                Usage = "\"inventory\", \"inv\", \"backpack\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "unlock",
                Options = new List<string> {"unlock"},
                Description = "Used to unlock an exit in a given direction",
                Usage = "\"unlock east\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "look",
                Options = new List<string> {"look"},
                Description = "Lets you look around the room.",
                Usage = "\"look\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "help",
                Options = new List<string> {"list", "help"},
                Description = "Lists all commands and their usages",
                Usage = "\"help\", \"list\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "quit",
                Options = new List<string> {"quit", "stop", "runaway"},
                Description = "Hides you forever in the darkness. Unable to escape.",
                Usage = "\"stop\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "take",
                Options = new List<string> {"take", "pickup", "grab"},
                Description = "Pick up, take or grab an item.",
                Usage = "\"pickup item\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "use",
                Options = new List<string> {"use"},
                Description = "Use an item you hold in your inventory",
                Usage = "\"use item\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "attack",
                Options = new List<string> {"attack", "hit", "strike"},
                Description = "When holding a sword strike out to a monster",
                Usage = "\"attack 'creature'\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "cast",
                Options = new List<string> {"cast"},
                Description = "Cast a spell",
                Usage = "\"cast 'spell'\""
            });

            ValidCommands.Add(new CommandWord {
                Command = "talk",
                Options = new List<string> {"talk", "conversation"},
                Description = "Talk to a NPC",
                Usage = "\"talk 'npc name'\""
            });
        }
    }
}