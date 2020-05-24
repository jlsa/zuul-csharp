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
        public static World Instance = new World();
        public List<Room> Rooms = new List<Room>();
        
        public void Create()
        {
            string json = _read();
            dynamic results = JsonConvert.DeserializeObject(json);
            _createRooms(results);
        }

        private string _read()
        {
            try {
                var json = File.ReadAllText("assets/world.json");
                return json;
            } 
            catch (IOException e)
            {
                // pokeball exception for now.
                Console.WriteLine(e.StackTrace);
                throw new Exception("Failed to read world file!");
            }
        }

        private void _createRooms(dynamic results)
        {
            List<Room> rooms = new List<Room>();

            foreach (var room in results.rooms)
            {
                var roomObj = new Room(room.description.ToString());
                roomObj.Name = room.name.ToString();
                roomObj.Start = false;
                if (room.start != null) {
                    roomObj.Start = room.start.ToObject<bool>();
                }
                
                rooms.Add(roomObj);
            }

            foreach (var resRoom in results.rooms) {
                Room room = rooms.Where(r => r.Name.Equals(resRoom.name.ToString())).FirstOrDefault();
                if (room != null) {
                    foreach (var npc in resRoom.npcs) {
                        Dictionary<string, string[]> subjectsAndSentences = new Dictionary<string, string[]>();
                        
                        foreach (var sns in npc.dialogue.subjectsAndSentences)
                        {
                            string topic = sns.subject;
                            string[] sentences = sns.sentences.ToObject<string[]>();
                            subjectsAndSentences.Add(topic, sentences);
                        }

                        Dialogue dialogue = new Dialogue {
                            Greeting = npc.dialogue.startSentence,
                            Goodbye = npc.dialogue.endSentence,
                            SubjectsAndSentences = subjectsAndSentences
                        };

                        room.AddNpc(new Zuul.Entity.Npc {
                            Name = npc.name,
                            Inventory = new Inventory(npc.inventory.Count),
                            Gender = (Gender) Enum.Parse(typeof(Gender), npc.gender.ToString()),
                            Age = npc.age,
                            ShortName = npc.shortname,
                            Dialogue = dialogue
                        });
                    }

                    foreach (var item in resRoom.items)
                    {
                        // Console.WriteLine($"itemstats: {item.stats}");
                        ItemStats itemStats = new ItemStats
                        {
                            Weight = item.stats.Weight,
                            Strength = item.stats.Strength,
                            Agility = item.stats.Agility,
                            HealthPoints = item.stats.HealthPoints
                        };
                        room.AddItem(new Zuul.Item(
                            item.name.ToString(),
                            item.description.ToString(),
                            (ItemType) Enum.Parse(typeof(ItemType), item.type.ToString())
                        ));
                    }

                    foreach (var exit in resRoom.exits)
                    {
                        var ro = rooms
                            .Where(r => r.Name.Equals(exit.room.ToString()))
                            .FirstOrDefault();

                        if (ro != null)
                        {
                            room.AddExit(new Exit {
                                Direction = (Directions) Enum.Parse(typeof(Directions), exit.direction.ToString()),
                                Room = ro,
                                Locked = exit.locked
                            });
                        }
                    }

                    foreach (var monster in resRoom.monsters)
                    {
                        Dictionary<string, string[]> subjectsAndSentences = new Dictionary<string, string[]>();
                        
                        foreach (var sns in monster.dialogue.subjectsAndSentences)
                        {
                            string topic = sns.subject;
                            string[] sentences = sns.sentences.ToObject<string[]>();
                            subjectsAndSentences.Add(topic, sentences);
                        }

                        Dialogue dialogue = new Dialogue {
                            Greeting = monster.dialogue.startSentence,
                            Goodbye = monster.dialogue.endSentence,
                            SubjectsAndSentences = subjectsAndSentences
                        };

                        room.AddMonster(new Zuul.Entity.Monster {
                            Name = monster.name,
                            Inventory = new Inventory(monster.inventory.Count),
                            ShortName = monster.shortName,
                            Dialogue = dialogue,
                            Stats = new Entity.MonsterStats {
                                InventorySize = monster.inventory.Count,
                                BaseHealthPoints = monster.stats.baseHealthPoints,
                                HealthPoints = monster.stats.healthPoints,
                                Strength = monster.stats.strength,
                                Intellect = monster.stats.intellect,
                                Agility = monster.stats.agility,
                                Sight = monster.stats.sight
                            }
                        });
                    }
                }
            }

            Rooms = rooms;
        }
    }
}