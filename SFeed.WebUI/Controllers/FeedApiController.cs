using SFeed.Business.Services;
using SFeed.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace SFeed.WebUI.Controllers
{
    [Route("api/feeds")]
    public class FeedApiController : ApiController
    {
        public IEnumerable<SocialPostModel> Get()
        {
            using (var service = new SocialPostService())
            {
                return service.GetUserFeed(1);
            }
        }
    }
}
