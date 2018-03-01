using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface IRepository<T>: IDisposable where T :class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(int id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        bool Any(Expression<Func<T, bool>> where);
        void CommitChanges();
    }
}
