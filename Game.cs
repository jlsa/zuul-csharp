using System;
using System.Collections.Generic;

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
        NONE
    }

    public class Game
    {
        private Parser _parser;
        private Player _player;

        public Game () {
            _parser = new Parser();
            _player = new Player();

            // eventually pull this out for a room manager
            _createRooms();
        }

        private void _createRooms()
        {
            Room outside, theatre, pub, lab, office, cellar;

            // create rooms;
            outside = new Room("outside the main entrance of the university");
            theatre = new Room("in a lecture theatre");
            pub = new Room("in the campus pub");
            lab = new Room("in a computing lab");
            office = new Room("in the computing admin office");
            cellar = new Room("in the cellar");

            // initialise room exits
            outside.AddExit(Directions.EAST, theatre);
            outside.AddExit(Directions.SOUTH, lab);
            outside.AddExit(Directions.WEST, pub);

            theatre.AddExit(Directions.WEST, outside);

            pub.AddExit(Directions.EAST, outside);

            lab.AddExit(Directions.NORTH, outside);
            lab.AddExit(Directions.EAST, office);

 
            office.AddExit(Directions.WEST, lab);
            office.AddExit(Directions.DOWN, cellar);

            cellar.AddExit(Directions.UP, office);

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

            _player.EnterRoom(outside); // starts the player outside
        }


        public void Run()
        {
            // print welcome
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
                    _printHelp();
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
                    Console.WriteLine("You dead!");
                    wantToQuit = _quit(cmd);
                }
            }

            return wantToQuit;
        }

        private void _useItem(Command cmd)
        {
            if (!cmd.HasSecondWord())
            {
                Console.WriteLine("Use what item?");
                return;
            }

            string possibleItemName = cmd.GetSecondWord();

            for (int i = 0; i < _player.Room.Items.Count; i++)
            {
                if (_player.Room.Items[i].Name.Equals(possibleItemName))
                {
                    Item item = _player.Room.PeekItem(_player.Room.Items[i]);
                    if (item.CanBeUsed())
                    {
                        // and here it all goes to schiit..
                        // cause to use an item. it has to do something. but where and how..
                        // thats for another day. 
                        Console.WriteLine($"You used the {possibleItemName}.");
                    }
                    else
                    {
                        Console.WriteLine($"{possibleItemName} could not be used.");
                    }
                    return;
                }
            }
            
            Console.WriteLine("There was no item to take.");
            
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

        private void _printHelp()
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
                Console.WriteLine("Go where?");
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
            Room nextRoom = _player.GetCurrentRoom().GetExit(direction);

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