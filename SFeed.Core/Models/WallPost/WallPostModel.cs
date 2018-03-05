namespace SFeed.Core.Models.WallPost
{
    public class WallPostModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public Actor PostedBy { get; set; }

        public short PostType { get; set; }
    }
}
