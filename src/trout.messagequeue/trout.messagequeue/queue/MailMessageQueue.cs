using System;
using System.Collections.Generic;
using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
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
            Context.EmailQueueItemRepo.Add(new EmailQueueItem()
                            {
                                To = message.To.ToString(),
                                Cc = message.CC.ToString(),
                                Bcc = message.Bcc.ToString(),
                                Subject = message.Subject,
                                Body = message.Body,
                                IsSent = false,
                                CreateDate = DateTime.Now,
                                NumberTries = 0,
                                LastTryDate = null,
                                SendDate = null,
                                IsBodyHtml = true
                            });

            Context.SaveChanges();
        }

        public void EnqueueMessages(IEnumerable<MailMessage> messages)
        {
            foreach (var message in messages)
            {
                Context.EmailQueueItemRepo.Add(new EmailQueueItem()
                                                   {
                                                       To = message.To.ToString(),
                                                       Cc = message.CC.ToString(),
                                                       Bcc = message.Bcc.ToString(),
                                                       Subject = message.Subject,
                                                       Body = message.Body,
                                                       IsSent = false,
                                                       NumberTries = 0,
                                                       LastTryDate = null,
                                                       SendDate = null,
                                                       IsBodyHtml = true
                                                   });
            }

            Context.SaveChanges();
        }
    }
}
