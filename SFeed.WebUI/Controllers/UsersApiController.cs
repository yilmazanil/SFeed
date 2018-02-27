using SFeed.Business.Services;
using SFeed.Model;
using System.Web.Http;

namespace SFeed.WebUI.Controllers
{
    [Route("api/users")]
    public class UsersApiController : ApiController
    {
        [Route("api/users/{username}")]
        public UserViewModel Get(string username)
        {
            using (var service = new UserService())
            {
                return service.GetUser(username);
            }
        }
    }
}
