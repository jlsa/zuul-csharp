using System.Collections.Generic;

namespace Zuul
{
    public class Dialogue
    {
        public string Greeting {get; set;}
        public string Goodbye {get; set;}
        public Dictionary<string, string[]> SubjectsAndSentences {get; set;}
    }
}