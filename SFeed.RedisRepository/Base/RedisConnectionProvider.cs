using ServiceStack.Redis;
using System.Configuration;

namespace SFeed.RedisRepository
{
    public static class RedisConnectionProvider
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["RedisConnection"].ConnectionString;

        private static PooledRedisClientManager ClientManager = new PooledRedisClientManager(connectionString);

        public static IRedisClient GetClient()
        {
            return ClientManager.GetClient();
        }

    }
}
