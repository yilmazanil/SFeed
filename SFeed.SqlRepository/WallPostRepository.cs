using System;
using System.Linq.Expressions;

namespace SFeed.SqlRepository
{
    public class WallPostRepository : SqlRepositoryBase<WallPost>
    {
        public override void Delete(Expression<Func<WallPost, bool>> where)
        {
            var items = base.GetMany(where);

            foreach (var item in items)
            {
                item.IsDeleted = true;
            }
        }

        public override void Delete(WallPost entity)
        {
            Delete(u => u.Id == entity.Id);
        }

        public override void Add(WallPost entity)
        {
            base.Add(entity);
        }
    }
}
