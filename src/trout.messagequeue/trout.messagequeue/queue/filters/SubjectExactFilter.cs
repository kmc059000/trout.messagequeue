using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class SubjectExactFilter : DequeueFilter
    {
        private readonly string Subject;

        public SubjectExactFilter(string subject)
        {
            Subject = subject;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.Subject == Subject);
        }
    }
}