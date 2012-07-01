using trout.emailservice.model.repository;

namespace trout.emailservice.model
{
    public interface IEmailQueueDbContext
    {
        IRepository<EmailQueueItem> EmailQueueItemRepo { get; }

        int SaveChanges();
    }
}