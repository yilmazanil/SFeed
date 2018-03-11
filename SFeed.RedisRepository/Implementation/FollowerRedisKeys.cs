using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFeed.RedisRepository.Implementation
{
    public class FollowerRedisKeys
    {
        public string PostFeedKey { get; set; }
        public string PostActionsKey { get; set; }
        public string PostId { get; set; }
    }
}
