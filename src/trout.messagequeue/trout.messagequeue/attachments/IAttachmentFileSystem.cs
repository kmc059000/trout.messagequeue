using System.Collections.Generic;
using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.attachments
{
    /// <summary>
    /// Abstract file system for storing attachments.
    /// </summary>
    public interface IAttachmentFileSystem
    {
        /// <summary>
        /// Saves attachments on the provided MailMessage based on values in the EmailQueueItem
        /// </summary>
        /// <param name="item">EmailQueueItem that attachments are to be saved on</param>
        /// <param name="mailMessage">MailMessage which contains attachments</param>
        void SaveAttachments(EmailQueueItem item, MailMessage mailMessage);
        
        /// <summary>
        /// Retrieves attachments for an EmailQueueItem
        /// </summary>
        /// <param name="item">The item to get attachments for</param>
        /// <returns></returns>
        Attachment[] GetAttachments(EmailQueueItem item);
        
        /// <summary>
        /// Permanently removes attachments for a set of EmailQueueItems. 
        /// </summary>
        /// <param name="items"></param>
        void PurgeAttachments(IEnumerable<EmailQueueItem> items);
    }
}