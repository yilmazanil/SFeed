using ServiceStack.Redis;

namespace SFeed.Data.Infrastructure
{
    public class RedisConnectionHelper
    {
        public static PooledRedisClientManager ClientManager = new PooledRedisClientManager("localhost");
    }
}
