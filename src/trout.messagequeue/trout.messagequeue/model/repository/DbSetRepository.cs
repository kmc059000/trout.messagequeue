using System;
using System.Data.Entity;
using System.Linq;

namespace trout.messagequeue.model.repository
{
    public sealed class DbSetRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> Source;

        public DbSetRepository(DbSet<T> dbSet)
        {
            if(dbSet == null) throw new ArgumentNullException("dbSet");

            Source = dbSet;
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

        public T First(Func<T, bool> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            return Source.FirstOrDefault(filter);
        }
    }
}