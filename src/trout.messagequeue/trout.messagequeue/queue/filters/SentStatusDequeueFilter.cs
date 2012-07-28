using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue filter which filters on whether an email has been sent or not
    /// </summary>
    public sealed class SentStatusDequeueFilter : DequeueFilter
    {
        private readonly bool IsSent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isSent"></param>
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