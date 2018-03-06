using System;
using System.Linq.Expressions;

namespace SFeed.SqlRepository
{
    public class CommentRepository : SqlRepositoryBase<UserComment>
    {
        public override void Delete(Expression<Func<UserComment, bool>> where)
        {
            var items = base.GetMany(where);

            foreach (var item in items)
            {
                item.IsDeleted = true;
            }
        }

        public override void Delete(UserComment entity)
        {
            Delete(u => u.Id == entity.Id);
        }
    }
}
