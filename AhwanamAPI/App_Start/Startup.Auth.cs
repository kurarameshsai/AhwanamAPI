using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using AhwanamAPI.Providers;



namespace AhwanamAPI
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        static Startup()

        {
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/token"),
            //    Provider = new OAuthProvider(),
            //    //AccessTokenExpireTimeSpan = TimeSpan.FromDays(100),
            //    AccessTokenExpireTimeSpan=TimeSpan.FromMinutes(15),
            //    AllowInsecureHttp = true
            //};
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}