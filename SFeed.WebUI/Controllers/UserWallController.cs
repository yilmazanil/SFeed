using SFeed.Business.Services;
using SFeed.Core.Infrastructue.Services;
using SFeed.Core.Models;
using SFeed.WebUI.UserProfile;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class WallEntryController : Controller
    {
        IUserWallEntryService wallEntryService;

        public WallEntryController()
        {
            this.wallEntryService = new UserWallEntryService();
        }

        [Route("user/posts/new")]
        public ActionResult CreateEntry(WallEntryModel request, string targetUserId)
        {
            var blRequest = new WallEntryModel()
            {
                Body = request.Body,
                CreatedBy = ActiveUser.Username,
                EntryType = request.EntryType > 0 ? request.EntryType : (short)WallEntryTypeEnum.plaintext
            };
            //If target user is not specified, user is posting his/her own wall}
            var wallOwner = !string.IsNullOrWhiteSpace(targetUserId) ? targetUserId : ActiveUser.Username;
            var entryId = wallEntryService.CreatePost(blRequest, wallOwner);
            return Json(entryId);
        }

        [Route("user/posts/{postId}")]
        public ActionResult GetEntry(string postId)
        {
            return Json(wallEntryService.GetPost(postId), JsonRequestBehavior.AllowGet);
        }

        [Route("user/posts/delete/{id}")]
        public ActionResult DeleteEntry(string postId)
        {
            wallEntryService.DeletePost(postId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [Route("user/posts/update/")]
        public ActionResult UpdateEntry(WallEntryModel request)
        {
            wallEntryService.UpdatePost(request);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Route("user/wall/")]
        public ActionResult GetUserWall()
        {
            return Json(wallEntryService.GetUserWall(ActiveUser.Username), JsonRequestBehavior.AllowGet);
        }

        [Route("user/wall/{userId}")]
        public ActionResult GetUserWall(string userId)
        {
            return Json(wallEntryService.GetUserWall(userId), JsonRequestBehavior.AllowGet);
        }



    }
}