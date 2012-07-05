using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using trout.messagequeue.attachments;
using trout.messagequeue.model;

namespace trout.tests.messagequeue.mocks
{
    class InMemoryAttachmentFileSystem : IAttachmentFileSystem
    {
        private readonly Dictionary<EmailQueueItem, Attachment[]> Attachments = new Dictionary<EmailQueueItem, Attachment[]>();

        public void SaveAttachments(EmailQueueItem item, MailMessage mailMessage)
        {
            Attachments[item] = mailMessage.Attachments.ToArray();
        }

        public Attachment[] GetAttachments(EmailQueueItem item)
        {
            return Attachments.ContainsKey(item) ? Attachments[item] : new Attachment[0];
        }

        public void PurgeAttachments(IEnumerable<EmailQueueItem> items)
        {
            foreach (var item in items)
            {
                Attachments.Remove(item);
            }
        }
    }
}
