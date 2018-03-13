namespace SFeed.Core.Models.WallPost
{
    public class WallPostUpdateRequest
    {
        public string PostId { get; set; }

        public string Body { get; set; }

        public WallPostType PostType { get; set; }
    }
}
