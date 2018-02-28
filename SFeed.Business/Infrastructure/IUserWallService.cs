using SFeed.Model;
using System.Collections;
using System.Collections.Generic;

namespace SFeed.Business.Infrastructure
{
    public interface IUserWallService
    {
        IEnumerable<SocialPostModel> GetUserWall(int userId);
        long PublishToUserWall(int wallUserId, SocialPostModel entry);
        void Delete(long postId);
    }
}
