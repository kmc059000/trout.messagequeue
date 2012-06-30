using System;
using System.Net.Mail;
using trout.emailservice.infrastrucure.dependencies;
using trout.emailservice.model;
using trout.emailservice.queue;

namespace trout.emailserviceclient.commands
{
    class AddCommand : Command
    {
        public override void Do(string[] args)
        {
            var sender = DependencyResolver.Resolve<MailMessageQueue>();

            var random = new Random();

            for (int i = 0; i < 1000; i++)
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

        protected override void ParseArguments(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}