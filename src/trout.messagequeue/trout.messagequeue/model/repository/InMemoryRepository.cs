using System;
using System.Collections.Generic;
using System.Linq;

namespace trout.messagequeue.model.repository
{
    public sealed class InMemoryRepository<T> : IRepository<T> where T : class
    {
        readonly List<T> Source = new List<T>();

        public IQueryable<T> Fetch()
        {
            return Source.AsQueryable();
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