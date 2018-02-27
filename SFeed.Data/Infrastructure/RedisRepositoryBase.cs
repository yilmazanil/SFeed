namespace SFeed.Data.Infrastructure
{
    public abstract class RedisRepositoryBase<T> : IRedisTypedRepository<T> where T :class
    {
        public void Add(T item)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var redisInstance = redis.As<T>();
                redisInstance.Store(item);
            }
        }

        public void Retrieve(object id)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var redisInstance = redis.As<T>();
                redisInstance.GetById(id);
            }
        }
    }
}
