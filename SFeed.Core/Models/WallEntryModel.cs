using System;

namespace SFeed.Core.Models
{
    public class WallEntryModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string CreatedBy { get; set; }

        public short EntryType { get; set; }
    }
}
