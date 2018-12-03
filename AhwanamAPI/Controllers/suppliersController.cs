using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AhwanamAPI.Custom;


namespace AhwanamAPI.Controllers
{
    
    public class suppliersController : ApiController
    {
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mngvendorservice = new VendorDashBoardService();
        newmanageuser newmanageuse = new newmanageuser();

        [HttpGet]
        [AllowAnonymous]

        [Route("api/suppliers/GetAllSuppliers/{VendorId}")]
        public IHttpActionResult GetAllSuppliers(string VendorId)
        {
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //string uid = user.UserId.ToString();
            //string vemail = newmanageuse.Getusername(long.Parse(uid));
            //vendorMaster = newmanageuse.GetVendorByEmail(vemail);
            //VendorId = vendorMaster.Id.ToString();
            var SupplierServicesLst = mngvendorservice.getsupplierservices(VendorId);
            return Ok(SupplierServicesLst);
        }
    }
}
