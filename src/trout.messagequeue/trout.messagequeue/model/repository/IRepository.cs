using System;
using System.Linq;

namespace trout.emailservice.model.repository
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Fetch();
        void Add(T item);
        void Delete(T item);
        T First(Func<T, bool> filter);
    }
}