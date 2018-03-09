using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ICacheItemRepository<T> : IDisposable
    {
        void AddItem(string key, T cacheItem);
        void AddOrUpdateItem(string key, T cacheItem);
        void RemoveItem(string key);
        IEnumerable<T> GetAllByKeys(IEnumerable<string> keys);
    }
}
