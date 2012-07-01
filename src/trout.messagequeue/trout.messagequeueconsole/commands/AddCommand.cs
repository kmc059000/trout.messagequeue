using System;
using System.Net.Mail;
using trout.messagequeue.queue;

namespace trout.messagequeueconsole.commands
{
    class AddCommand : Command
    {
        private readonly MailMessageQueue MailQueue;

        public AddCommand(MailMessageQueue mailQueue)
        {
            MailQueue = mailQueue;
        }

        public override void Do(string[] args)
        {
            var random = new Random();

            for (int i = 0; i < 50; i++)
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

        protected override void ParseArguments(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}