namespace SFeed.Core.Models.Comments
{
    public class CommentCreateRequest
    {
        public string Body { get; set; }

        public string WallPostId { get; set; }

        public string CreatedBy { get; set; }
    }
}
