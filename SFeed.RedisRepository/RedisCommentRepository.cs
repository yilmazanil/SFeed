using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisCommentRepository : RedisListBehaviourBase<CommentCacheModel>
    {
        public override string ListName => "postComment";
    }
}
