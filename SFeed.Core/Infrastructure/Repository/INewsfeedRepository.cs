using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface INewsfeedRepository
    {
        IEnumerable<NewsfeedEventModel> Generate(string userId);
    }
}
