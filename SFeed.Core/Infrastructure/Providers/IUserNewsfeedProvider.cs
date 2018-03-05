using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddPost(WallPostNewsfeedModel wallPost);
        void UpdatePost(WallPostNewsfeedModel wallPost);
        void DeletePost(string Id);
        void AddAction(NewsfeedAction newsFeedAction);
        void RemoveAction(NewsfeedAction newsFeedAction);
        void DeleteAction(NewsfeedAction item);
        IEnumerable<NewsfeedResponseItem> GetNewsfeed(string userId);
    }
}
