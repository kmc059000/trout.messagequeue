using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class BodyContainsFilter : DequeueFilter
    {
        private readonly string Body;

        public BodyContainsFilter(string body)
        {
            Body = body;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Body.Contains(Body));
        }
    }
}