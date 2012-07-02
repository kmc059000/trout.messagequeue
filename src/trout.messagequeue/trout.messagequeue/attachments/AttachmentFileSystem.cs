using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using trout.messagequeue.config;
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
            for (int i = 0; i < mailMessage.Attachments.Count; i++)
            {
                var attachment = mailMessage.Attachments[i];

                using(FileStream file = new FileStream(GetAttachmentFileName(item, i, attachment), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] buffer = new byte[attachment.ContentStream.Length];

                    attachment.ContentStream.Read(buffer, 0, buffer.Length);

                    file.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public Attachment[] GetAttachments(EmailQueueItem item)
        {
            List<Attachment> attachments = new List<Attachment>();

            var attachmentDirectories = Directory.EnumerateDirectories(GetAttachmentDirectory(item));

            foreach (var attachmentDirectory in attachmentDirectories)
            {
                var files = Directory.EnumerateFiles(attachmentDirectory);

                if(files.Any())
                {
                    FileInfo fi = new FileInfo(files.First());

                    using(FileStream fs = fi.OpenRead())
                    {
                        attachments.Add(new Attachment(fs, fi.Name));
                    }
                }
            }

            return attachments.ToArray();
        }

        private string GetAttachmentDirectory(EmailQueueItem item)
        {
            return string.Format("{0}\\{1}t\\{2}t\\{3}\\",
                                 Config.AttachmentPath,
                                 (item.ID/10000), //123,456,789 -> 123400
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
