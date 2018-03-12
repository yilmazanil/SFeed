using System;

namespace SFeed.Core.Models.Comments
{
    public class CommentUpdateRequest
    {
        public string PostId { get; set; }

        public long CommentId { get; set; }

        public string Body { get; set; }
    }
}
