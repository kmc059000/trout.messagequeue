using System.Net.Mail;
using trout.emailservice.model;

namespace trout.emailservice.queue
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