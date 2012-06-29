using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using trout.emailserviceclient.commands;

namespace trout.emailserviceclient
{
    class EmailServiceClient
    {
        static void Main(string[] args)
        {
            new EmailServiceClient().Start(args);
        }

        public void Start(string[] args)
        {
            if (args.Length > 0)
            {
                ExecuteCommand(args);
            }
            else
            {
                while (true)
                {
                    Console.Write("> ");
                    var strCommand = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (strCommand.Length > 0)
                    {
                        ExecuteCommand(strCommand);
                    }
                }
            }
        }

        private void ExecuteCommand(string[] args)
        {
            Command command = null;
            List<string> remainingArgs = null;

            OptionSet optionSet = new OptionSet()
                .Add("?|help", v => command = new HelpCommand())
                .Add("add", v => command = new AddCommand())
                .Add("send", v => command = new SendCommand())
                .Add("exit", v => command = new ExitCommand())
                .Add("list", v => command = new ListCommand())
                ;

            try
            {
                remainingArgs = optionSet.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Error");
            }

            if (command != null && remainingArgs != null)
            {
                command.Do(remainingArgs.ToArray());
            }

        }
    }
}
