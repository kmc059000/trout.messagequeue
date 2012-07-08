using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public sealed class SubjectExactFilter : DequeueFilter
    {
        private readonly string Subject;

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