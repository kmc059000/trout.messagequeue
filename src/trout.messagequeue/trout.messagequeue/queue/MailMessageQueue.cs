using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trout.messagequeue.attachments;
using trout.messagequeue.infrastrucure.logging;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    /// <summary>
    /// Queue for mail messages which adds emails to the queue to be sent at a later date.
    /// </summary>
    public sealed class MailMessageQueue
    {
        private readonly IEmailQueueDbContext Context;
        private readonly IAttachmentFileSystem AttachmentFileSystem;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attachmentFileSystem"></param>
        public MailMessageQueue(IEmailQueueDbContext context, IAttachmentFileSystem attachmentFileSystem)
        {
            Context = context;
            AttachmentFileSystem = attachmentFileSystem;
        }

        /// <summary>
        /// Enqueues a single MailMessage to be sent
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueMessage(MailMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            EnqueueMessages(new[] {message});
        }

        /// <summary>
        /// Enqueues a set of MailMessages to be sent
        /// </summary>
        /// <param name="messages"></param>
        public void EnqueueMessages(IEnumerable<MailMessage> messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");

            TroutLog.Log.Info(string.Format("Beginning queuing of {0} messages", messages.Count()));

            List<Tuple<EmailQueueItem, MailMessage>> createdMessages = new List<Tuple<EmailQueueItem, MailMessage>>();

            foreach (var message in messages.Where(m => m != null))
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

                TroutLog.Log.Info(string.Format("{0} was queued", createdMessage.Item1.ID));
            }

        }
    }
}
