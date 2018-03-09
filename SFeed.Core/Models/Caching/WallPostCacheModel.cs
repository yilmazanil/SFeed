﻿using System;

namespace SFeed.Core.Models.Newsfeed
{
    public class WallPostCacheModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public WallOwnerCacheModel WallOwner { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; } 
    }
}
