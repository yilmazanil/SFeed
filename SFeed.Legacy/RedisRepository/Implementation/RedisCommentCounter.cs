using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisCommentCounter : RedisCounterBase
    {
        public override string counterPrefix => RedisNameConstants.CommentCounterNamePrefix;
    }
}
