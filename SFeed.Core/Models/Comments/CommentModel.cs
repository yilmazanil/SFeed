using System;

namespace SFeed.Core.Models.Comments
{
    public class CommentModel
    {
        public long Id { get; set; }

        public string Body { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int LikeCount { get; set; }

    }
}
