using System;

namespace SFeed.Core.Models.Comments
{
    public class CommentCacheModel 
    { 
        public string PostId { get; set; }

        public long CommentId { get; set; }

        public string Body { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
