namespace SFeed.Core.Models.Wall
{
    public class WallModel
    {
        public string OwnerId { get; set; }

        public WallType WallOwnerType { get; set; }

        public bool IsPublic { get; set; }
    }
}
