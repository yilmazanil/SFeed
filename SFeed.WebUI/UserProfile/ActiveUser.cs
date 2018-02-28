using SFeed.Business.Services;
using SFeed.Model;
using System;
using System.Web;

namespace SFeed.WebUI.UserProfile
{
    public static class ActiveUser
    {
        public static string Username => HttpContext.Current.User.Identity.Name;

        public static int Id
        {
            get
            {
                if (HttpContext.Current.Session["CurrentUserId"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["CurrentUserId"]);
                }
                else
                {
                    UserService service = new UserService();
                    var user = service.GetUser(Username);
                    if (user == null)
                    {
                        user = new UserModel() { Username = Username };
                        user.Id = service.AddUser(user);
                        if (user.Id <= 0)
                        {
                            throw new Exception("Could not create or fetch user data");
                        }
                    }
                    HttpContext.Current.Session["CurrentUser"] = user;
                    HttpContext.Current.Session["CurrentUserId"] = user.Id;
                    return user.Id;
                }
            }

        }

    }
}