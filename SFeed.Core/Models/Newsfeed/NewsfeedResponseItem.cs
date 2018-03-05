namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedResponseItem : NewsfeedAction
    {
        public WallPostNewsfeedModel ReferencedPost { get; set; }
    }
}
