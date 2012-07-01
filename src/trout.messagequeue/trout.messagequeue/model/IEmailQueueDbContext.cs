using trout.messagequeue.model.repository;

namespace trout.messagequeue.model
{
    public interface IEmailQueueDbContext
    {
        IRepository<EmailQueueItem> EmailQueueItemRepo { get; }

        int SaveChanges();
    }
}