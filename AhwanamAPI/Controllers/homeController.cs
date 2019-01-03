using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Service;

namespace AhwanamAPI.Controllers
{
    public class homeController : ApiController
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        [HttpGet]
        [Route("api/home/services")]
        public IHttpActionResult GetVendorServicesList()
        {
            string list = "Venue,Catering,Decorator,Photography,Pandit,Mehendi";
            return Json(list.Split(','));
        }
    }
}
