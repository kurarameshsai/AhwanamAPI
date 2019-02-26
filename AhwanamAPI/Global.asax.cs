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
     
       }
}
