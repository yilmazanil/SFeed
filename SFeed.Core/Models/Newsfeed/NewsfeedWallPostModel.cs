using SFeed.Core.Models.Caching;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedWallPostModel : TypedCacheItemBaseModel
    {
        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public string WallOwnerId { get; set; }
    }
}
