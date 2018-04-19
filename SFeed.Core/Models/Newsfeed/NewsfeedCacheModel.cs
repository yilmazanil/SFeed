using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedEventModel
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedEventType FeedType { get; set; }
    }
}
