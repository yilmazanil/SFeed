using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFeed.SqlRepository
{
    public class UserCommentLikeRepository : SqlRepositoryBase<UserCommentLike>
    {
        public override void Add(UserCommentLike entity)
        {
            if (!DbSet.Any(u => u.CreatedBy == entity.CreatedBy && u.CommentId == entity.CommentId))
            {
                DbSet.Add(entity);
            }
        }
    }
}
