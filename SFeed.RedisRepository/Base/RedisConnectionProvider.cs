using ServiceStack.Redis;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SFeed.RedisRepository.Base
{
    public static class RedisConnectionProvider
    {
        //private static readonly string connectionString = ConfigurationManager.ConnectionStrings["RedisConnection"].ConnectionString;

        private static readonly IEnumerable<string> RedisSlaves = ConfigurationManager.ConnectionStrings["RedisSlaveConnections"].ConnectionString.Split(',');

        private static readonly string RedisMaster = ConfigurationManager.ConnectionStrings["RedisMasterConnection"].ConnectionString;

        //private static PooledRedisClientManager ClientManager = new PooledRedisClientManager(connectionString);

        private static PooledRedisClientManager ClientManager = new PooledRedisClientManager(new List<string> { RedisMaster }, RedisSlaves);

        public static IRedisClient GetClient()
        {
            return ClientManager.GetClient();
        }

        public static IRedisClient GetReadOnlyClient()
        {
            return ClientManager.GetReadOnlyClient();
        }

    }
}
