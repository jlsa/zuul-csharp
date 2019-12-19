using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

using Zuul.Enums;

namespace Zuul
{
    public class World
    {
        public List<Room> Rooms = new List<Room>();
        
        public void CreateRooms()
        {   
            try {
                var json = File.ReadAllText("assets/world.json");
                dynamic results = JsonConvert.DeserializeObject(json);
                //Console.WriteLine(results.rooms[0].name);
                foreach (var room in results.rooms)
                {
                    var roomObj = new Room(room.description.ToString());
                    roomObj.Name = room.name.ToString();
                    foreach (var npc in room.npcs)
                    {
                        var npcObj = new Zuul.Entity.Npc {
                            Name = npc.name,
                            Inventory = new Inventory(npc.inventory.Count),
                            Gender = (Gender) Enum.Parse(typeof(Gender), npc.gender.ToString()),
                            Age = npc.age
                        };
                        roomObj.AddNpc(npcObj);
                    }
                    
                    Console.WriteLine();
                    Console.WriteLine($"> {room.name}");
                    Console.WriteLine($"> {room.description}");
                    Console.WriteLine($"> {room.exits.Count} exits available.");
                    Console.WriteLine($"> {room.items.Count} items available.");
                    Console.WriteLine($"> {room.npcs.Count} npcs in this room.");
                    
                    foreach (var npc in roomObj.Npcs)
                    {
                        Console.WriteLine($"> npc: {npc.Name}");
                    }
                    
                }
            } 
            catch (IOException e)
            {
                // pokeball exception for now.
                Console.WriteLine(e.StackTrace);
            }
            
            // create rooms from json file
        }

    }
}