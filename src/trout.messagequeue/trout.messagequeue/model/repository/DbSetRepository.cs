using System;
using System.Data.Entity;
using System.Linq;

namespace trout.messagequeue.model.repository
{
    public class DbSetRepository<T> : IRepository<T>  where T : class
    {
        private readonly DbSet<T> Source;

        public DbSetRepository(DbSet<T> dbSet)
        {
            Source = dbSet;
        }

        public IQueryable<T> Fetch()
        {
            return Source;
        }

        public void Add(T item)
        {
            Source.Add(item);
        }

        public void Delete(T item)
        {
            Source.Remove(item);
        }

        public T First(Func<T, bool> filter)
        {
            return Source.FirstOrDefault(filter);
        }
    }
}