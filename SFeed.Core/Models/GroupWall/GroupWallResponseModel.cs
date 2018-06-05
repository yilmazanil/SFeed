using SFeed.Core.Models.WallPost;

namespace SFeed.Core.Models.GroupWall
{
    public class GroupWallResponseModel : WallPostModel
    {
        public int LikeCount { get; set; }

        public int CommentCount { get; set; }
    }
}
