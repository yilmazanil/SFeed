using SFeed.Business.Services;
using SFeed.Core.Infrastructue.Services;
using SFeed.WebUI.UserProfile;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class NewsfeedController : Controller
    {
        IUserNewsfeedService newsFeedService;

        public NewsfeedController()
        {
            this.newsFeedService = new UserNewsfeedService();
        }

        [Route("user/newsfeed/")]
        public ActionResult GetFeed()
        {
            return Json(newsFeedService.GetUserFeed(ActiveUser.Username), JsonRequestBehavior.AllowGet);
        }
    }
}