using SFeed.Business.Services;
using SFeed.Model;
using SFeed.Model.Presentation;
using SFeed.WebUI.UserProfile;
using System.Net;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class SocialPostController : Controller
    {
        public ActionResult Create(SocialPostRequestModel request)
        {
            //If target user is not specified, user is posting his/her own wall
            if (request.TargetUserId == 0)
            {
                request.TargetUserId = ActiveUser.Id;
            }

            var blRequest = new SocialPostModel()
            {
                Body = request.Body,
                CreatedBy = ActiveUser.Id
            };

            using (var postService = new SocialPostService())
            { 
                var createdPost = postService.Create(blRequest, request.TargetUserId);
                return Json(createdPost);
            } 
        }

        public ActionResult Delete(long postId)
        {
            var wallService = new UserWallService();
            wallService.Delete(postId);

            var feedService = new UserFeedService();
            feedService.DeleteFromFeeds(postId);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}