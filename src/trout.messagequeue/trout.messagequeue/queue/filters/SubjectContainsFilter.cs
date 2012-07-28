using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue filter for whether a subject contains a specified string
    /// </summary>
    public sealed class SubjectContainsFilter : DequeueFilter
    {
        private readonly string Subject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject"></param>
        public SubjectContainsFilter(string subject)
        {
            Subject = subject;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Subject.Contains(Subject));
        }
    }
}