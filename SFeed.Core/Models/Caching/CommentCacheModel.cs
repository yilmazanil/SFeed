namespace SFeed.Core.Models.Caching
{
    public class CommentCacheModel : CacheListItemBaseModel
    { 
        public string Body { get; set; }

        public string CreatedBy { get; set; }
    }
}
