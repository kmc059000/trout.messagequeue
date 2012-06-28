using System;

namespace trout.emailserviceclient.commands
{
    class ExitCommand : Command
    {
        public override void Do(string[] args)
        {
            Environment.Exit(0);
        }

        protected override void ParseArguments(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}