using SFeed.Business.Services;
using SFeed.Core.Infrastructue.Services;
using SFeed.WebUI.UserProfile;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserFollowerService userFollowerService;

        public UserController()
        {
            this.userFollowerService = new UserFollowerService();
        }

        [Route("user/follow/{username}")]
        public ActionResult Follow(string userIdToFollow)
        {
            userFollowerService.FollowUser(ActiveUser.Username, userIdToFollow);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [Route("user/unfollow/{username}")]
        public ActionResult UnFollow(string userIdToUnfollow)
        {
            userFollowerService.UnFollowUser(ActiveUser.Username, userIdToUnfollow);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [Route("user/followers/{userId}")]
        public ActionResult Followers(string userId)
        {
           userId = !string.IsNullOrWhiteSpace(userId) ? userId : ActiveUser.Username;
           return Json(userFollowerService.GetFollowers(userId), JsonRequestBehavior.AllowGet);
        }

        [Route("user/newsfeed/")]
        public ActionResult Feed()
        {
            using (var service = new UserNewsfeedService())
            {
                return Json(service.GetUserNewsfeed(ActiveUser.Username), JsonRequestBehavior.AllowGet);
            }
        }
      
    }
}