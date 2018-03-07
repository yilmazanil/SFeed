using SFeed.Core.Models.Newsfeed;
using System.Linq;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<NewsfeedEntry>
    {
        protected override string ListPrefix => "userfeed";

        public override void RemoveFromList(string listKey, NewsfeedEntry item)
        {
            var listRef = GetAssociatedList(listKey);
            NewsfeedEntry refItem;

            if (item.EntryTypeId != (short)NewsfeedEntryType.wallpost)
            {
                 refItem = listRef.FirstOrDefault(p => p.By == item.By && p.EntryTypeId == item.EntryTypeId);
            }
            else
            {
                refItem = listRef.FirstOrDefault(p => p.ReferencePostId == item.ReferencePostId);
            }
            if (refItem != null)
            {
                listRef.RemoveValue(refItem);
            }
        }
    }
}
