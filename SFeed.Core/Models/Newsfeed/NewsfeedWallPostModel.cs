namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedWallPostModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public string WallOwnerId { get; set; }
    }
}
