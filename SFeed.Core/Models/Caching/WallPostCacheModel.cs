using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Models.Newsfeed
{
    public class WallPostCacheModel : TypedCacheItemBaseModel
    {
        public string Body { get; set; }

        public Actor PostedBy { get; set; }

        public short PostType { get; set; }

        public Actor WallOwner { get; set; }

        public List<UserCommentModel> LatestComments { get; set; }

        public int CommentCount { get; set; }

        public int LikeCount { get; set; }
    }
}
