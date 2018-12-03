using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace AhwanamAPI.Controllers
{
    public class VendorAuthController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        [HttpGet]
        public UserLogin Authenticate()
        {
            UserLogin userLogin = new UserLogin();
            userLogin = resultsPageService.GetUserLogin(userLogin);
            return userLogin;
        }
    }
}
