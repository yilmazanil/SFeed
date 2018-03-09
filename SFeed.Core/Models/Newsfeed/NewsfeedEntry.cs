namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedEntry
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedType FeedType { get; set; }
    }
}
