using AhwanamAPI.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
   
    public class suppliersController : ApiController
    {
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mngvendorservice = new VendorDashBoardService();
        newmanageuser newmanageuse = new newmanageuser();

        ResultsPageService resultsPageService = new ResultsPageService();
        [HttpGet]
        [Route("api/suppliers/GetAllSuppliers")]
        public IHttpActionResult GetAllSuppliers()
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //string uid = user.UserId.ToString();
            string uid = userLogin.UserLoginId.ToString();
            string vemail = newmanageuse.Getusername(long.Parse(uid));
            vendorMaster = newmanageuse.GetVendorByEmail(vemail);
            string VendorId = vendorMaster.Id.ToString();
          var vendorlist= mngvendorservice.getvendor(VendorId);
            //string VendorId = "30059";
            var SupplierServicesLst = mngvendorservice.getsupplierservices(VendorId);
            return Json(vendorlist);
        }
    }
}
