using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue filter for whether a subject is exactly equal to a specified string
    /// </summary>
    public sealed class SubjectExactFilter : DequeueFilter
    {
        private readonly string Subject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject"></param>
        public SubjectExactFilter(string subject)
        {
            Subject = subject;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Subject == Subject);
        }
    }
}