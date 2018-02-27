using SFeed.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFeed.Data.RedisRepositories
{
    public class UserFollowerCacheRepository : RedisListRepositoryBase<int, int>
    {
        protected override string ListPrefix
        {
            get
            {
                return "userfeed";
            }
        }
    }
}
