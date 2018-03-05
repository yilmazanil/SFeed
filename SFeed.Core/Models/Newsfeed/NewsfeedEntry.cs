namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedEntry
    {
        public Actor From { get; set; }

        public Actor To { get; set; }

        public short TypeId { get; set; }

        public string Body { get; set; }

        public string ReferencePostId { get; set; }
    }
}
