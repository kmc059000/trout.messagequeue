using System.Collections.Generic;
using System.Net.Mail;
using trout.emailservice.model;

namespace trout.emailservice.queue
{
    public class MailMessageQueue
    {
        private readonly IEmailQueueDbContext Context;

        public MailMessageQueue(IEmailQueueDbContext context)
        {
            Context = context;
        }

        public void EnqueueMessage(MailMessage message)
        {
            Context.Add(new EmailQueueItem()
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
            Context.SaveChanges();
        }

        public void EnqueueMessages(IEnumerable<MailMessage> messages)
        {
            foreach (var message in messages)
            {
                Context.Add(new EmailQueueItem()
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


            Context.SaveChanges();
        }
    }
}
