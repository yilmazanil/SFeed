﻿namespace SFeed.Core.Models.WallPost
{
    public class WallPostCreateRequest
    {
        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public string WallOwnerId { get; set; }
    }
}
