using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Models.WallPost
{
    public class WallPostModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public WallOwner WallOwner { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public List<CommentModel> LatestComments { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
