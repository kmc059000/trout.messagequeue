using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using trout.emailservice.infrastrucure.dependencies;
using trout.emailserviceclient.commands;

namespace trout.emailserviceclient
{
    class EmailServiceClient
    {
        static void Main(string[] args)
        {
            Bootstrapper.Setup();
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
                .Add("?|help", v => command = DependencyResolver.Resolve<HelpCommand>())
                .Add("add", v => command = DependencyResolver.Resolve<AddCommand>())
                .Add("send", v => command = DependencyResolver.Resolve<SendCommand>())
                .Add("exit", v => command = DependencyResolver.Resolve<ExitCommand>())
                .Add("list", v => command = DependencyResolver.Resolve<ListCommand>())
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
