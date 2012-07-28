using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    /// <summary>
    /// Joins EmailQueueItem and a MailMessage which represent the same entity. Used during dequeue and send process.
    /// </summary>
    public sealed class DequeueListItem
    {
        /// <summary>
        /// The EmailQueueItem
        /// </summary>
        public readonly EmailQueueItem EmailQueueItem;

        /// <summary>
        /// The MailMessage
        /// </summary>
        public readonly MailMessage Message;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="emailQueueItem"></param>
        /// <param name="message"></param>
        public DequeueListItem(EmailQueueItem emailQueueItem, MailMessage message)
        {
            EmailQueueItem = emailQueueItem;
            Message = message;
        }
    }
}