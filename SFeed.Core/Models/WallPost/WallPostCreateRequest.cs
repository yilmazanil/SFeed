using SFeed.Core.Models.Wall;

namespace SFeed.Core.Models.WallPost
{
    public class WallPostCreateRequest
    {
        public string Body { get; set; }

        public string PostedBy { get; set; }

        public WallPostType PostType { get; set; }

        public WallModel TargetWall { get; set; }
    }
}
