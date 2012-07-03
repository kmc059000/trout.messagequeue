using System;
using System.Collections.Generic;
using System.Linq;
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
        private string[] attachments = new string[0];

        public AddCommand(MailMessageQueue mailQueue)
        {
            MailQueue = mailQueue;
        }

        public override void Do(string[] args)
        {
            if (ParseArguments(args))
            {
                var random = new Random();

                List<MailMessage> messages = new List<MailMessage>(); 

                for (int i = 0; i < count; i++)
                {
                    var mailMessage = new MailMessage();
                    mailMessage.To.Add("user@example.com");
                    mailMessage.CC.Add("usercc@example.com");
                    mailMessage.Bcc.Add("userbcc@example.com");
                    mailMessage.Subject = "Test Email - " + random.Next(1000000);

                    mailMessage.Body = "Test Email Body - k6rLh1xgvX2J8IgsVkoJ";

                    if(attachments.Any())
                    {
                        AddAttachments(mailMessage);
                    }

                    messages.Add(mailMessage);
                }

                MailQueue.EnqueueMessages(messages);
            }
        }

        private void AddAttachments(MailMessage mailMessage)
        {
            foreach (var attachment in attachments)
            {
                mailMessage.Attachments.Add(new Attachment(attachment));
            }
        }

        protected override bool ParseArguments(string[] args)
        {
            OptionSet optionSet = new OptionSet()
                //filters
                .Add("c=|count=", (int v) => count = v)
                .Add("a=|attachments=|attach=", v => attachments = v.Split(','))
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