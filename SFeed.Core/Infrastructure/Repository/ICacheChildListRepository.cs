using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheChildListRepository<T> : IDisposable
    {
        string ListName { get; }

        void AddOrUpdateItem(string parentKey, string key, T item);
        T GetItem(string parentKey, string key);

        IEnumerable<T> GetList(string parentKey);

        void RemoveItem(string parentKey, string key);

        void RecreateList(string parentKey, Dictionary<string,T> listItems);
        void ClearList(string key);
    }
}
