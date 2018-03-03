using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ICacheListRepository<T> : IDisposable
    {
        void AppendToList(string listKey, T item);
        void PrependToList(string listKey, T item);
        IEnumerable<T> GetList(string listKey);
        void RecreateList(string listKey, IEnumerable<T> listItems);
        void RemoveFromList(string listKey, T item);
        void ClearList(string listKey);
        void DeleteList(string listKey);
    }
}
