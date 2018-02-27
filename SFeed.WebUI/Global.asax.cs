using SFeed.WebUI.App_Start;
using System.Web;
using System.Web.Http;

namespace SFeed.WebUI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Configure();
        }
    }
}
