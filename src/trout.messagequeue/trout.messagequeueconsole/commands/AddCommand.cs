using System;
using System.Net.Mail;
using trout.messagequeue.queue;
using trout.messagequeue.queue.filters;
using trout.messagequeueconsole.arguments;

namespace trout.messagequeueconsole.commands
{
    class AddCommand : Command
    {
        private readonly MailMessageQueue MailQueue;
        private int count = 5;

        public AddCommand(MailMessageQueue mailQueue)
        {
            MailQueue = mailQueue;
        }

        public override void Do(string[] args)
        {
            if (ParseArguments(args))
            {
                var random = new Random();

                for (int i = 0; i < count; i++)
                {
                    var mailMessage = new MailMessage();
                    mailMessage.To.Add("user@example.com");
                    mailMessage.CC.Add("usercc@example.com");
                    mailMessage.Bcc.Add("userbcc@example.com");
                    mailMessage.Subject = "Test Email - " + random.Next(1000000);

                    mailMessage.Body = "Test Email Body - k6rLh1xgvX2J8IgsVkoJ";

                    MailQueue.EnqueueMessage(mailMessage);
                }
            }
        }

        protected override bool ParseArguments(string[] args)
        {
            OptionSet optionSet = new OptionSet()
                //filters
                .Add("c=|count=", (int v) => count = v)
                ;

            try
            {
                optionSet.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Error");
                return false;
            }

            return true;
        }
    }
}