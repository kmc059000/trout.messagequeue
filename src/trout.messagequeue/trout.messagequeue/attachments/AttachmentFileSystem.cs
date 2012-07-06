using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using trout.messagequeue.config;
using trout.messagequeue.infrastrucure.logging;
using trout.messagequeue.model;

namespace trout.messagequeue.attachments
{
    public sealed class AttachmentFileSystem : IAttachmentFileSystem
    {
        private readonly IMailMessageSenderConfig Config;

        public AttachmentFileSystem(IMailMessageSenderConfig config)
        {
            Config = config;
        }

        public void SaveAttachments(EmailQueueItem item, MailMessage mailMessage)
        {
            TroutLog.Log.Info(string.Format("Saving {0} attachments for email {1} at {2}", mailMessage.Attachments.Count, item.ID, DateTime.Now.ToLongDateString()));

            for (int i = 0; i < mailMessage.Attachments.Count; i++)
            {
                var attachment = mailMessage.Attachments[i];

                Directory.CreateDirectory(GetAttachmentDirectory(item, i));

                using(FileStream file = new FileStream(GetAttachmentFileName(item, i, attachment), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] buffer = new byte[attachment.ContentStream.Length];

                    attachment.ContentStream.Read(buffer, 0, buffer.Length);

                    file.Write(buffer, 0, buffer.Length);
                }
            }

            TroutLog.Log.Info(string.Format("Saved {0} attachments for email {1} at {2}", mailMessage.Attachments.Count, item.ID, DateTime.Now.ToLongDateString()));
        }

        public Attachment[] GetAttachments(EmailQueueItem item)
        {
            TroutLog.Log.Info(string.Format("Retrieving attachments for email {0} at {1}", item.ID, DateTime.Now.ToLongDateString()));

            List<Attachment> attachments = new List<Attachment>();

            if (!Directory.Exists(GetAttachmentDirectory(item))) return attachments.ToArray();

            var attachmentDirectories = Directory.EnumerateDirectories(GetAttachmentDirectory(item));

            foreach (var attachmentDirectory in attachmentDirectories)
            {
                var files = Directory.EnumerateFiles(attachmentDirectory);

                if(files.Any())
                {
                    attachments.Add(new Attachment(files.First()));
                }
            }

            TroutLog.Log.Info(string.Format("Retrieved {0} attachments for email {1} at {2}",attachments.Count, item.ID, DateTime.Now.ToLongDateString()));

            return attachments.ToArray();
        }

        public void PurgeAttachments(IEnumerable<EmailQueueItem> items)
        {
            TroutLog.Log.Info(string.Format("Purging attachments for {0} attachments {1}", items.Count(), DateTime.Now.ToLongDateString()));

            foreach (var item in items)
            {
                PurgeAttachments(item);
            }
        }

        private void PurgeAttachments(EmailQueueItem item)
        {
            if (!Directory.Exists(GetAttachmentDirectory(item))) return;

            TroutLog.Log.Info(string.Format("Purging attachments for email {0} attachments {1}", item.ID, DateTime.Now.ToLongDateString()));

            Directory.Delete(GetAttachmentDirectory(item), true);
        }

        private string GetAttachmentDirectory(EmailQueueItem item)
        {
            return string.Format("{0}\\{1}t\\{2}t\\{3}\\",
                                 Config.AttachmentPath,
                                 (item.ID/100000).ToString("000\0\0"), //123,456,789 -> 123400
                                 (item.ID/1000)%100, //123,456,789 -> 56
                                 item.ID);
        }

        private string GetAttachmentDirectory(EmailQueueItem item, int attachmentIndex)
        {
            return string.Format("{0}\\{1}a\\",
                                 GetAttachmentDirectory(item),
                                 attachmentIndex);
        }

        private string GetAttachmentFileName(EmailQueueItem item, int attachmentIndex, Attachment attachment)
        {
            return string.Format("{0}\\{1}",
                                 GetAttachmentDirectory(item, attachmentIndex),
                                 attachment.Name);
        }
    }
}
