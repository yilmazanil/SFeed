namespace SFeed.Model
{
    public class WallEntryModel
    {
        public long Id { get; set; }

        public string Body { get; set; }

        public int CreatedBy { get; set; }

        public short EntryType { get; set; }
    }
}
