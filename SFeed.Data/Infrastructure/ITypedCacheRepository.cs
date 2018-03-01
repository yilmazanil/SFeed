using System.Collections;
using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public interface ITypedCacheRepository<T> where T : class
    {
        T AddItem(T cacheItem);
        T GetItem(object id);
        IEnumerable<T> GetByIds(IEnumerable<object> ids);
        void RemoveItem(string id);
    }
}
