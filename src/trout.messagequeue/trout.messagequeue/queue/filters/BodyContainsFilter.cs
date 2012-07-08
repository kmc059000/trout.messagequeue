using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public sealed class BodyContainsFilter : DequeueFilter
    {
        private readonly string Body;

        public BodyContainsFilter(string body)
        {
            Body = body;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Body.Contains(Body));
        }
    }
}