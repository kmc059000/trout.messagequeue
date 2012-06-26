using trout.emailservice.model;

namespace trout.emailservice.queue
{
    public class DequeueResultItem
    {
        public readonly EmailQueueItem EmailQueueItem;
        public readonly bool IsSuccess;
        public readonly string Message;

        public DequeueResultItem(EmailQueueItem emailQueueItem, bool isSuccess, string message)
        {
            EmailQueueItem = emailQueueItem;
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}