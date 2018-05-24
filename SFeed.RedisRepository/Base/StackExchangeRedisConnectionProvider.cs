using StackExchange.Redis;
using System.Configuration;

namespace SFeed.RedisRepository.Base
{
    public class StackExchangeRedisConnectionProvider
    {
        private static ConnectionMultiplexer redisConnectionMultiplexer = ConnectionMultiplexer.Connect(GenerateConnectionString());

        private static string GenerateConnectionString()
        {
            var masterConnection = ConfigurationManager.ConnectionStrings["RedisMasterConnection"].ConnectionString;
            var slaveConnections = ConfigurationManager.ConnectionStrings["RedisSlaveConnections"].ConnectionString;

            return string.Concat(masterConnection, ',', slaveConnections);
        }

        internal static IDatabase GetDataBase()
        {
            return redisConnectionMultiplexer.GetDatabase();
        }

        internal static ITransaction GetTransaction()
        {
            return GetDataBase().CreateTransaction();
        }

        internal static IServer GetServer()
        {
            return redisConnectionMultiplexer.GetServer(ConfigurationManager.ConnectionStrings["RedisMasterConnection"].ConnectionString);
        }
    }
}
