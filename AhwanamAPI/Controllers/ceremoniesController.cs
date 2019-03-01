using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class ceremoniesController : ApiController
    {
        [HttpGet]
        [Route("api/ceremonies/details")]
        public IHttpActionResult ceremonydetails(string ceremony,string city)
        {
            CeremonyServices ceremonyServices = new CeremonyServices();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var eventname = ceremony.Split('_').ToList();
              eventname.RemoveAt(0);
            var details = ceremonyServices.getceremonydetails(Convert.ToInt64(eventname));
            VendorMasterService vendorMasterService = new VendorMasterService();
            var data = vendorMasterService.SearchVendors();
            var citylist = data.Select(m => m.City).Distinct().ToList();








            return Ok();
        }
    }
}
