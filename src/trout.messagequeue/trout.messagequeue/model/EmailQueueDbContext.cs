using System.Data.Entity;

namespace trout.emailservice.model
{
    public class EmailQueueDbContext : DbContext
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
    }
}