using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public abstract class DequeueFilter
    {
        public abstract IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source);
    }
}