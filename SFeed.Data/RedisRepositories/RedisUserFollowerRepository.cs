using SFeed.Data.Infrastructure;

namespace SFeed.Data.RedisRepositories
{
    public class RedisUserFollowerRepository : RedisListRepositoryBase<int>
    {
        protected override string ListPrefix => "userfollower";
    }
}
