﻿using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository.Caching
{
    public interface INewsfeedCacheRepository
    {
        void AddEntry(NewsfeedCacheModel entry, IEnumerable<string> followers);
        void RemoveEntry(NewsfeedCacheModel entry, IEnumerable<string> followers);
        void RemovePost(string postId, IEnumerable<string> followers);
        void RemovePostsFromUser(string follower, IEnumerable<string> postIds);
        IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take);
    }
}
