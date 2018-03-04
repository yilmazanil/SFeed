using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ITypedCacheRepository<T> : IDisposable where T : TypedCacheItemBaseModel
    {
        T AddItem(T cacheItem);
        T GetItem(string id);
        IEnumerable<T> GetByIds(IEnumerable<string> ids);
        void RemoveItem(string id);
        T UpdateItem(string id, T cacheItem);
    }
}
