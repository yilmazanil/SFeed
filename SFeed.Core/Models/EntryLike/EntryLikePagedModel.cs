using System.Collections.Generic;

namespace SFeed.Core.Models.EntryLike
{
    public class EntryLikePagedModel
    {
        public int TotalCount { get; set; }

        public IEnumerable<string> Records { get; set; }
    }
}
