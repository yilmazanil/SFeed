namespace SFeed.Data.Infrastructure
{
    public abstract class RedisRepositoryBase<T> : ITypedCacheRepository<T> where T :class
    {
        public void Add(T item)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var socialPosts = redis.As<T>();
                socialPosts.Store(item);
            }
        }

        public void Retrieve(object id)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var socialPosts = redis.As<T>();
                socialPosts.GetById(id);
            }
        }
    }
}
