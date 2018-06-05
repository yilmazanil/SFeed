using SFeed.Core.Models.GroupWall;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface IGroupWallRepository
    {
        void RefreshGroupWall(string groupId, IEnumerable<string> postIds);
        void ClearGroupWall(string groupId);
        IEnumerable<GroupWallResponseModel> GetGroupWall(string groupId);
    }
}
