using System.Data.Entity;
using System.Linq;

namespace trout.emailservice.model
{
    public interface IEmailQueueDbContext
    {
        IQueryable<EmailQueueItem> FetchEmailQueueItems();

        void Add(EmailQueueItem item);

        int SaveChanges();
    }
}