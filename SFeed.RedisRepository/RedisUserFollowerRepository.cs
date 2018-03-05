namespace SFeed.RedisRepository
{
    public class RedisUserFollowerRepository : RedisListRepositoryBase<string>
    {
        protected override string ListPrefix => "userfollower";

    }
}
