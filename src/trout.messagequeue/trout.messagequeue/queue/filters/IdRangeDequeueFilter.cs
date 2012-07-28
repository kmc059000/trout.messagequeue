using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue filter for emails that have an ID within a specified range. Min and max values are inclusive
    /// </summary>
    public sealed class IdRangeDequeueFilter : DequeueFilter
    {
        private readonly int RangeMinimum = 0;
        private readonly int RangeMaximum = int.MaxValue;
        
        /// <summary>
        /// Constructor. Min and max values are inclusive.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public IdRangeDequeueFilter(int minimum = 0, int maximum = int.MaxValue)
        {
            RangeMinimum = minimum;
            RangeMaximum = maximum;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.ID >= RangeMinimum && e.ID <= RangeMaximum);
        }
    }
}