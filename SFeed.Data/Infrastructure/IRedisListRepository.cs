using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public interface IRedisListRepository<T,K>
    {
        void Add(T key, K item);
        IEnumerable<K> Retrieve(T key);
        void Recreate(T key, IEnumerable<K> itemCollection);
        void Remove(T key, K item);
        //Must be used with caution
        //void Clear(T key);
        bool Exists(T key, K value);
    }
}
