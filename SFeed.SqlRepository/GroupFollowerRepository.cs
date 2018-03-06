using System.Linq;

namespace SFeed.SqlRepository
{
    public class GroupFollowerRepository : SqlRepositoryBase<GroupFollower>
    {
        public override void Add(GroupFollower entity)
        {
            if (!DbSet.Any(u => u.FollowerId == entity.FollowerId && u.GroupId == entity.GroupId))
            {
                DbSet.Add(entity);
            }
        }
    }
}
