using System;
using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public abstract class DequeueFilter
    {
        public abstract IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source);
    }
}