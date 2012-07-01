using System;

namespace trout.emailserviceconsole.commands
{
    class HelpCommand : Command
    {
        public override void Do(string[] args)
        {
            //Console.WriteLine("add - adds 1000 randomly generated email");
            Console.WriteLine("send - sends all pending emails");
            Console.WriteLine("list - displays emails");
            Console.WriteLine("exit - exits");
        }

        protected override void ParseArguments(string[] args)
        {
            //no arguments
            throw new NotImplementedException();
        }
    }
}