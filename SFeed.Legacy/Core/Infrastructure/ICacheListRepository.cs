using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheListRepository<T> : IDisposable
    {
        void AppendItem(string listKey, T item);
        void PrependItem(string listKey, T item);

        bool UpdateItem(string listKey, T item);
        bool UpdateItem(string listKey, Predicate<T> where, T item);


        T GetItem(string listKey, Expression<Func<T, bool>> where);

        IEnumerable<T> GetList(string listKey);

        void RemoveItem(string listKey, T item);
        void RemoveItem(string listKey, Predicate<T> predicate);

        void RecreateList(string listKey, IEnumerable<T> listItems);
        void ClearList(string listKey);
    }
}
