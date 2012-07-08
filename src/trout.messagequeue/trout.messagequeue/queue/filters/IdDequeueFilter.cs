using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public sealed class IdDequeueFilter : DequeueFilter
    {
        private readonly int[] IdFilters;

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