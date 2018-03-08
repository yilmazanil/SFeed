using SFeed.Core.Models.Caching;
using System.Collections.Generic;

namespace SFeed.Core.Models.Newsfeed
{
    public class WallPostCacheModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public Actor WallOwner { get; set; }
    }
}
