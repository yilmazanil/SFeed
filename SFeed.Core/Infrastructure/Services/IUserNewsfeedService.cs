﻿using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserNewsfeedService : IDisposable
    {
        IEnumerable<NewsfeedItemModel> GetUserFeed(string userId);
    }
}
