using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue filter for items which have the number of retry attempts less than the specified value.
    /// </summary>
    public sealed class RetriesFilter : DequeueFilter
    {
        private readonly byte MaximumRetries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maximumRetries"></param>
        public RetriesFilter(byte maximumRetries)
        {
            MaximumRetries = maximumRetries;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.NumberTries < MaximumRetries);
        }
    }
}