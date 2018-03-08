namespace SFeed.RedisRepository
{
    public static class RedisNameConstants
    {
        public static readonly string FeedRepoPrefix = "userfeed";
        public static readonly int FeedRepoSize = 100;
        public static readonly string GroupFollowerRepoPrefix = "groupFollower";
        public static readonly string UserFollowerRepoPrefix = "userFollower";
        public static readonly string WallPostRepoPrefix = "wallPost";
        public static readonly string CommentRepoPrefix = "comments";
        public static readonly int CommentRepoSize = 3;
        public static readonly string CommentCounterNamePrefix = "commentCounter";
        public static readonly string LikeCounterNamePrefix = "likeCounter";
    }
}
