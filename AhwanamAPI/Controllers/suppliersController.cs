using AhwanamAPI.Custom;
using AhwanamAPI.Models;
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
        UserLogin userLogin = new UserLogin();
        suppliersviewmodel listofdata = new suppliersviewmodel();


        [HttpGet]
        [Route("api/suppliers/GetAllSuppliers")]
        public IHttpActionResult GetAllSuppliers()
        {
           
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //string uid = user.UserId.ToString();
            string uid = userLogin.UserLoginId.ToString();
            string vemail = newmanageuse.Getusername(long.Parse(uid));
            vendorMaster = newmanageuse.GetVendorByEmail(vemail);
            string VendorId = vendorMaster.Id.ToString();
            listofdata.managevendor = mngvendorservice.getvendor(VendorId);
            listofdata.supplierservices = mngvendorservice.getsupplierservices(VendorId);
            //return Json(vendorlist);
            return Json(listofdata);
        }

        [HttpPost]
        [Route("api/suppliers/InsertOrEdit")]
        public IHttpActionResult InsertOrEdit([FromBody] ManageVendor mngvendor, [FromUri] string id, [FromUri] string command)
        {
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //string uid = user.UserId.ToString();
            string uid = userLogin.UserLoginId.ToString();
            string vemail = newmanageuse.Getusername(long.Parse(uid));
            vendorMaster = newmanageuse.GetVendorByEmail(vemail);
            string VendorId = vendorMaster.Id.ToString();
            mngvendor.vendorId = VendorId;
            string msg = string.Empty;
            mngvendor.registereddate = DateTime.Now;
            mngvendor.updateddate = DateTime.Now;
            if (command == "Save")
            {
                mngvendor = mngvendorservice.SaveVendor(mngvendor);
                msg = "Added New vendor";
            }
            else if (command == "Update")
            {
                mngvendor = mngvendorservice.UpdateVendor(mngvendor, int.Parse(id));
                msg = "Updated vendor";
            }
            return Json(msg);
        }
    }
}
