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