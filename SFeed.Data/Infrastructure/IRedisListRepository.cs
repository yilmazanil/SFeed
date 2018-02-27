using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public interface IRedisListRepository<T,K>
    {
        void Add(T key, K item);
        IEnumerable<K> Retrieve(T key);
        void Refresh(T key, IEnumerable<K> itemCollection);
        void Remove(T key, K item);
        void Clear(T key);
    }
}
