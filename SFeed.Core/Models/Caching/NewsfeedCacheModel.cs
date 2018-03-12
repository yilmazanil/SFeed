using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Models.Caching
{
    public class NewsfeedCacheModel
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public NewsfeedType FeedType { get; set; }
    }
}
