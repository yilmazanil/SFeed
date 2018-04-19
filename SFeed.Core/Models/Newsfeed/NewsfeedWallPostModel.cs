using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.WallPost;
using System.Collections.Generic;

namespace SFeed.Core.Models.Caching
{
    public class NewsfeedWallPostModel : WallPostModel
    {
        public List<NewsfeedAction> FeedDescription { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }
    }
}
