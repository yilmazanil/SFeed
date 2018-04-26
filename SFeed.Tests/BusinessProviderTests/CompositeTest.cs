using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Wall;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Tests.BusinessProviderTests
{
    public class FollowersSample
    {
        public WallModel WallOwner { get; set; }
        public string Follower { get; set; }
        public string Publisher { get; set; }
        public string PublisherFollower { get; set; }
    }
    [TestClass]
    public class CompositeTest : ProviderTestBase
    {
        IWallPostProvider wallPostProvider;
        IFollowerProvider followerProvider;
        INewsfeedProvider newsFeedProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.wallPostProvider = new WallPostProvider();
            this.followerProvider = new FollowerProvider();
            this.newsFeedProvider = new NewsfeedProvider();
        }

        [TestMethod]
        public void Should_Post_And_Get_Newsfeed()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var users = ManageFollowers();

            stopWatch.Stop();
            Trace.WriteLine(string.Format("Followers initialized : {0} ms", stopWatch.ElapsedMilliseconds));

            ManageWallPost(users, stopWatch);
        }

        private void ManageWallPost(FollowersSample users, Stopwatch stopWatch)
        {
            Trace.WriteLine("");
            stopWatch.Restart();
            var randomNumber = new Random().Next(10);

            for (int i = 0; i < randomNumber; i++)
            {
                var sampleRequest = GetSampleWallCreateRequest(users.Publisher, users.WallOwner);
                var samplePostId = wallPostProvider.AddPost(sampleRequest);

                Trace.WriteLine(string.Format("CreatedPost - {0}, {1}", samplePostId, JsonConvert.SerializeObject(sampleRequest)));
                var newsFeedEntry = new NewsfeedItem
                {
                    By = sampleRequest.PostedBy,
                    ReferencePostId = samplePostId,
                    FeedType = NewsfeedActionType.wallpost,
                    WallOwner = new NewsfeedWallModel { IsPublic = true, OwnerId = users.WallOwner.OwnerId, WallOwnerType = WallType.user }
                };
                newsFeedProvider.AddNewsfeedItem(newsFeedEntry);

            }
            Trace.WriteLine(string.Format("{0} posts {1}'s wall, NumberOfPosts : {2}", users.Publisher, users.WallOwner.OwnerId, randomNumber));
            stopWatch.Stop();
            Trace.WriteLine(string.Format("Posted (Db & Cache) {0} items : {1} ms", randomNumber, stopWatch.ElapsedMilliseconds));
            Trace.WriteLine("");


            stopWatch.Restart();
            var feedResult = newsFeedProvider.GetUserNewsfeed(users.Follower, 0, 100);
            Trace.WriteLine(string.Format("Feed of {0} contains {1} items", users.Follower, feedResult.Count()));
            stopWatch.Stop();
            Trace.WriteLine(string.Format("GetFeed : {0} ms", stopWatch.ElapsedMilliseconds));

            stopWatch.Restart();
            feedResult = newsFeedProvider.GetUserNewsfeed(users.PublisherFollower, 0, 100);
            Trace.WriteLine(string.Format("Feed of {0} contains {1} items", users.PublisherFollower, feedResult.Count()));
            stopWatch.Stop();
            Trace.WriteLine(string.Format("GetFeed : {0} ms", stopWatch.ElapsedMilliseconds));

            stopWatch.Restart();
            feedResult = newsFeedProvider.GetUserNewsfeed(users.Publisher, 0, 100);
            Trace.WriteLine(string.Format("Feed of {0} contains {1} items", users.Publisher, feedResult.Count()));
            stopWatch.Stop();
            Trace.WriteLine(string.Format("GetFeed : {0} ms", stopWatch.ElapsedMilliseconds));

            stopWatch.Restart();
            feedResult = newsFeedProvider.GetUserNewsfeed(users.WallOwner.OwnerId, 0, 100);
            Trace.WriteLine(string.Format("Feed of {0} contains {1} items", users.WallOwner.OwnerId, feedResult.Count()));
            stopWatch.Stop();
            Trace.WriteLine(string.Format("GetFeed : {0} ms", stopWatch.ElapsedMilliseconds));
            Trace.WriteLine("");


            feedResult = newsFeedProvider.GetUserNewsfeed(users.Follower, 0, 100);
            Trace.WriteLine(string.Format("Feed of {0}", users.Follower));
            foreach (var post in feedResult)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(post));
            }
            Trace.WriteLine("");

            stopWatch.Restart();
            var wallPostResult = wallPostProvider.GetUserWall(users.WallOwner.OwnerId, DateTime.Now, 100);

            
            foreach (var post in wallPostResult)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(post));
            }
            Trace.WriteLine(string.Format("Fetched {0} items from Wall of {1} : {2} ms", wallPostResult.Count(), users.Follower, stopWatch.ElapsedMilliseconds));
            Trace.WriteLine("");
            stopWatch.Stop();

            stopWatch.Restart();
            var wallPostDetailedResult = wallPostProvider.GetUserWallDetailed(users.WallOwner.OwnerId, DateTime.Now, 100);
              
            foreach (var post in wallPostResult)
            {
                Trace.WriteLine(JsonConvert.SerializeObject(post));
            }
            stopWatch.Stop();
            Trace.WriteLine(string.Format("Fetched {0} items from Wall of {1} : {2} ms", wallPostDetailedResult.Count(), users.Follower, stopWatch.ElapsedMilliseconds));

        }

        private FollowersSample ManageFollowers()
        {
            var result = new FollowersSample();
            //Define sample user
            result.WallOwner = GetRandomUserWallOwner(true);
            var wallOwner = result.WallOwner.OwnerId;

            Trace.WriteLine(string.Concat("WallOwner: ", wallOwner));

            result.Follower = GetRandomUserName();
            Trace.WriteLine(string.Concat("Follower: ", result.Follower));

            result.Publisher = GetRandomUserName();
            Trace.WriteLine(string.Concat("Publisher: ", result.Publisher));

            result.PublisherFollower = GetRandomUserName();
            Trace.WriteLine(string.Concat("PublisherFollower: ", result.PublisherFollower));

            followerProvider.FollowUser(result.Follower, wallOwner);
            followerProvider.FollowUser(result.PublisherFollower, result.Publisher);

            Trace.WriteLine(string.Format("{0} follows {1}, {2} follows {3}", result.Follower, wallOwner, result.PublisherFollower, result.Publisher));

            return result;
        }

        [TestMethod]
        public void Should_Read_Wall_And_Newsfeed()
        {
            var userId = "Yoshie";
            var userId2 = "Willy";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var posts1 = wallPostProvider.GetUserWallDetailed(userId, DateTime.Now, 100000);
            var posts2 = wallPostProvider.GetUserWallDetailed(userId2, DateTime.Now, 100000);

            Trace.WriteLine(string.Format("Fetched total number of {0} records for 2 different users as wallposts in {1} ms", posts1.Count() + posts2.Count(), stopWatch.ElapsedMilliseconds));
            stopWatch.Restart();
            userId = "Karla";
            var feed = newsFeedProvider.GetUserNewsfeed(userId, 0 , 1000);
            stopWatch.Stop();

            Trace.WriteLine(string.Format("Fetched total number of {0} records from newsfeed in {1} ms", feed.Count(), stopWatch.ElapsedMilliseconds));

        }
    }
}
