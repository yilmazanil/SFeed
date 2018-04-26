using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Implementation;
using SFeed.Core.Models.Caching;

namespace SFeed.Business.Providers
{
    public class NewsfeedResponseProvider : INewsfeedReaderCacheRepository
    {
        INewsfeedReaderCacheRepository newsFeedResponseRepo;

        public NewsfeedResponseProvider() : this(
            new RedisNewsfeedResponseRepository()
            )
        {

        }
        public NewsfeedResponseProvider(INewsfeedReaderCacheRepository newsFeedResponseRepo)
        {
            this.newsFeedResponseRepo = newsFeedResponseRepo;
        }

        public IEnumerable<NewsfeedResponseModel> GetUserFeed(string userId, int skip, int take)
        {
            return newsFeedResponseRepo.GetUserFeed(userId, skip, take);
        }
    }
}
