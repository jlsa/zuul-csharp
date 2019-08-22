namespace Zuul
{
    /*
        Needs work.. I dont like how it works with command word and second command
        word. It is fidgety.. I want more options for commands.
     */
    public class Command
    {
        private string _commandWord;
        private string _secondCommandWord;
    
        /**
         * Create a command object. First and second word must be supplied, but
         * either one (or both) can be null. The command word should be null to
         * indicate that this was a command that is not recognized by this game.
         */
        public Command(string firstWord, string secondWord)
        {
            _commandWord = firstWord;
            _secondCommandWord = secondWord;
        }

        /*
            Return the command word (the first word) of this command. If the
            command was not understoor, the result is null.
         */
        public string GetCommandWord()
        {
            return _commandWord;
        }

        /*
            Returns the second word of this command. Returns null if there was no
            second word.
         */
        public string GetSecondWord()
        {
            return _secondCommandWord;
        }

        public bool IsUnknown() // not known exception is thrown?
        {
            return _commandWord == null;
        }

        public bool HasSecondWord()
        {
            return _secondCommandWord != null;
        }
    }
}