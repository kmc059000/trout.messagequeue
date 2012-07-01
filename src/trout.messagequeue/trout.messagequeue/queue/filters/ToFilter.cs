using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public class ToFilter : DequeueFilter
    {
        private readonly string To;

        public ToFilter(string to)
        {
            To = to;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.To.Contains(To) || e.Cc.Contains(To) || e.Bcc.Contains(To));
        }
    }
}