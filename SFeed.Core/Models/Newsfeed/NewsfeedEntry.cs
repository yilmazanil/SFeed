namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedEntry
    {
        public Actor From { get; set; }

        public Actor To { get; set; }

        public short ActionTypeId { get; set; }

        public string ActionBody { get; set; }

        public string ReferencePostId { get; set; }
    }
}
