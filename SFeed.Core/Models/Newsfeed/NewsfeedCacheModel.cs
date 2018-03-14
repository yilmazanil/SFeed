using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedCacheModel
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedType FeedType { get; set; }
    }
}
