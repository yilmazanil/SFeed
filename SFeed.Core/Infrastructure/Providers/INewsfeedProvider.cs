﻿using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedProvider : IDisposable
    {
        void AddPost(WallPostCacheModel wallPost);
        void UpdatePost(string postId, string body, WallPostType postType);
        void DeletePost(string Id);
        void AddComment(string postId, CommentCacheModel comment);
        void UpdateComment(string postId, CommentCacheModel comment);
        void DeleteComment(string postId, long commentId, string commentBy);
        void LikePost(string postId, string likedBy);
        void UnlikePost(string postId, string unlikedBy);
        IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId);
    }
}
