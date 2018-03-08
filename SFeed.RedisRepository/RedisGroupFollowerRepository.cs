
using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisGroupFollowerRepository : RedisListRepositoryBase<string>
    {
        public override string ListName => "groupFollower";
    }
}
