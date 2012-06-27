using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class BodyExactFilter : DequeueFilter
    {
        private readonly string Body;

        public BodyExactFilter(string body)
        {
            Body = body;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Body == Body);
        }
    }
}