namespace SFeed.Core.Models
{
    public class WallOwner
    {
        public string Id { get; set; }

        public WallOwnerType WallOwnerType { get; set; }

        public bool IsPublic { get; set; }
    }
}
