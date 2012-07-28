using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Filter for when the body of the email contains a string
    /// </summary>
    public sealed class BodyContainsFilter : DequeueFilter
    {
        private readonly string Body;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
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