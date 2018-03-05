namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedResponseItem : NewsfeedEntry
    {
        public WallPostNewsfeedModel ReferencedPost { get; set; }
    }
}
