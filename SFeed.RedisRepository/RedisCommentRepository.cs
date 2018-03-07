using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisCommentRepository : RedisListRepositoryBase<CommentCacheModel>
    {
        protected override string ListPrefix => "postComment";
    }
}
