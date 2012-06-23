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

        public EmailQueueSender(IMailMessageSenderConfig config)
        {
            Config = config;
        }

        public void SendQueuedMessages()
        {
            using (var ctx = new EmailQueueDbContext())
            {
                var messages = from e in ctx.EmailQueueItems
                               where e.IsSent == false && e.NumberTries < Config.MaxTries
                               select e;

                SmtpClient client = new SmtpClient();

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
                        client.Send(mailMessage);
                    }
                    catch (SmtpFailedRecipientsException failedRecipientsException)
                    {
                        //should actually retry for those addresses that were failed on
                        message.IsSent = false;
                        message.SendDate = null;
                    }
                    catch (InvalidOperationException ioe)
                    {
                        //something is wrong with email
                        message.IsSent = false;
                        message.SendDate = null;
                    }
                    catch (SmtpException smtpException)
                    {
                        message.IsSent = false;
                        message.SendDate = null;
                    }
                    finally
                    {
                        message.NumberTries++;
                        message.LastTryDate = DateTime.Now;
                    }
                }

                ctx.SaveChanges();
            }
        }
    }
}
