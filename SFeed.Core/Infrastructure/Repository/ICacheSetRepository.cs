using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ICacheSetRepository : IDisposable
    {
        bool Contains(string setId, string value);
        void RemoveItem(string setId, string value);
        void AddItem(string setId, string item);
        IEnumerable<string> GetItems(string setId);
        void Clear(string setId);
 
    }
}
