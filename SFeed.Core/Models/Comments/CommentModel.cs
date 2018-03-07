using System;

namespace SFeed.Core.Models.Comments
{
    public class CommentModel : CommentCreateModel
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
