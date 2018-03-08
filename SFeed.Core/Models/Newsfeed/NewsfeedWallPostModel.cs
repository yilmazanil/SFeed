using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Models.Caching
{
    public class NewsfeedWallPostModel : WallPostCacheModel
    {
        public List<CommentCacheModel> LatestComments { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }
    }
}
