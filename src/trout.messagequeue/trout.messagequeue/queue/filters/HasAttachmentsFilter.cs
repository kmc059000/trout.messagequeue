using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// Dequeue Filter which only returns whether an email does or does not have attachments
    /// </summary>
    public sealed class HasAttachmentsFilter : DequeueFilter
    {
        private readonly bool HasAttachments = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hasAttachmentsFilter">default is true</param>
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