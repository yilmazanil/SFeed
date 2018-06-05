using Newtonsoft.Json;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.GroupWall;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.Wall;
using SFeed.RedisRepository.Base;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisGroupWallRepository : RedisRepositoryBase, IGroupWallRepository
    {
        public string FeedPrefix => RedisNameConstants.GroupWallPrefix;

        IWallPostCacheRepository wallPostRepo;
        ICommentCountCacheRepository commentCountRepo;
        ILikeCountCacheRepository entryLikeRepo;

        public RedisGroupWallRepository() : this(
            new RedisCommentCountRepository()
            , new RedisEntryLikeRepository(),
            new RedisWallPostRepository())
        {

        }
        public RedisGroupWallRepository(ICommentCountCacheRepository commentCountRepo,
             ILikeCountCacheRepository entryLikeRepo,
             IWallPostCacheRepository wallPostRepo)
        {
            this.commentCountRepo = commentCountRepo;
            this.entryLikeRepo = entryLikeRepo;
            this.wallPostRepo = wallPostRepo;
        }
        public void RefreshGroupWall(string groupId, IEnumerable<string> postIds)
        {
            var key = GetEntryKey(FeedPrefix, groupId);
            var transaction = StackExchangeRedisConnectionProvider.GetTransaction();
            foreach (var postId in postIds)
            {
                transaction.SetAddAsync(key, postId);
            }
            transaction.Execute();
        }

        public void ClearGroupWall(string groupId)
        {
            var key = GetEntryKey(FeedPrefix, groupId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.KeyDelete(key);
        }

        public IEnumerable<GroupWallResponseModel> GetGroupWall(string groupId)
        {
            var result = new List<GroupWallResponseModel>();
            var key = GetEntryKey(FeedPrefix, groupId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            var allPostIds = db.SetMembers(key).ToList();

            if (allPostIds != null && allPostIds.Count > 0)
            {
                var postKeys = allPostIds.Select(p => (RedisKey)GetEntryKey(RedisNameConstants.WallPostRepoPrefix, p)).ToArray();
                var likeKeys = allPostIds.Select(p => (RedisKey)GetEntryKey(RedisNameConstants.PostLikeCounterNamePrefix, p)).ToArray();
                var comment = allPostIds.Select(p => (RedisKey)GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, p)).ToArray();
                //foreach (var postId in allPostIds)
                //{

                //var transaction = db.CreateTransaction();
                //var posts = transaction.StringGetAsync(postKeys);
                var posts = db.StringGet(postKeys);
                foreach (var post in posts)
                {
                    var currentPost = JsonConvert.DeserializeObject<WallPostCacheModel>(post);

                    var model = new GroupWallResponseModel
                    {
                        Body = currentPost.Body,
                        CreatedDate = currentPost.CreatedDate,
                        Id = currentPost.Id,
                        ModifiedDate = currentPost.ModifiedDate,
                        PostedBy = currentPost.PostedBy,
                        PostType = currentPost.PostType,
                        WallOwner = new WallModel { OwnerId = currentPost.TargetWall.Id, WallOwnerType = (WallType)currentPost.TargetWall.WallOwnerTypeId },
                    };

                    var currentCommentCountEntry = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, currentPost.Id);
                    var totalCommentCountEntry = db.StringGet(currentCommentCountEntry);
                    model.CommentCount = !string.IsNullOrWhiteSpace(totalCommentCountEntry) ? Convert.ToInt32(totalCommentCountEntry) : 0;

                    var totalLikeCountEntryKey = GetEntryKey(RedisNameConstants.PostLikeCounterNamePrefix, currentPost.Id);
                    var value = db.StringGet(totalLikeCountEntryKey);
                    model.LikeCount = !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
                    result.Add(model);
                }
                //JsonConvert.DeserializeObject<List<WallPostCacheModel>>();


                //    var comments = transaction.StringGetAsync(comment);
                //    transaction.Execute();


                // var entryKey = GetEntryKey(RedisNameConstants.WallPostRepoPrefix, postId);
                // var currentPostValue = db.StringGet(entryKey);
                //// var currentPost =  !string.IsNullOrWhiteSpace(currentPostValue) ? JsonConvert.DeserializeObject<WallPostCacheModel>(currentPostValue) : null;
                // if (!string.IsNullOrWhiteSpace(currentPostValue))
                // {
                //     //var currentCommentCountEntry = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
                //     //var totalCommentCountEntry = db.StringGet(currentCommentCountEntry);
                //     //var totalCommentCount = !string.IsNullOrWhiteSpace(totalCommentCountEntry) ? Convert.ToInt32(totalCommentCountEntry) : 0;

                //     //var totalLikeCountEntryKey = GetEntryKey(RedisNameConstants.PostLikeCounterNamePrefix, postId);
                //     //var value = db.StringGet(entryKey);
                //     //var totalLikeCount =  !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;

                //     var model = new GroupWallResponseModel
                //     {
                //         //Body = currentPost.Body,
                //         //CreatedDate = currentPost.CreatedDate,
                //         Id = "123"//,
                //         //ModifiedDate = currentPost.ModifiedDate,
                //         //PostedBy = currentPost.PostedBy,
                //         //PostType = currentPost.PostType,
                //         //WallOwner = new WallModel { OwnerId = currentPost.TargetWall.Id, WallOwnerType = (WallType)currentPost.TargetWall.WallOwnerTypeId },
                //         //LikeCount = totalLikeCount,
                //         //CommentCount = totalCommentCount
                //     };
                //     result.Add(model);
                //}
                //}
            }
            return result;
        }
    }
}
