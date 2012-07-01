using System.Data.Entity;
using trout.messagequeue.model.repository;

namespace trout.messagequeue.model
{
    public class EmailQueueDbContext : DbContext, IEmailQueueDbContext
    {
        public DbSet<EmailQueueItem> EmailQueueItems { get; set; }

        public EmailQueueDbContext()
            : base("trout.messagequeue")
        {
        }

        public EmailQueueDbContext(string connString)
            : base (connString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmailQueueItem>().HasKey(i => i.ID);
        }

        private IRepository<EmailQueueItem> emailRepository;

        public IRepository<EmailQueueItem> EmailQueueItemRepo
        {
            get { return emailRepository ?? (emailRepository = new DbSetRepository<EmailQueueItem>(this.EmailQueueItems)); }
        }
    }
}