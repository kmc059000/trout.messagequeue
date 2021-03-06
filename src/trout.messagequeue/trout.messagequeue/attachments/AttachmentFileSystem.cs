﻿using System;
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
    /// <summary>
    /// Built in IAttachmentFileSystem which saves attachments on disk.
    /// </summary>
    public sealed class AttachmentFileSystem : IAttachmentFileSystem
    {
        private readonly IFileSystemAttachmentHandlerConfig Config;

        /// <summary>
        /// Constructor for AttachmentFileSystem
        /// </summary>
        /// <param name="config"></param>
        public AttachmentFileSystem(IFileSystemAttachmentHandlerConfig config)
        {
            Config = config;
        }


        /// <summary>
        /// Saves attachments on the provided MailMessage based on values in the EmailQueueItem
        /// </summary>
        /// <param name="item">EmailQueueItem that attachments are to be saved on</param>
        /// <param name="mailMessage">MailMessage which contains attachments</param>
        public void SaveAttachments(EmailQueueItem item, MailMessage mailMessage)
        {
            if(item == null) throw new ArgumentNullException("item");
            if (mailMessage == null) throw new ArgumentNullException("mailMessage");

            TroutLog.Log.Info(string.Format("Saving {0} attachments for email {1}", mailMessage.Attachments.Count, item.ID));

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

            TroutLog.Log.Info(string.Format("Saved {0} attachments for email {1}", mailMessage.Attachments.Count, item.ID));
        }

        /// <summary>
        /// Retrieves attachments for an EmailQueueItem
        /// </summary>
        /// <param name="item">The item to get attachments for</param>
        /// <returns></returns>
        public Attachment[] GetAttachments(EmailQueueItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            TroutLog.Log.Info(string.Format("Retrieving attachments for email {0}", item.ID));

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

            TroutLog.Log.Info(string.Format("Retrieved {0} attachments for email {1}",attachments.Count, item.ID));

            return attachments.ToArray();
        }

        /// <summary>
        /// Permanently removes attachments for a set of EmailQueueItems.
        /// </summary>
        /// <param name="items"></param>
        public void PurgeAttachments(IEnumerable<EmailQueueItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            TroutLog.Log.Info(string.Format("Purging attachments for {0} attachments", items.Count()));

            foreach (var item in items)
            {
                PurgeAttachments(item);
            }
        }

        private void PurgeAttachments(EmailQueueItem item)
        {
            if (!Directory.Exists(GetAttachmentDirectory(item))) return;

            TroutLog.Log.Info(string.Format("Purging attachments for email {0} attachments", item.ID));

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
