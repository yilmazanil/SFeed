using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ITypedCacheRepository<T> : IDisposable where T : class
    {
        T AddItem(T cacheItem);
        T GetItem(object id);
        IEnumerable<T> GetByIds(IEnumerable<object> ids);
        void RemoveItem(object id);
        void UpdateItem(object id, T cacheItem);
    }
}
