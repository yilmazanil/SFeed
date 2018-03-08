using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisUserFollowerRepository : RedisListRepositoryBase<string>
    {
        public override string ListName => RedisNameConstants.UserFollowerRepoPrefix;
    }
}
