using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class IdDequeueFilter : DequeueFilter
    {
        private readonly int[] IdFilters;

        public IdDequeueFilter(params int[] ids)
        {
            IdFilters = ids;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => IdFilters.Contains(e.ID));
        }
    }
}