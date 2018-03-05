namespace SFeed.Core.Models.WallPost
{
    public class WallPostCreateRequest
    {
        public string Body { get; set; }

        public Actor PostedBy { get; set; }

        public short PostType { get; set; }

        public Actor WallOwner { get; set; }
    }
}
