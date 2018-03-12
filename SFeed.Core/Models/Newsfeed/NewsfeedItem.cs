namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedItem
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedType FeedType { get; set; }

        public WallOwner WallOwner { get; set; }
    }
}
