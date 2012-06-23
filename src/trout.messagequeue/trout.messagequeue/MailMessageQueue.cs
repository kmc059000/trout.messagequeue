using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace trout.emailservice
{
    public class MailMessageQueue
    {
        public void EnqueueMessage(MailMessage message)
        {
            using (var ctx = new EmailQueueDbContext())
            {
                ctx.EmailQueueItems.Add(new EmailQueueItem()
                                            {
                                                To = message.To.ToString(),
                                                Cc = message.CC.ToString(),
                                                Bcc = message.Bcc.ToString(),
                                                Subject = message.Subject,
                                                Body = message.Body,
                                                IsSent = false,
                                                NumberTries = 0,
                                                LastTryDate = null,
                                                SendDate = null
                                            });
                ctx.SaveChanges();
            }
        }

        public void EnqueueMessages(IEnumerable<MailMessage> messages)
        {
            using (var ctx = new EmailQueueDbContext())
            {
                foreach (var message in messages)
                {
                    ctx.EmailQueueItems.Add(new EmailQueueItem()
                    {
                        To = message.To.ToString(),
                        Cc = message.CC.ToString(),
                        Bcc = message.Bcc.ToString(),
                        Subject = message.Subject,
                        Body = message.Body,
                        IsSent = false,
                        NumberTries = 0,
                        LastTryDate = null,
                        SendDate = null
                    });
                }


                ctx.SaveChanges();
            }
        }
    }
}
