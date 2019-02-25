using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using MaaAahwanam.Utility;
using Newtonsoft.Json;
using System.Web.Security;
using MaaAahwanam.Models;
using AhwanamAPI.Custom;

namespace AhwanamAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
           
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null)
                {
                    var serializeModel = JsonConvert.DeserializeObject<UserResponse>(authTicket.UserData);
                    var newUser = new CustomPrincipal(authTicket.Name)
                    {
                        UserId = serializeModel.UserLoginId,
                        FirstName = serializeModel.FirstName,
                        LastName = serializeModel.LastName,
                        UserType = serializeModel.UserType
                    };
                    HttpContext.Current.User = newUser;

                }
            }
        }
       }
}
