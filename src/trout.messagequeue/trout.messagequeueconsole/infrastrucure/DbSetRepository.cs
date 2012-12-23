using System;
using System.Linq;
using System.Linq.Expressions;
using trout.messagequeue.model.repository;
using System.Data.Entity;

namespace trout.messagequeueconsole.infrastrucure
{
    class DbSetRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext Context;
        private readonly DbSet<T> Source;

        public DbSetRepository(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Context = context;
            Source = Context.Set<T>();
        }

        public DbSetRepository(DbContext context, DbSet<T> set)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (set == null) throw new ArgumentNullException("set");

            Context = context;
            Source = set;
        }

        public IQueryable<T> Fetch()
        {
            return Source;
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException("item");

            Source.Add(item);
        }

        public void Delete(T item)
        {
            if (item == null) throw new ArgumentNullException("item");

            Source.Remove(item);
        }

        public T First(Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            return Source.FirstOrDefault(filter);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}