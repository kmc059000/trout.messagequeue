using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class SentStatusDequeueFilter : DequeueFilter
    {
        private readonly bool IsSent;

        public SentStatusDequeueFilter(bool isSent)
        {
            IsSent = isSent;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.IsSent == IsSent);
        }
    }
}