using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Implementation;
using SFeed.Core.Models.Caching;

namespace SFeed.Business.Providers
{
    public class NewsfeedResponseProvider : INewsfeedResponseProvider
    {
        INewsfeedResponseCacheRepository newsFeedResponseRepo;

        public NewsfeedResponseProvider() : this(
            new RedisNewsfeedResponseRepository()
            )
        {

        }
        public NewsfeedResponseProvider(INewsfeedResponseCacheRepository newsFeedResponseRepo)
        {
            this.newsFeedResponseRepo = newsFeedResponseRepo;
        }
        public IEnumerable<NewsfeedWallPostModel> GetUserNewsfeed(string userId, int skip, int take)
        {
            return newsFeedResponseRepo.GetUserFeed(userId, skip, take);
        }
    }
}
