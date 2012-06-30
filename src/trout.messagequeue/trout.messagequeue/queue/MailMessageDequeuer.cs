using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trout.emailservice.config;
using trout.emailservice.infrastrucure;
using trout.emailservice.model;
using trout.emailservice.queue.filters;
using trout.emailservice.queue.overrides;

namespace trout.emailservice.queue
{
    public class MailMessageDequeuer
    {
        private readonly IMailMessageSenderConfig Config;
        private readonly ISmtpClient SmtpClient;
        private readonly IEmailQueueDbContext Context;

        public MailMessageDequeuer(IMailMessageSenderConfig config, ISmtpClient smtpClient, IEmailQueueDbContext context)
        {
            Config = config;
            Context = context;
            SmtpClient = smtpClient;
        }

        public IEnumerable<DequeueResultItem> SendQueuedMessages()
        {
            return this.SendQueuedMessages(new DequeueFilterList(), new OverrideList());
        }

        public IEnumerable<DequeueResultItem> SendQueuedMessages(DequeueFilterList filters)
        {
            return this.SendQueuedMessages(filters, new OverrideList());
        }

        public IEnumerable<DequeueResultItem> SendQueuedMessages(OverrideList overrideList)
        {
            return this.SendQueuedMessages(new DequeueFilterList(), overrideList);
        }


        public IEnumerable<DequeueResultItem> SendQueuedMessages(DequeueFilterList filters, OverrideList overrides)
        {
            List<DequeueResultItem> results = new List<DequeueResultItem>();

            var messages = filters.Filter(Context);

            foreach (var message in messages.ToList())
            {
                var mailMessage = new MailMessage();
                mailMessage.From = Config.FromAddress;
                mailMessage.To.Add(message.To);
                mailMessage.CC.Add(message.Cc);
                mailMessage.Bcc.Add(message.Bcc);
                mailMessage.Subject = message.To;
                mailMessage.Body = message.Body;

                mailMessage = overrides.ApplyOverrides(mailMessage);

                var result = SmtpClient.Send(mailMessage);
                results.Add(new DequeueResultItem(message, result.IsSuccess, result.Message));

                if (result.IsSuccess)
                {
                    message.IsSent = true;
                    message.SendDate = DateTime.Now;
                }
                else
                {
                    message.IsSent = false;
                    message.SendDate = null;
                }

                message.NumberTries++;
                message.LastTryDate = DateTime.Now;
            }

            Context.SaveChanges();


            return results;
        }

        public IEnumerable<DequeueListItem> GetQueuedMessages(DequeueFilterList filters, OverrideList overrides)
        {
            List<DequeueListItem> results = new List<DequeueListItem>();

            var messages = filters.Filter(Context);

            foreach (var message in messages.ToList())
            {
                var mailMessage = new MailMessage();
                mailMessage.From = Config.FromAddress;
                mailMessage.To.Add(message.To);
                mailMessage.CC.Add(message.Cc);
                mailMessage.Bcc.Add(message.Bcc);
                mailMessage.Subject = message.To;
                mailMessage.Body = message.Body;

                mailMessage = overrides.ApplyOverrides(mailMessage);

                results.Add(new DequeueListItem(message, mailMessage));
            }

            return results;
        }
    }
}
