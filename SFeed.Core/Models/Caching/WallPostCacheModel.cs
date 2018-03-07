using SFeed.Core.Models.Caching;
using System.Collections.Generic;

namespace SFeed.Core.Models.Newsfeed
{
    public class WallPostCacheModel : CacheListItemBaseModel
    {
        public string Body { get; set; }

        public string PostedBy { get; set; }

        public short PostType { get; set; }

        public Actor WallOwner { get; set; }

        public int CommentCount { get; set; }

        public List<CommentCacheModel> LatestComments { get; set; }

        //x and other users liked
        public List<string> Likes { get; set; }

    }
}
