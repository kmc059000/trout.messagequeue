﻿using System;

namespace trout.emailserviceconsole.commands
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