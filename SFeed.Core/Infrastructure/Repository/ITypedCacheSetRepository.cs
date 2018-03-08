using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ITypedCacheSetRepository<T> : IDisposable
    {
        bool Contains(string setId, T value);
        void RemoveItem(string setId, T value);
        void AddItem(string setId, T item);
        IEnumerable<T> GetItems(string setId);
        void RecreateSet(string setId, IEnumerable<T> values);
        void Clear(string setId);
    }
}
