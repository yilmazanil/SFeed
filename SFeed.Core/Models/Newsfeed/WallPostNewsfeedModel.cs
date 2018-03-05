using SFeed.Core.Models.Caching;

namespace SFeed.Core.Models.Newsfeed
{
    public class WallPostNewsfeedModel : TypedCacheItemBaseModel
    {
        public string Body { get; set; }

        public Actor PostedBy { get; set; }

        public short PostType { get; set; }

        public Actor WallOwner { get; set; }

        public string[] LatestComments { get; set; }

        public int CommentCount { get; set; }

        public int LikeCount { get; set; }
    }
}
