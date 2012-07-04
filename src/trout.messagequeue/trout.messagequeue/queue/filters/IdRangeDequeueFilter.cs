using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public class IdRangeDequeueFilter : DequeueFilter
    {
        private readonly int RangeMinimum = 0;
        private readonly int RangeMaximum = int.MaxValue;
        
        /// <summary>
        /// Min and max are inclusive
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public IdRangeDequeueFilter(int minimum = 0, int maximum = int.MaxValue)
        {
            RangeMinimum = minimum;
            RangeMaximum = maximum;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.ID >= RangeMinimum && e.ID <= RangeMaximum);
        }
    }
}