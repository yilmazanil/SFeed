using SFeed.Business.Services;
using SFeed.Core.Models;
using SFeed.WebUI.UserProfile;
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

            using (var userWallService = new UserWallService())
            {
                var createdPostId = userWallService.PublishEntryToUserWall(blRequest, wallOwner);
                return Json(createdPostId);
            } 
        }

        public ActionResult DeleteEntry(string postId)
        {
            using (var wallEntryService = new WallEntryService() )
            {
                wallEntryService.Delete(postId);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}