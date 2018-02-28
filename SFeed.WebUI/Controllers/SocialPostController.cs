using SFeed.Business.Services;
using SFeed.Model;
using SFeed.Model.Presentation;
using SFeed.WebUI.UserProfile;
using System.Web.Mvc;

namespace SFeed.WebUI.Controllers
{
    public class SocialPostController : Controller
    {
        public ActionResult Create(SocialPostRequestModel request)
        {
            if (request.TargetUserId == 0)
            {
                request.TargetUserId = ActiveUser.Id;
            }
            var businessRequest = new SocialPostModel()
            {
                Body = request.Body,
                CreatedBy = request.TargetUserId
            };
            var postService = new SocialPostService();
            var createdPost =  postService.Create(businessRequest, request.TargetUserId);
            return Json(createdPost);
        }

    }
}