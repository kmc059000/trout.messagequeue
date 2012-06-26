using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace trout.emailservice
{
    public class EmailQueueSender
    {
        private readonly IMailMessageSenderConfig Config;
        private readonly ISmtpClient SmtpClient;

        public EmailQueueSender(IMailMessageSenderConfig config, ISmtpClient smtpClient)
        {
            Config = config;
            SmtpClient = smtpClient;
        }

        public IEnumerable<DequeueResultItem> SendQueuedMessages()
        {
            List<DequeueResultItem> results = new List<DequeueResultItem>();

            using (var ctx = new EmailQueueDbContext())
            {
                var messages = from e in ctx.EmailQueueItems
                               where e.IsSent == false && e.NumberTries < Config.MaxTries
                               select e;

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

                ctx.SaveChanges();
            }

            return results;
        }
    }
}
