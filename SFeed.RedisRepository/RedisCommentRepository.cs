using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisCommentRepository : RedisUniqueListBase<CommentCacheModel>
    {
        public override string ListName => "postComment";
    }
}
