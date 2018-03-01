using SFeed.Business.Services;
using SFeed.WebUI.UserProfile;
using System.Net;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class UserController : Controller
    {
        [Route("user/Feed/")]
        public ActionResult Feed()
        {
            using (var service = new UserFeedService())
            {
                return Json(service.GetUserFeed(ActiveUser.Username), JsonRequestBehavior.AllowGet);
            }
        }

        [Route("user/follow/{userId}")]
        public ActionResult Follow(string userIdToFollow)
        {
            using (var service = new FollowerService())
            {
                service.FollowUser(ActiveUser.Username, userIdToFollow);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        [Route("user/unfollow/{userId}")]
        public ActionResult UnFollow(string userIdToUnfollow)
        {
            using (var service = new FollowerService())
            {
                service.UnFollowUser(ActiveUser.Username, userIdToUnfollow);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }
        [Route("user/followers/")]
        public ActionResult Followers()
        {
            using (var service = new FollowerService())
            {
                return Json(service.GetFollowers(ActiveUser.Username), JsonRequestBehavior.AllowGet);
            }
        }
    }
}