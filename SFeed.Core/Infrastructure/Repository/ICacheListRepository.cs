using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheListRepository<T> : IDisposable
    { 
        void AddItem(string listKey, T item);
        T GetItem(string listKey, Expression<Func<T, bool>> where);

        IEnumerable<T> GetList(string listKey);

        void RemoveItem(string listKey, T item);

        void RecreateList(string listKey, IEnumerable<T> listItems);
        void ClearList(string listKey);
    }
}
