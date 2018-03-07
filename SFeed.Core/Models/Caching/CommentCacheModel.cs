namespace SFeed.Core.Models.Caching
{
    public class CommentCacheModel : CacheListItemBaseModel
    { 
        public long CommentId { get; set; }

        public string Body { get; set; }

        public string CreatedBy { get; set; }
    }
}
