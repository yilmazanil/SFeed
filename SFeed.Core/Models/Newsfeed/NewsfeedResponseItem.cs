namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedResponseItem
    {
        public dynamic Item { get; set; }

        public string ItemId { get; set; }

        public NewsfeedEntryType ItemType;
    }
}
