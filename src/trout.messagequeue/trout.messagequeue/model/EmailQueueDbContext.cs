using System;
using System.Data.Entity;
using System.Linq;
using trout.emailservice.model.repository;

namespace trout.emailservice.model
{
    public class EmailQueueDbContext : DbContext, IEmailQueueDbContext
    {
        public DbSet<EmailQueueItem> EmailQueueItems { get; set; }

        public EmailQueueDbContext()
            : base("trout.emailservice")
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