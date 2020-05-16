using System;

namespace Zuul.Commands
{
    class HelpCommand : Command
    {
        private Help _help;
        public HelpCommand(Help help) {
            _help = help;
        }

        public override void Execute()
        {
            _help.Display();
        }
    }

    class Help
    {
        private Parser _parser;

        public Help(Parser parser)
        {
            _parser = parser;
        }

        public void Display()
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around at the university.");
            Console.WriteLine();
            Console.WriteLine("Your command words are: ");
            _parser.ShowCommands();
        }
    }
}