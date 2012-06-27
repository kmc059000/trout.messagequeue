using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trout.emailservice.config;
using trout.emailservice.infrastrucure;
using trout.emailservice.model;
using trout.emailservice.queue.filters;

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
            return this.SendQueuedMessages(new DequeueFilterList());
        }

        public IEnumerable<DequeueResultItem> SendQueuedMessages(DequeueFilterList filters)
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

                message.IsSent = true;
                message.SendDate = DateTime.Now;

                try
                {
                    SmtpClient.Send(mailMessage);
                    results.Add(new DequeueResultItem(message, true, "Success"));
                }
                catch (SmtpFailedRecipientsException failedRecipientsException)
                {
                    message.IsSent = false;
                    message.SendDate = null;
                    results.Add(new DequeueResultItem(message, false,
                                                      string.Format("Failed Recipient: {0} - {1}",
                                                                    failedRecipientsException.StatusCode,
                                                                    failedRecipientsException.FailedRecipient)));
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    message.IsSent = false;
                    message.SendDate = null;
                    results.Add(new DequeueResultItem(message, false, "Invalid Operation"));
                }
                catch (SmtpException smtpException)
                {
                    message.IsSent = false;
                    message.SendDate = null;
                    results.Add(new DequeueResultItem(message, false, "SMTP Exception"));
                }
                finally
                {
                    message.NumberTries++;
                    message.LastTryDate = DateTime.Now;
                }
            }

            Context.SaveChanges();


            return results;
        }

    }
}
