using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public sealed class SentStatusDequeueFilter : DequeueFilter
    {
        private readonly bool IsSent;

        public SentStatusDequeueFilter(bool isSent)
        {
            IsSent = isSent;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.IsSent == IsSent);
        }
    }
}