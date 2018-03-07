﻿using SFeed.Core.Models.Caching;
using System;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedEntry : CacheListItemBaseModel
    {
        public string ReferencePostId { get; set; }

        public string By { get; set; }

        public short EntryTypeId { get; set; }

        public string Body { get; set; }

        public DateTime EventDate { get; set; }

    }
}
