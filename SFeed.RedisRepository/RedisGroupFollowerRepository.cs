namespace SFeed.RedisRepository
{
    public class RedisGroupFollowerRepository : RedisListRepositoryBase<string>
    {
        protected override string ListPrefix => "groupFollower";
    }
}
