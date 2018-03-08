using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisFixedListRepositoryBase<NewsfeedEntry>
    {
        public override string ListName => RedisNameConstants.FeedRepoPrefix;

        private int listSize = 0;

        public override int ListSize
        {
            get
            {
                return listSize >0 ? listSize :  RedisNameConstants.FeedRepoSize;
            }
            set
            {
                listSize = value;
            }
        }
    }
}
