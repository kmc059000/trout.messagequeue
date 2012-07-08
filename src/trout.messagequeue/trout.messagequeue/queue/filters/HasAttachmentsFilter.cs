using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public sealed class HasAttachmentsFilter : DequeueFilter
    {
        private bool HasAttachments = true;

        public HasAttachmentsFilter(bool hasAttachmentsFilter = true)
        {
            HasAttachments = hasAttachmentsFilter;
        }

        internal override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.AttachmentCount > 0 == HasAttachments);
        }
    }
}