using SFeed.Core.Models.Wall;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedItem
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedType FeedType { get; set; }

        public WallModel WallOwner { get; set; }
    }
}
