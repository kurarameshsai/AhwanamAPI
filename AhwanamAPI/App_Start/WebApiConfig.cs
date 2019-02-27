using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;


namespace AhwanamAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.EnableCors(new EnableCorsAttribute("http://localhost:3000", headers: "*", methods: "*"));
            config.EnableCors(new EnableCorsAttribute("https://api.ahwanam.com", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("localhost", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("http://52.66.202.144/", headers: "*", methods: "*"));
            //config.EnableCors(new EnableCorsAttribute("*", headers: "*", methods: "*"));
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
