using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    public sealed class DequeueResultItem
    {
        public readonly EmailQueueItem EmailQueueItem;
        public readonly bool IsSuccess;
        public readonly string Message;
        public readonly MailMessage SentMailMessage;
        public readonly int Tries;

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