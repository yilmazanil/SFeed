using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructure.Repository;
using SFeed.RedisRepository;
using System.Linq;
using SFeed.Core.Models.Caching;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedResponseProvider : INewsfeedResponseProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        IReadonlyItemRepository<NewsfeedWallPostModel> wallPostCacheRepo;

        public UserNewsfeedResponseProvider()
        {
            this.feedCacheRepo = new RedisUserFeedRepository();
            this.wallPostCacheRepo = new RedisNewsfeedResponseRepository();
        }

        public IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
        {
            var feeds = feedCacheRepo.GetList(userId);
            var ignoreList = new List<string>();
            var response = new List<NewsfeedResponseItem>();

            foreach (var feedEntry in feeds)
            {
                if (!ignoreList.Contains(feedEntry.ReferencePostId))
                {
                    var existingItemIndex = response.FindIndex(n => n.ReferencedPost.Id == feedEntry.ReferencePostId);
                    if (existingItemIndex > -1)
                    {
                        response[existingItemIndex].UserActions.Add(feedEntry);
                    }
                    else
                    {
                        var post = wallPostCacheRepo.GetItem(feedEntry.ReferencePostId);
                        if (post != null)
                        {
                            response.Add(new NewsfeedResponseItem
                            {
                                UserActions = new List<NewsfeedEntry> { feedEntry },
                                ReferencedPost = post
                            });
                        }
                        else
                        {
                            ignoreList.Add(feedEntry.ReferencePostId);
                        }
                    }
                }
            }
            return response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (feedCacheRepo != null)
                {
                    feedCacheRepo.Dispose();
                }
                if (wallPostCacheRepo != null)
                {
                    wallPostCacheRepo.Dispose();
                }
            }
        }
    }
}
