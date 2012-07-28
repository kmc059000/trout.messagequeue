using trout.messagequeue.model.repository;

namespace trout.messagequeue.model
{
    /// <summary>
    /// Abstract DbContext implementation for trout
    /// </summary>
    public interface IEmailQueueDbContext
    {
        /// <summary>
        /// Repository of EmailQueueItems
        /// </summary>
        IRepository<EmailQueueItem> EmailQueueItemRepo { get; }

        /// <summary>
        /// Saves changes that have occurred in this DbContext
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}