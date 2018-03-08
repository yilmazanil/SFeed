using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ICacheRepository<T> : IDisposable
    {
        void AddItem(string key, T cacheItem);
        void RemoveItem(string key);
        IEnumerable<T> GetAllByKeys(IEnumerable<string> keys);
        void UpdateItem(string key, T cacheItem);
    }
}
