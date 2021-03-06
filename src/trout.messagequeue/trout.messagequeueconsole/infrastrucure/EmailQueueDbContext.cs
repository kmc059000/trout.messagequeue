﻿using System.Data.Entity;
using trout.messagequeue.model;

namespace trout.messagequeueconsole.infrastrucure
{
    public class EmailQueueDbContext : DbContext
    {
        public DbSet<EmailQueueItem> EmailQueueItems { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailQueueDbContext()
            : base("trout.messagequeue")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connString"></param>
        public EmailQueueDbContext(string connString)
            : base (connString)
        {

        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmailQueueItem>().HasKey(i => i.ID);
        }
    }
}