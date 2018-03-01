using SFeed.Business.Services;
using SFeed.Core.Models;
using SFeed.WebUI.UserProfile;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class WallEntryController : Controller
    {
        public ActionResult CreateEntry(WallEntryModel request, string targetUserId)
        {
            //If target user is not specified, user is posting his/her own wall}

            var blRequest = new WallEntryModel()
            {
                Body = request.Body,
                CreatedBy = ActiveUser.Username,
                EntryType = request.EntryType > 0 ? request.EntryType : (short)WallEntryTypeEnum.plaintext
            };

            var wallOwner = !string.IsNullOrWhiteSpace(targetUserId) ? targetUserId : ActiveUser.Username;

            var entryId = string.Empty;

            using (var wallEntryService = new WallEntryService())
            {
                entryId = wallEntryService.CreateEntry(blRequest);

                using (var userWallService = new UserWallService())
                {
                    userWallService.AddEntryToUserWall(entryId, wallOwner);

                    using (var followerService = new FollowerService())
                    {
                        var followers = followerService.GetFollowers(ActiveUser.Username);
                        if (!string.Equals(ActiveUser.Username, wallOwner))
                        {
                            var wallOwnerFollowers = followerService.GetFollowers(ActiveUser.Username);
                            followers = Enumerable.Union(followers, wallOwnerFollowers);
                        }
                        using (var feedService = new UserFeedService())
                        {
                            feedService.AddToUserFeeds(new FeedItemModel { EntryType = blRequest.EntryType, ReferenceId = entryId }, followers);
                        }

                    }
                }
            }

            return Json(entryId);


        }

        public ActionResult DeleteEntry(string postId)
        {
            using (var wallEntryService = new WallEntryService())
            {
                wallEntryService.Delete(postId);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}