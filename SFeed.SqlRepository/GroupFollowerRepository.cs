using System.Linq;

namespace SFeed.SqlRepository
{
    public class GroupFollowerRepository : SqlRepositoryBase<GroupFollower>
    {
        public override void Add(GroupFollower entity)
        {
            if (!dbSet.Any(u => u.FollowerId == entity.FollowerId && u.GroupId == entity.GroupId))
            {
                dbSet.Add(entity);
            }
        }
    }
}
