using SFeed.Core.Models.Comments;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Models.Caching
{
    public class NewsfeedWallPostModel : WallPostCacheModel
    {
        public IEnumerable<CommentCacheModel> LatestComments { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public List<string> FeedDescription { get; set; }
    }
}
