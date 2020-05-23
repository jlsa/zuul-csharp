using System;
using System.Linq;
using System.Collections.Generic;
using Zuul.Enums;
using Zuul.Commands;

namespace Zuul
{
    public class Game
    {
        private Parser _parser;
        private Player _player;
        private Help _help;

        public Game () {
            _parser = new Parser();
            _help = new Help(_parser);
            _player = new Player("John Doe");

            World.Instance.Create();
            _player.EnterRoom(World.Instance.Rooms
                .Where(r => r.Start)
                .Single());
        }

        public void Run()
        {
            _printWelcome(); // add this to an output manager

            bool finished = false;
            while (!finished) {
                finished = _processCommand(_parser.GetCommand());
            }
            Console.WriteLine("Thank you for playing. Good bye.");
        }

        private void _printWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is an old but amazing and incredible text based adventure game.");
            Console.WriteLine("Type 'help' if you need help.");
            Console.WriteLine();
            Console.WriteLine(_player.Room.LongDescription);
        }

        private bool _processCommand(Command cmd)
        {
            bool wantToQuit = false;

            if (cmd.IsUnknown())
            {
                Console.WriteLine("I don't know what you mean... be more specific.");
                Console.WriteLine("Use the 'help' command to see which commands you can use.");
                return false;
            }

            if (_player.IsAlive())
            {
                // this should be handled at one location...
                string commandWord = cmd.GetCommandWord();
                
                if (commandWord.Equals("help"))
                {
                    Zuul.Commands.HelpCommand command = new HelpCommand(_help);
                    command.Execute();
                }
                else if (commandWord.Equals("go"))
                {
                    _goRoom(cmd);
                } 
                else if (commandWord.Equals("take"))
                {
                    _takeItem(cmd);
                }
                else if (commandWord.Equals("use"))
                {
                    _useItem(cmd);
                }
                else if (commandWord.Equals("quit"))
                {
                    Console.WriteLine("You died.. poor you. Cya soon..!");
                    wantToQuit = _quit(cmd);
                }
                else if (commandWord.Equals("inventory"))
                {
                    _showInventory(cmd);
                }
                else if (commandWord.Equals("unlock"))
                {
                    _unlockRoom(cmd);
                }
                else if (commandWord.Equals("look"))
                {
                    _lookInRoom(cmd);
                }
                else if (commandWord.Equals("talk"))
                {
                    _talkToNpc(cmd);
                }
                else if (commandWord.Equals("ask"))
                {
                    _askNpc(cmd);
                } else if (commandWord.Equals("bye"))
                {
                    _byeNpc(cmd);
                }
            }

            return wantToQuit;
        }

        private string _getGenderHimHerIt(Gender gender)
        {
            switch(gender)
            {
                case Gender.MALE:
                    return "him";
                case Gender.FEMALE:
                    return "her";
                case Gender.OTHER:
                default:
                    return "it";
            }
        }

        private string _getGenderHeSheThey(Gender gender)
        {
            switch(gender)
            {
                case Gender.MALE:
                    return "he";
                case Gender.FEMALE:
                    return "she";
                case Gender.OTHER:
                default:
                    return "they";
            }
        }

        private void _byeNpc(Command cmd)
        {
            if (_player.Npc != null)
            {
                Console.WriteLine($"{_player.Npc.ShortName}: {_player.Npc.Dialogue.Goodbye}");
                _player.Npc = null;
            }
            else
            {
                Console.WriteLine("You say goodbye out loud. No-one hears you. You feel strange, but it doesn't matter.");
            }
        }

        private void _askNpc(Command cmd)
        {
            if (_player.Npc == null)
            {
                Console.WriteLine("You should probably first 'talk' to an NPC");
                // get all NPC's in this room
                // print ("Maybe bob, viv, or lala would be interested to have a conversation with you");
                return;
            }

            var npc = _player.Npc;
            if (!cmd.HasSecondWord())
            {
                Console.WriteLine($"You just stare at {npc.ShortName}. Mouth open, only an uh sound escaping it.");
                Console.WriteLine($"{npc.ShortName} looks awkward at you, wondering why {_getGenderHeSheThey(npc.Gender)} greeted you in the first place.");

                Console.WriteLine("Maybe you would like to strike up a conversation about:");
                foreach (var subjectsAndSentences in npc.Dialogue.SubjectsAndSentences)
                {
                    Console.WriteLine($" - {subjectsAndSentences.Key}");
                }
                return;
            }

            string subject = cmd.GetSecondWord();
            if (npc.Dialogue.SubjectsAndSentences[subject] != null)
            {
                foreach (var sentence in npc.Dialogue.SubjectsAndSentences[subject])
                {
                    Console.WriteLine($"{npc.ShortName}: {sentence}");
                }
            }
            else
            {
                Console.WriteLine($"{npc.ShortName}: I'm sorry. I have no idea what you're talking about.");
            }
        }

        private void _talkToNpc(Command cmd)
        {
            string npcName = cmd.GetSecondWord();

            var npc = _player.GetCurrentRoom().GetNpc(npcName);
            if (npc != null)
            {
                if (_player.Npc != null || _player.Npc != npc)
                {
                    // Console.WriteLine($"You are now talking with {npc.Name}");
                    Console.WriteLine($"{npc.ShortName}: {npc.Dialogue.Greeting}");
                    Console.WriteLine($"Maybe you would like to ask {npc.Name} about:");
                    foreach (var subjectsAndSentences in npc.Dialogue.SubjectsAndSentences)
                    {
                        Console.WriteLine($" - {subjectsAndSentences.Key}");
                    }
                    _player.ChatToNpc(npc);
                }
                else
                {
                    // Console.WriteLine($"You are still in a conversation with {npc.ShortName}");
                }
                

                // Console.WriteLine($"What do you want to ask {_getGenderHimHerIt(npc.Gender)}?");
            }
            else
            {
                Console.WriteLine("No NPC found with that name");
            }
        }

        private void _lookInRoom(Command cmd)
        {
            Console.WriteLine(_player.Room.LongDescription);
        }

        private void _unlockRoom(Command cmd)
        {
            if (!cmd.HasSecondWord())
            {
                Console.WriteLine("I wonder which direction you want to unlock.. Hmm?!");
                return;
            }

            Directions direction = _getDirectionFromString(cmd.GetSecondWord());
            
            if (_player.Inventory.HasItem("key"))
            {
                Item key = _player.Inventory.Take("key");
                if (key.CanBeUsed())
                {
                    Exit exit = _player.Room.GetExit(direction);
                    if (exit.Locked)
                    {
                        exit.Unlock(key);
                        Console.WriteLine($"You've successfully unlocked the door to {direction.ToString().ToLower()}");
                        key.Use();
                    }
                }
                if (key.CanBeUsed()) {
                    _player.Inventory.Add(key);
                }
            }
        }

        private void _useItem(Command cmd)
        {
            if (!cmd.HasSecondWord())
            {
                Console.WriteLine("Use what item?");
                return;
            }

            string possibleItemName = cmd.GetSecondWord();
            // check if the item is in the inventory or in the room
            // lets always first check the inventory then the room.

            if (_player.Inventory.HasItem(possibleItemName))
            {
                Item item = _player.Inventory.Take(possibleItemName);
                item.Use();
                if (item.CanBeUsed() || item.CanBeTaken()) {
                    _player.Inventory.Add(item);
                }
            }
        }

        private void _showInventory(Command cmd)
        {
            Console.WriteLine(_player.Inventory.LongDescription());
        }

        private void _takeItem(Command cmd)
        {
            if (!cmd.HasSecondWord())
            {
                Console.WriteLine("What?");
                return;
            }

            string possibleItemName = cmd.GetSecondWord();

            for (int i = 0; i < _player.Room.Items.Count; i++)
            {
                if (_player.Room.Items[i].Name.Equals(possibleItemName))
                {
                    Item item = _player.Room.GetItem(_player.Room.Items[i]);
                    if (item.CanBeTaken())
                    {
                        _player.Inventory.Add(item);
                        Console.WriteLine($"You took the {possibleItemName}.");
                    }
                    else
                    {
                        Console.WriteLine($"{possibleItemName} could not be taken along. "
                            + "Perhaps try another use for it.");
                    }
                    return;
                }
            }

            Console.WriteLine("There was no item to take.");
        }

        private void _printHelp(Command cmd)
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around at the university.");
            Console.WriteLine();
            Console.WriteLine("Your command words are: ");
            _parser.ShowCommands();
        }

        private void _goRoom(Command cmd)
        {
            if (!cmd.HasSecondWord())
            {
                // if there is no second word, we don't know where to go..
                Console.WriteLine("Go where? I wander.. in circles... ...");
                return;
            }

            // this should be checked if it is an unrecognized param or not.
            string secondWord = cmd.GetSecondWord();

            Directions direction = _getDirectionFromString(secondWord);

            if (direction == Directions.NONE)
            {
                Console.WriteLine("Only the directions (N/E/S/W/U/D) exists. Try again.");
                return;
            }

            // try to leave current room.
            Room nextRoom = _player.GetCurrentRoom().GetExit(direction)?.Room;
            Exit nextExit = _player.GetCurrentRoom().GetExit(direction);
            if (nextExit.Locked) 
            {
                // add auto unlock door here if player has key
                Console.WriteLine($"Door to the {nextRoom.Name} is locked. You need a key to open it.");
                return;
            }

            // if (_player.CanMoveInDirection(direction)) // this is better...
            if (nextRoom == null) {
                Console.WriteLine($"{_player.GetCurrentRoom().ShortDescription}");
                Console.WriteLine("There is no door!");
            }
            else
            {
                // int currentHealth = _player.Health;
                // _player.DoDamage(1);

                _player.EnterRoom(nextRoom);
                Console.WriteLine(_player.Room.LongDescription);
                // Console.WriteLine("Health: " + _player.Health); // should be in player long description
                Console.WriteLine(_player.LongDescription);
            }
        }

        private bool _quit(Command cmd)
        {
            if (cmd.HasSecondWord())
            {
                Console.WriteLine("Quit what?");
                return false;
            }

            return true;
        }

        private Directions _getDirectionFromString(string aString)
        {
            aString = aString.ToLower();
            Directions dir = Directions.NONE;

            switch (aString)
            {
                case "n":
                case "north":
                    dir = Directions.NORTH;
                    break;
                
                case "e":
                case "east":
                    dir = Directions.EAST;
                    break;

                case "s":
                case "south":
                    dir = Directions.SOUTH;
                    break;
                
                case "w":
                case "west":
                    dir = Directions.WEST;
                    break;

                case "d":
                case "down":
                    dir = Directions.DOWN;
                    break;
                
                case "u":
                case "up":
                    dir = Directions.UP;
                    break;

                case "ne":
                case "northeast":
                    dir = Directions.NORTHEAST;
                    break;

                case "se":
                case "southeast":
                    dir = Directions.SOUTHEAST;
                    break;

                case "nw":
                case "northwest":
                    dir = Directions.NORTHWEST;
                    break;
                
                case "sw":
                case "southwest":
                    dir = Directions.SOUTHWEST;
                    break;

                default:
                    break;
            }
            // throw exception if direction could not be obtained from given string.
            return dir;
        }
        
    }
}