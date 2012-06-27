using System;
using System.Data.Entity;
using System.Linq;

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


        public IQueryable<EmailQueueItem> FetchEmailQueueItems()
        {
            return this.EmailQueueItems;
        }

        public void Add(EmailQueueItem item)
        {
            this.EmailQueueItems.Add(item);
        }
    }
}