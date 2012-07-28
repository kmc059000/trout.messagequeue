using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    /// <summary>
    /// Result of send attempt
    /// </summary>
    public sealed class DequeueResultItem
    {
        /// <summary>
        /// The EmailQueueItem
        /// </summary>
        public readonly EmailQueueItem EmailQueueItem;

        /// <summary>
        /// Whether sending was successful
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// The result of the send attempt
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// The MailMessage which was attempted to be sent
        /// </summary>
        public readonly MailMessage SentMailMessage;

        /// <summary>
        /// The number of tries that were made to send this message
        /// </summary>
        public readonly int Tries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="emailQueueItem"></param>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        /// <param name="sentMailMessage"></param>
        /// <param name="tries"></param>
        public DequeueResultItem(EmailQueueItem emailQueueItem, bool isSuccess, string message, MailMessage sentMailMessage, int tries)
        {
            EmailQueueItem = emailQueueItem;
            Tries = tries;
            SentMailMessage = sentMailMessage;
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}