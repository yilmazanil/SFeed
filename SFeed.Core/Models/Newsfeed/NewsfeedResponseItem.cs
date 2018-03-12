using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedResponseItem
    {
        public List<NewsfeedItem> UserActions { get; set; }

        public NewsfeedWallPostModel ReferencedPost { get; set; }
    }
}
