namespace SFeed.Data.Infrastructure
{
    public interface IRedisTypedRepository<T> where T : class
    {
        void Add(T cacheItem);
        void Retrieve(object id);
    }
}
