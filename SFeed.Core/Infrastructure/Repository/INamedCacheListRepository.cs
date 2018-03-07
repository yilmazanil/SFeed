using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface INamedCacheListRepository<T> : IDisposable where T : CacheListItemBaseModel
    {
        string ListName { get; }

        void AddOrUpdateItem(string listId, T item);
        T GetItem(string listId, string itemId);

        IEnumerable<T> GetList(string listId);
       
        void RemoveItem(string listId, string itemId);
        void RecreateList(string listId, IEnumerable<T> listItems);
        void DeleteList(string listId);
    }
}
