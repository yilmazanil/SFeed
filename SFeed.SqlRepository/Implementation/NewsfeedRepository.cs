using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.SqlRepository.Implementation
{
    public class NewsfeedRepository : INewsfeedRepository
    {
        public IEnumerable<NewsfeedEventModel> Generate(string userId)
        {
            using (var context = new SocialFeedEntities())
            {
                var result = context.GetInitialFeedPosts(userId);
                return MapInitialFeed(result);
            }
        }

        private IEnumerable<NewsfeedEventModel> MapInitialFeed(IEnumerable<GetInitialFeedPosts_Result> procResult)
        {
            var retVal = new List<NewsfeedEventModel>();
            foreach (var record in procResult)
            {
                var model = new NewsfeedEventModel();
                model.ReferencePostId = record.Id;
                model.By = record.EventBy;
                model.ActionType = (NewsfeedActionType)Enum.Parse(typeof(NewsfeedActionType), record.EventDetail);
                retVal.Add(model);
            }
            return retVal;
        }
    }
}
