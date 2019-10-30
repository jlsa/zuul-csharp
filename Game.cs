using System.Buffers;
using System;
using System.Collections.Generic;
using Zuul.Enums;

namespace Zuul
{
    public enum Directions
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
        UP,
        DOWN,
        NORTHEAST,
        SOUTHEAST,
        NORTHWEST,
        SOUTHWEST,
        NONE
    }

    public class Game
    {
        private Parser _parser;
        private Player _player;

        public Game () {
            _parser = new Parser();
            _player = new Player("John Doe");

            // eventually pull this out for a room manager
            _createRooms();
        }

        private void _createRooms()
        {
            Room outside, theatre, pub, lab, office, cellar;

            // create rooms;
            outside = new Room("outside", "outside the main entrance of the university");
            theatre = new Room("theatre", "in a lecture theatre");
            pub = new Room("pub", "in the campus pub");
            lab = new Room("lab", "in a computing lab");
            office = new Room("office", "in the computing admin office");
            cellar = new Room("cellar", "in the cellar");

            // initialise room exits
            outside.AddExit(new Exit { Direction = Directions.EAST, Room = theatre, Locked = false });
            outside.AddExit(new Exit { Direction = Directions.SOUTH, Room = lab, Locked = false });
            outside.AddExit(new Exit { Direction = Directions.WEST, Room = pub, Locked = false });

            theatre.AddExit(new Exit { Direction = Directions.WEST, Room = outside, Locked = false }); 
            // an exit should be locked.. not the room itself, as there could be more doors in that room and they are not locked.

            pub.AddExit(new Exit { Direction = Directions.EAST, Room = outside, Locked = false });

            lab.AddExit(new Exit { Direction = Directions.NORTH, Room = outside, Locked = false });
            lab.AddExit(new Exit { Direction = Directions.EAST, Room = office, Locked = false });

 
            office.AddExit(new Exit { Direction = Directions.WEST, Room = lab, Locked = false });
            office.AddExit(new Exit { Direction = Directions.DOWN, Room = cellar, Locked = true });

            cellar.AddExit(new Exit { Direction = Directions.UP, Room = office, Locked = false });

            // create items
            Item paper = new Item("paper", "a piece of paper", ItemType.PICKUP); // can be picked up and used
            Item pencil = new Item("pencil", "a broken pencil", ItemType.PICKUP); // can be picked up not yet used, only usable when sharpened
            Item sharpener = new Item("sharpener", "a pencil sharpener", ItemType.USE); // can be used but not picked up!
            Item sword = new Item("excalibur", "an enchanted flaming sword", ItemType.BOTH); // can be picked up and used
            Item key = new Item("key", "a rusty key", ItemType.BOTH);
            Item beer = new Item("beer", "a bitter tasting beverage", ItemType.USE);

            // add items to rooms
            pub.AddItem(beer);

            theatre.AddItem(key);
            
            lab.AddItem(paper);
            
            office.AddItem(pencil);
            office.AddItem(sharpener);

            cellar.AddItem(sword);

            Zuul.Entity.Npc bartender = new Zuul.Entity.Npc {
                Name = "Bartender Bob Rushcoal",
                ShortName = "Bob",
                Inventory = new Inventory(),
                Gender = Zuul.Enums.Gender.MALE,
                Age = 62,
                // dialogue should contain chat_greeting and chat_ending so it guides the player to ask the proper questions.
                Dialogue = new Dialogue() {
                    StartSentence = $"Hello {_player.Name}, welcome back! What can I do for you? All you have to do is ask.",
                    EndSentence = "",
                    SubjectsAndSentences = new Dictionary<string, string>() {
                        {"events", "Ah, you're wondering what is happening all around campus? Well, those rumors are something I tell you"},
                        {"dragon", "Well, I don't know much but there used to be dragons hiding in the library."},
                        {"rumors", "I don't know anything about any drag... on.. Really I don't."},
                        {"library", "You can find it in the other building."},
                        {"age", $"I won't tell my age."}
                    }
                }
            };
            bartender.Inventory.Add(new Item("bill", "the bill for your beer", ItemType.BROKEN));
            pub.AddNpc(bartender);

            _player.EnterRoom(outside); // starts the player outside
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
            Console.WriteLine("World of Zuul is a new, incredibly text based adventure game.");
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
                    _printHelp(cmd);
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

        private void _askNpc(Command cmd)
        {
            if (_player.Npc == null)
            {
                Console.WriteLine("You should probably first 'talk' to an NPC");
            }

            if (!cmd.HasSecondWord())
            {
                Console.WriteLine($"About what subject does {_player.Npc.ShortName} know anything?");
                return;
            }

            string subject = cmd.GetSecondWord();
            var npc = _player.Npc;
            if (npc.Dialogue.SubjectsAndSentences[subject] != null)
            {
                Console.WriteLine(npc.Dialogue.SubjectsAndSentences[subject]);
            }
            else
            {
                Console.WriteLine("I'm sorry. I have no idea what you're talking about.");
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
                    Console.WriteLine(npc.Dialogue.StartSentence);
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

            // for (int i = 0; i < _player.Room.Items.Count; i++)
            // {
            //     if (_player.Room.Items[i].Name.Equals(possibleItemName))
            //     {
            //         Item item = _player.Room.PeekItem(_player.Room.Items[i]);
            //         if (item.CanBeUsed())
            //         {
            //             // and here it all goes to schiit..
            //             // cause to use an item. it has to do something. but where and how..
            //             // thats for another day. 

            //             // only make keys work to open a closed door
            //             if (item.Name.Equals("key"))
            //             {
            //                 foreach (Exit exit in _player.Room.Exits)
            //                 {
            //                     if (exit.Locked) {
            //                         exit.Unlock(item);
            //                         Console.Write($"Door to the {exit.Direction.ToString()} is now unlocked");
            //                     }// else {
            //                     //    exit.Lock(item);
            //                     //    Console.Write($"Door to the {exit.Direction.ToString()} is now locked");
            //                     //}
            //                 }
            //             } 
            //             else if (item.Name.Equals("beer"))
            //             {
            //                 item.Disable();
            //                 Console.WriteLine($"You drank the beer now you're drunk. You already had way to much to drink.");
            //                 // _player.Decrease("Sight", 1);
            //                 // _player.Decrease("Agility", 1);
            //                 // _player.Decrease("Intellect", 2);
            //             } else 
            //             {
            //                 Console.WriteLine($"You tried using the {possibleItemName} but nothing happened to or with it.");
            //             }
            //         }
            //         else
            //         {
            //             Console.WriteLine($"{possibleItemName} could not be used.");
            //         }
            //         return;
            //     }
            // }
            // Console.WriteLine("There was no item to take.");
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

                default:
                    break;
            }
            // throw exception if direction could not be obtained from given string.
            return dir;
        }
        
    }
}