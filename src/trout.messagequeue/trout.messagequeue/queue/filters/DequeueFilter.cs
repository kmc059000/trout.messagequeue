using System;
using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Filter which gets applied when dequeuing
    /// </summary>
    public abstract class DequeueFilter
    {
        internal abstract IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source);
    }
}