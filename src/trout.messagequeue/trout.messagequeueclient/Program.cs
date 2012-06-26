using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using trout.emailservice;
using SmtpClient = trout.emailservice.SmtpClient;

namespace trout.emailserviceclient
{
    class EmailServiceClient
    {
        static void Main(string[] args)
        {
            new EmailServiceClient().Start(args);
        }

        private string[] Arguments;

        private IEnumerable<Command> GetCommands()
        {
            return new[]
                       {
                           new Command("help", Help),
                           new Command("add", Add),
                           new Command("send", Send),
                           new Command("exit", Exit),
                       };
        }

        public void Start(string[] args)
        {
            var commands = GetCommands();

            if (args.Length > 0)
            {
                ExecuteCommand(args, commands);
            }
            else
            {
                while (true)
                {
                    Console.Write("> ");
                    var strCommand = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (strCommand.Length > 0)
                    {
                        ExecuteCommand(strCommand, commands);
                    }
                }
            }
        }

        private void ExecuteCommand(string[] strCommand, IEnumerable<Command> commands)
        {
            var foundCommand = commands.FirstOrDefault(c => c.Name == strCommand[0].Trim().ToLower());

            if (foundCommand != null)
            {
                this.Arguments = strCommand.Skip(1).ToArray();

                foundCommand.Do();
            }
        }



        private void Help()
        {
            Console.WriteLine("add - adds 5 randomly generated email");
            Console.WriteLine("send - sends all pending emails");
            Console.WriteLine("exit - exits");
        }

        private void Add()
        {
            var sender = new MailMessageQueue();

            var random = new Random();

            for (int i = 0; i < 5; i++)
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add("user@example.com");
                mailMessage.CC.Add("usercc@example.com");
                mailMessage.Bcc.Add("userbcc@example.com");
                mailMessage.Subject = "Test Email - " + random.Next(1000000);

                mailMessage.Body = "Test Email Body - k6rLh1xgvX2J8IgsVkoJ";

                sender.EnqueueMessage(mailMessage);
            }
        }

        private void Send()
        {
            var sender = new EmailQueueSender(new MailMessageSenderConfig(), new SmtpClient());

            sender.SendQueuedMessages();
        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
