using System;
using System.Linq.Expressions;

namespace SFeed.SqlRepository
{
    public class WallEntryRepository : SqlRepositoryBase<WallEntry>
    {
        public override void Delete(Expression<Func<WallEntry, bool>> where)
        {
            var items = base.GetMany(where);

            foreach (var item in items)
            {
                item.IsDeleted = true;
            }
        }
    }
}
