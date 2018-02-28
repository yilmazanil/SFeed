using SFeed.Business.Services;
using SFeed.WebUI.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Feed()
        {
            using (var service = new SocialPostService())
            {
                return Json(service.GetUserFeed(ActiveUser.Id), JsonRequestBehavior.AllowGet);
            }
        }
        [Route("users/follow/{userId}")]
        public ActionResult Follow(int userId)
        {
            using (var service = new UserService())
            {
                service.FollowUser(ActiveUser.Id, userId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        [Route("users/unfollow/{userId}")]
        public ActionResult UnFollow(int userId)
        {
            using (var service = new UserService())
            {
                service.UnFollowUser(ActiveUser.Id, userId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public ActionResult Followers()
        {
            using (var service = new UserService())
            {
                return Json(service.GetFollowers(ActiveUser.Id), JsonRequestBehavior.AllowGet);
            }
        }
    }
}