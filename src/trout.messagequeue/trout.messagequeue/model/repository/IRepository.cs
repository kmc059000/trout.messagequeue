using System;
using System.Linq;
using System.Linq.Expressions;

namespace trout.messagequeue.model.repository
{
    /// <summary>
    /// Repository for a specific type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T: class
    {
        /// <summary>
        /// Returns a set of type T
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Fetch();

        /// <summary>
        /// Adds the instance to the repository
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Deletes the instance from the repository
        /// </summary>
        /// <param name="item"></param>
        void Delete(T item);

        /// <summary>
        /// Returns the First or Default from the repository which matches a specific condition
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        T First(Expression<Func<T, bool>> filter);

        void SaveChanges();
    }
}