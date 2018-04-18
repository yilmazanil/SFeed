using SFeed.Core.Models.WallPost;
using System.Collections.Generic;

namespace SFeed.Core.Models.Caching
{
    public class NewsfeedWallPostModel : WallPostDetailedModel
    {
        public List<string> FeedDescription { get; set; }
    }
}
