using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Models.WallPost
{
    public class WallPostModel : WallPostBaseModel
    {
        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public IEnumerable<CommentModel> LatestComments { get; set; }
    }
}
