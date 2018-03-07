using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface INamedCacheListRepository<T> : IDisposable where T : CacheListItemBaseModel
    {
        void AddItem(string listName, T item);

        IEnumerable<T> GetList(string listName);
        T GetItem(string listName, string itemId);

        void RemoveItem(string listName, string itemId);

        void RecreateList(string listName, IEnumerable<T> listItems);
        void DeleteList(string listName);
    }
}
