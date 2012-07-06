using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using trout.messagequeue.infrastrucure.dependencies;
using trout.messagequeue.infrastrucure.logging;
using trout.messagequeueconsole.arguments;

namespace trout.messagequeueconsole.commands
{
    class RootCommand
    {
        public void PresentRootCommandToUser(string[] args)
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
                    var strCommand = GetArguments(Console.ReadLine());

                    if (strCommand.Length > 0)
                    {
                        TroutLog.Log.Info(string.Format("executing: {0}", strCommand.Aggregate(new StringBuilder(), (current, next) => current.Append(" ").Append(next))));

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
                .Add("edit", v => command = DependencyResolver.Resolve<EditCommand>())
                .Add("attachments", v => command = DependencyResolver.Resolve<AttachmentsCommand>())
                ;

            if (args.Length > 0 && !args[0].StartsWith("-")) args[0] = "-" + args[0];

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

        private string[] GetArguments(string readLine)
        {
            //I'm sure there is a built in function to do this or a regex expression to do this but cant find it!
            List<string> arguments = new List<string>();
            string currentString = "";
            bool quoted = false;

            foreach (var character in readLine)
            {
                if (character == ' ' && !quoted)
                {
                    if (currentString != "")
                    {
                        arguments.Add(currentString);
                        currentString = "";
                    }
                }
                else if (character == '"')
                {
                    quoted = !quoted;

                    if (!quoted && currentString != "")
                    {
                        arguments.Add(currentString);
                        currentString = "";
                    }
                }
                else
                {
                    currentString += character;
                }
            }

            if (currentString != "")
                arguments.Add(currentString);

            return arguments.ToArray();
        }
    }
}
