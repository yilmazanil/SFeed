using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using System;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;
using System.Linq;

namespace SFeed.Business.Services
{
    public class UserNewsfeedService : IUserNewsfeedService, IDisposable
    {
        IUserNewsfeedProvider newsFeedProvider;
        IUserWallPostProvider wallPostProvider;

        public UserNewsfeedService()
        {
            this.newsFeedProvider = new UserNewsfeedProvider();
            this.wallPostProvider = new UserWallPostProvider();
        }

        public void Dispose()
        {
            if (newsFeedProvider != null)
            {
                newsFeedProvider.Dispose();
            }
        }

        public IEnumerable<NewsfeedResponseItem> GetUserFeed(string userId)
        {
            //TODO:UpdateWithParsers currently just parses plain texts
            var feedEntries =  newsFeedProvider.GetUserFeed(userId);

            if (feedEntries.Any())
            {
                var postIds = feedEntries.Select(f => f.ReferenceEntryId);
                var associatedPosts = wallPostProvider.GetEntries(postIds).Select(p => new NewsfeedResponseItem
                {
                    ItemType = NewsfeedEntryTypeEnum.wallpost,
                    Item = p,
                    ItemId = p.Id
                });

                return associatedPosts;
            }
            return null;
        }
    }
}
