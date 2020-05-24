using System;
using System.Collections.Generic;
using Zuul;
using Zuul.Enums;

namespace Zuul.Entity
{
    public class MonsterStats
    {
        public int InventorySize { get; set; }
        public int BaseHealthPoints { get; set; }
        public int HealthPoints { get; set; }
        public int Strength { get; set; }
        public int Intellect { get; set; }
        public int Agility { get; set; }
        public int Sight { get; set; }
    }
    
    public class Monster
    {
        public string Name { get; set; }

        public string ShortName { get; set; }
        
        public Inventory Inventory { get; set; }

        public Dialogue Dialogue { get; set; }

        public MonsterStats Stats { get; set; }

        private Random _random = new Random();

        public void Attack(Zuul.Player player)
        {
            player.Hurt(Stats.Strength);
            Console.WriteLine($"{Name} deals {Stats.Strength} damage to you.");
            // speak fight sentence
            int index = _random.Next(Dialogue.SubjectsAndSentences["fight"].Length);
            Console.WriteLine($"{Name}: [yell] {Dialogue.SubjectsAndSentences["fight"][index]}");
        }

        public void Hurt(int damage)
        {
            _doDamage(damage);
            Console.WriteLine($"You deal {damage} damage to {Name}.");
            if (Stats.HealthPoints > 0)
            {
                int index = _random.Next(Dialogue.SubjectsAndSentences["hurt"].Length);
                Console.WriteLine($"{Name}: [hurt] {Dialogue.SubjectsAndSentences["hurt"][index]}");
            }
            else
            {
                Console.WriteLine($"{Name}: [death] {Dialogue.Goodbye}");
            }
        }

        private void _doDamage(int amount)
        {
            Stats.HealthPoints -= amount;
            if (Stats.HealthPoints <= 0)
            {
                Stats.HealthPoints = 0;
            }
        }

        public bool IsAlive()
        {
            return Stats.HealthPoints > 0;
        }
    }
}