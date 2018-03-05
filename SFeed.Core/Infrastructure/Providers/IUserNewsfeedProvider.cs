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
        void AddAction(NewsfeedEntry newsFeedAction);
        void RemoveAction(NewsfeedEntry newsFeedAction);
        void DeleteAction(NewsfeedEntry item);
        IEnumerable<NewsfeedResponseItem> GetNewsfeed(string userId);
    }
}
