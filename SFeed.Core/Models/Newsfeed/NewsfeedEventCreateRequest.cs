using SFeed.Core.Models.Wall;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedItem
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedActionType FeedType { get; set; }

        public NewsfeedWallModel WallOwner { get; set; }
    }
}
