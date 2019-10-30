using System.Collections.Generic;
namespace Zuul
{
    public class Dialogue
    {
        public string StartSentence {get; set;}
        public string EndSentence {get; set;}
        public Dictionary<string, string> SubjectsAndSentences {get; set;}
    }
}