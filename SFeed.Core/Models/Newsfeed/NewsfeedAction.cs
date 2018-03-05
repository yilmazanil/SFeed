namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedAction
    {
        public string From { get; set; }

        public Actor To { get; set; }

        public short ActionId { get; set; }

        public string ReferencePostId { get; set; }

        public string ActionBody { get; set; }
    }
}
