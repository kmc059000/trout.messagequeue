using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public sealed class ToFilter : DequeueFilter
    {
        private readonly string To;

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