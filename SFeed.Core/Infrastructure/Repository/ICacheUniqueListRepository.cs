using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheUniqueListRepository<T> : IDisposable
    {
        string ListName { get; }

        void AddOrUpdateItem(string listKey, string itemId, T item);
        T GetItem(string listKey, string itemId);

        IEnumerable<T> GetList(string listKey);

        void RemoveItem(string listKey, string itemId);

        void RecreateList(string listKey, Dictionary<string,T> listItems);
        void ClearList(string listKey);
    }
}
