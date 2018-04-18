namespace SFeed.RedisRepository.Base
{
    public static class RedisNameConstants
    {
        public static readonly string FeedRepoPrefix = "feed";
        public static readonly string ActivityIdPrefix = "activityId";
        public static readonly string ActivityPrefix = "activities";
        public static readonly int FeedRepoSize = 100;
        public static readonly string GroupFollowerRepoPrefix = "groupFollower";
        public static readonly string UserFollowerRepoPrefix = "userFollower";
        public static readonly string WallPostRepoPrefix = "wallPost";
        public static readonly string CommentRepoPrefix = "comment";
        public static readonly string CommentLatestRepoPrefix = "comment:latest";
        public static readonly int CommentRepoSize = 3;
        public static readonly string CommentCounterNamePrefix = "commentCounter";
        public static readonly string LikeCounterNamePrefix = "likeCounter";
        public static readonly string PostLikeCounterNamePrefix = "postLikeCounter";
        public static readonly string CommentLikeCounterNamePrefix = "commentLikeCounter";
    }
}
