using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    public class DequeueResultItem
    {
        public readonly EmailQueueItem EmailQueueItem;
        public readonly bool IsSuccess;
        public readonly string Message;
        public readonly MailMessage SentMailMessage;

        public DequeueResultItem(EmailQueueItem emailQueueItem, bool isSuccess, string message, MailMessage sentMailMessage)
        {
            EmailQueueItem = emailQueueItem;
            SentMailMessage = sentMailMessage;
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}