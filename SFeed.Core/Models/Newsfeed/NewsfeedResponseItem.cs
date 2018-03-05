namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedResponseItem : NewsfeedEntry
    {
        public WallPostCacheModel ReferencedPost { get; set; }
    }
}
