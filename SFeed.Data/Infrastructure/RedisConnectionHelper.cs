using ServiceStack.Redis;

namespace SFeed.Data.Infrastructure
{
    public class RedisHelper
    {
        public static PooledRedisClientManager ClientManager = new PooledRedisClientManager("localhost");
    }
}
