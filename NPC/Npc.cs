using System;
using System.Collections.Generic;
using Zuul;
using Zuul.Enums;

namespace Zuul.Entity
{
    public class Npc
    {
        public string Name {get; set;}

        public string ShortName {get; set;}
        
        public Inventory Inventory {get; set;}

        public Gender Gender {get; set;}

        public int Age {get; set;}

        public List<string> Dialogue {get; set;}

        // way the player can interact with it
        // interaction -> leads to an action that the npc can do/say/whatever..
    }
}