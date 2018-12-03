using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using Newtonsoft.Json;

namespace AhwanamAPI.Controllers
{
    public class VendorAuthController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        [HttpGet]
        public IHttpActionResult Authenticate()
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            return Json(userLogin.UserName);
        }
    }
}
