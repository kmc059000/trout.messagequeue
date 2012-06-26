﻿namespace trout.emailservice
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