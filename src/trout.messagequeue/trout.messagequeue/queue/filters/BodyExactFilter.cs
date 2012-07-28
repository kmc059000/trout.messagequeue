using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Filter for when the body is equal to a filter string
    /// </summary>
    public sealed class BodyExactFilter : DequeueFilter
    {
        private readonly string Body;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        public BodyExactFilter(string body)
        {
            Body = body;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Body == Body);
        }
    }
}