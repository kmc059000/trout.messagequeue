using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.queue
{
    public class DequeueListItem
    {
        public readonly EmailQueueItem EmailQueueItem;
        public readonly MailMessage Message;

        public DequeueListItem(EmailQueueItem emailQueueItem, MailMessage message)
        {
            EmailQueueItem = emailQueueItem;
            Message = message;
        }
    }
}