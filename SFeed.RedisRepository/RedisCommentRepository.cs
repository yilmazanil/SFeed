using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisCommentRepository : RedisNamedListRepositoryBase<CommentCacheModel>
    {
        public override string ListName => "postComment";
    }
}
