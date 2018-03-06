using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFeed.SqlRepository
{
    public class WallPostLikeRepository : SqlRepositoryBase<WallPostLike>
    {
        public override void Add(WallPostLike entity)
        {
            if (!DbSet.Any(u => u.CreatedBy == entity.CreatedBy && u.WallPostId == entity.WallPostId))
            {
                DbSet.Add(entity);
            }
        }
    }
}
