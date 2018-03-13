using System.Collections.Generic;

namespace SFeed.Core.Models.Follower
{
    public class FollowerPagedModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<string> Records { get; set; }
    }
}
