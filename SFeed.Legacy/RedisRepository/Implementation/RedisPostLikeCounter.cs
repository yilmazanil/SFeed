using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisPostLikeCounter : RedisCounterBase
    {
        public override string counterPrefix => RedisNameConstants.CommentCounterNamePrefix;
    }
}
