namespace SFeed.Data.Infrastructure
{
    public interface ITypedCacheRepository<T> where T : class
    {
        void Add(T cacheItem);
        void Retrieve(object id);
    }
}
