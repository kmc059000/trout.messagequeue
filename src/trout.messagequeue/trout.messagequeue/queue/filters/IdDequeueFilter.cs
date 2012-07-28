using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue Filter for items which have a specific id(s)
    /// </summary>
    public sealed class IdDequeueFilter : DequeueFilter
    {
        private readonly int[] IdFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ids"></param>
        public IdDequeueFilter(params int[] ids)
        {
            IdFilters = ids;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => IdFilters.Contains(e.ID));
        }
    }
}