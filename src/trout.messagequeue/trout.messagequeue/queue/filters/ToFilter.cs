using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue filter which returns emails which have the specified email in the To, CC, or BCC fields
    /// </summary>
    public sealed class ToFilter : DequeueFilter
    {
        private readonly string To;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="to"></param>
        public ToFilter(string to)
        {
            To = to;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.To.Contains(To) || e.Cc.Contains(To) || e.Bcc.Contains(To));
        }
    }
}