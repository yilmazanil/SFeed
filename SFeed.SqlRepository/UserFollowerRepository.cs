using System.Linq;

namespace SFeed.SqlRepository
{
    public class UserFollowerRepository : SqlRepositoryBase<UserFollower>
    {
        public override void Add(UserFollower entity)
        {
            if (!dbSet.Any(u => u.FollowerId == entity.FollowerId && u.UserId == entity.UserId));
            {
                dbSet.Add(entity);
            }
        }
    }
}
