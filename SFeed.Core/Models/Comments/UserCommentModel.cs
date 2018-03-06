﻿using System;

namespace SFeed.Core.Models.Comments
{
    public class UserCommentModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string WallPostId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
