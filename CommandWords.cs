using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
            try
            {
                var json = File.ReadAllText("assets/commands.json");
                dynamic results = JsonConvert.DeserializeObject(json);

                // Console.WriteLine(results.commands);
                foreach (var cmd in results.commands)
                {
                    var options = new List<string>();
                    foreach (var o in cmd.options)
                    {
                        options.Add(o.ToString());
                    }

                    ValidCommands.Add(new CommandWord {
                        Command = cmd.command.ToString(),
                        Options = options,
                        Description = cmd.description.ToString(),
                        Usage = cmd.usage.ToString()
                    });
                }
            }
            catch (IOException e)
            {
                // pokeball exception for now.
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}