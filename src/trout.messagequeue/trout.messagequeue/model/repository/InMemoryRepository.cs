using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace trout.messagequeue.model.repository
{
    /// <summary>
    /// Repository which is stored in memory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InMemoryRepository<T> : IRepository<T> where T : class
    {
        readonly List<T> Source = new List<T>();

        /// <summary>
        /// fetches all items in the repository
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Fetch()
        {
            return Source.AsQueryable();
        }

        /// <summary>
        /// Adds the specified item to the repository
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException("item");

            Source.Add(item);
        }

        /// <summary>
        /// Deletes the specified item from the repository
        /// </summary>
        /// <param name="item">The item.</param>
        public void Delete(T item)
        {
            if (item == null) throw new ArgumentNullException("item");

            Source.Remove(item);
        }

        /// <summary>
        /// Gets the First or default of items in the repository which match a predicate
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            return Source.FirstOrDefault(filter.Compile());
        }

        public void SaveChanges()
        {
            //in memory repository does not persist anything!
        }
    }
}