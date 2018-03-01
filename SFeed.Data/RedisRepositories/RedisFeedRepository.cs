using SFeed.Data.Infrastructure;
using SFeed.Model;
using System.Collections.Generic;
using System;

namespace SFeed.Data.RedisRepositories
{
    public class RedisFeedRepository : RedisListRepositoryBase<long>
    {
        protected override string ListPrefix => "userfeed";

        //public void AddToUserFeeds(IEnumerable<int> userIds, long postId)
        //{
        //    using (var redis = RedisHelper.ClientManager.GetClient())
        //    {
        //        var feedlist = redis.As<long>();

        //        foreach (var userId in userIds)
        //        {
        //            feedlist.Lists[ListPrefix + userId].Add(postId);
        //        }
        //    }
        //}

        //public void DeleteFromUserFeeds(long postId, IEnumerable<int> userIds)
        //{
        //    using (var redis = RedisHelper.ClientManager.GetClient())
        //    {
        //        var feedlist = redis.As<long>();

        //        foreach (var userId in userIds)
        //        {
        //            var associatedRedisEntry = feedlist.Lists[ListPrefix + userId];
        //            if (associatedRedisEntry != null)
        //            {
        //                associatedRedisEntry.Remove(postId);
        //            }
        //        }

        //    }
        //}

        //public IEnumerable<SocialPostModel> GetUserFeeds(int userId)
        //{
        //    using (var redis = RedisHelper.ClientManager.GetClient())
        //    {
        //        var feedlist = redis.As<long>();

        //        var postIds = feedlist.Lists[ListPrefix + userId].GetAll();

        //        var posts = redis.As<SocialPostModel>();

        //        return posts.GetByIds(postIds);
        //    }
        //}
    }
}
