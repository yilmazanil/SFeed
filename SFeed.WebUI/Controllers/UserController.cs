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
            using (var service = new SocialPostService())
            {
                return Json(service.GetUserFeed(ActiveUser.Id), JsonRequestBehavior.AllowGet);
            }
        }

        [Route("user/follow/{userId}")]
        public ActionResult Follow(int userId)
        {
            using (var service = new UserService())
            {
                service.FollowUser(ActiveUser.Id, userId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        [Route("user/unfollow/{userId}")]
        public ActionResult UnFollow(int userId)
        {
            using (var service = new UserService())
            {
                service.UnFollowUser(ActiveUser.Id, userId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }
        [Route("user/followers/")]
        public ActionResult Followers()
        {
            using (var service = new UserService())
            {
                return Json(service.GetFollowers(ActiveUser.Id), JsonRequestBehavior.AllowGet);
            }
        }
    }
}