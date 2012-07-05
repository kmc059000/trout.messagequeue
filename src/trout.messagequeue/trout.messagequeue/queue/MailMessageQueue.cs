using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trout.messagequeue.attachments;
using trout.messagequeue.infrastrucure.logging;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    public class MailMessageQueue
    {
        private readonly IEmailQueueDbContext Context;
        private readonly IAttachmentFileSystem AttachmentFileSystem;

        public MailMessageQueue(IEmailQueueDbContext context, IAttachmentFileSystem attachmentFileSystem)
        {
            Context = context;
            AttachmentFileSystem = attachmentFileSystem;
        }

        public void EnqueueMessage(MailMessage message)
        {
            EnqueueMessages(new[] {message});
        }

        public void EnqueueMessages(IEnumerable<MailMessage> messages)
        {
            List<Tuple<EmailQueueItem, MailMessage>> createdMessages = new List<Tuple<EmailQueueItem, MailMessage>>();

            foreach (var message in messages)
            {
                var emailQueueItem = new EmailQueueItem()
                                         {
                                             To = message.To.ToString(),
                                             Cc = message.CC.ToString(),
                                             Bcc = message.Bcc.ToString(),
                                             Subject = message.Subject,
                                             Body = message.Body,
                                             CreateDate = DateTime.Now,
                                             IsSent = false,
                                             NumberTries = 0,
                                             LastTryDate = null,
                                             SendDate = null,
                                             IsBodyHtml = true,
                                             AttachmentCount = (byte)message.Attachments.Count
                                         };
                
                createdMessages.Add(new Tuple<EmailQueueItem, MailMessage>(emailQueueItem, message));
                
                Context.EmailQueueItemRepo.Add(emailQueueItem);
            }

            Context.SaveChanges();

            foreach (var createdMessage in createdMessages)
            {
                if (createdMessage.Item1.AttachmentCount > 0)
                {
                    AttachmentFileSystem.SaveAttachments(createdMessage.Item1, createdMessage.Item2);
                }

                TroutLog.Log.Info(string.Format("{0} was queued at {1}", createdMessage.Item1.ID, DateTime.Now.ToString()));
            }

        }
    }
}
