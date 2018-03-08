using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisLikeCounter : RedisCounterBase
    {
        public override string counterPrefix => RedisNameConstants.CommentCounterNamePrefix;
    }
}
