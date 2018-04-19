using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class NewsfeedProviderTest : ProviderTestBase
    {
        IWallPostProvider userWallPostProvider;
        INewsfeedProvider userNewsfeedProvider;
        IFollowerProvider followerProvider;
        INewsfeedResponseProvider newsFeedResponseProvider;


        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new WallPostProvider();
            this.userNewsfeedProvider = new UserNewsfeedProvider();
            this.followerProvider = new FollowerProvider();
            this.newsFeedResponseProvider = new NewsfeedResponseProvider();
        }

        [TestMethod]
        public void Newsfeed_Should_Insert_NewWallPost_To_UserFeed_And_Check_Duplicate()
        {
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleUser = sampleUserWall.OwnerId;
            foreach (var user in RandomUserNames)
            {
                followerProvider.FollowUser(user, sampleUser);
            }

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = userWallPostProvider.AddPost(request);

            var newsFeedEntry = new NewsfeedItem
            {
                By = request.PostedBy,
                ReferencePostId = samplePostId,
                FeedType = NewsfeedType.wallpost,
                WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = sampleUserWall.OwnerId, WallOwnerType = sampleUserWall.WallOwnerType }
            };

            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);
            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);

            newsFeedEntry = new NewsfeedItem
            {
                By = request.PostedBy,
                ReferencePostId = samplePostId,
                FeedType = NewsfeedType.like,
                WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = sampleUserWall.OwnerId, WallOwnerType = sampleUserWall.WallOwnerType }
            };
            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);

            var feeds = new List<IEnumerable<NewsfeedWallPostModel>>();

            foreach (var user in RandomUserNames)
            {
                feeds.Add(newsFeedResponseProvider.GetUserNewsfeed(user, 0, 30));
            }



            userNewsfeedProvider.RemovePost(newsFeedEntry);
            //var shouldExist = wallOwnerFeeds.Any(p => p.ReferencedPost.Id == postId
            //&& p.UserActions.Any(t => t.FeedType == newsFeedEntry.FeedType && t.By == newsFeedEntry.By));

            //Assert.IsTrue(shouldExist);
        }

        //[TestMethod]
        //public void Newsfeed_Should_Delete_WallPost_From_UserFeed()
        //{
        //    var newPost = GetSampleWallCreateRequest();
        //    var postId = userWallPostProvider.AddPost(newPost);

        //    var newsFeedEntry = new NewsfeedItem
        //    {
        //        By = newPost.PostedBy,
        //        ReferencePostId = postId,
        //        FeedType = (short)NewsfeedType.wallpost,
        //        EventDate = DateTime.Now
        //    };


        //    userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);
        //    userNewsfeedProvider.RemoveNewsfeedItem(
        //        newsFeedEntry.By,
        //        p => p.FeedType == newsFeedEntry.FeedType
        //        && p.ReferencePostId == newsFeedEntry.ReferencePostId
        //        && p.By == newsFeedEntry.By);

        //    var wallOwnerFeeds = userNewsfeedProvider.GetUserNewsfeed(testWallOwnerId);
        //    var shouldNotExist = wallOwnerFeeds.Any(p => p.ReferencedPost.Id == postId
        //    && p.UserActions.Any(t => t.FeedType == newsFeedEntry.FeedType && t.By == newsFeedEntry.By));

        //    Assert.IsFalse(shouldNotExist);
        //}


        //[TestMethod]
        //public void Newsfeed_Should_Update_WallPost_In_UserFeed()
        //{

        //    var newPost = GetSampleWallCreateRequest();
        //    var postId = wallPostProvider.AddPost(newPost);

        //    var newsFeedEntry = new NewsfeedEntry
        //    {
        //        From = new Actor { Id = newPost.PostedBy.Id, ActorTypeId = (short)ActorType.user },
        //        ReferencePostId = postId,
        //        TypeId = (short)NewsfeedEntryType.wallpost,
        //        To = new Actor { Id = newPost.WallOwner.Id, ActorTypeId = (short)ActorType.user }
        //    };

        //    //Add and get feed
        //    userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);
        //    var wallOwnerFeeds = userNewsfeedProvider.GetUserNewsfeed(testWallOwnerId);
        //    var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ReferencePostId == postId && f.TypeId == (short)NewsfeedEntryType.wallpost);
        //    Assert.IsTrue(feedItem.ReferencePostId == postId);

        //    //Check if item is wallpost model
        //    var model = feedItem.ReferencedPost as WallPostCacheModel;
        //    Assert.IsNotNull(model);

        //    var wallPost = wallPostProvider.GetPost(model.Id);

        //    Update and refetch again
        //    wallPost.Body = "UpdatedBody";
        //    wallPostProvider.UpdatePost(wallPost);

        //    wallOwnerFeeds = userNewsfeedProvider.GetUserNewsfeed(testWallOwnerId);
        //    feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ReferencePostId == postId && f.TypeId == (short)NewsfeedEntryType.wallpost);
        //    Assert.IsTrue(string.Equals(wallPost.Body, feedItem.ReferencedPost.Body, StringComparison.OrdinalIgnoreCase));

        //}

        //[TestMethod]
        //public void Newsfeed_Should_Like_WallPost_In_UserFeed()
        //{

        //    var newsFeedAction = new NewsfeedEntry
        //    {
        //        TypeId = (short)NewsfeedEntryType.like,
        //        From = new Actor { ActorTypeId = (short)ActorType.user, Id = testUserId },
        //        ReferencePostId = Guid.NewGuid().ToString()
        //    };
        //    userNewsfeedProvider.AddNewsfeedItem(newsFeedAction);
        //    var wallOwnerFeeds = userNewsfeedProvider.GetUserNewsfeed(testWallOwnerId);

        //    var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.TypeId == (short)NewsfeedEntryType.like
        //    && f.From.Id == testUserId);
        //    Assert.IsTrue(feedItem.ReferencePostId == newsFeedAction.ReferencePostId);

        //}
    }
}
