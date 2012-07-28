using System;
using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Filter for when the create date of an email is within a particular range
    /// </summary>
    public sealed class DateRangeFilter : DequeueFilter
    {
        private readonly DateTime FromDate, ToDate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        public DateRangeFilter(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.CreateDate >= FromDate && e.CreateDate <= ToDate);
        }
    }
}