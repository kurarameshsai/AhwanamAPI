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
using System.Web.Http.Cors;


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
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
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

        //[HttpPost]
        //[Route("api/suppliers/InsertOrEdit")]
        //public IHttpActionResult InsertOrEdit([FromBody] ManageVendor mngvendor, [FromUri] string id, [FromUri] string command)
        //{
        //    userLogin.UserName = "Sireesh.k@xsilica.com";
        //    userLogin.Password = "ksc";
        //    userLogin = resultsPageService.GetUserLogin(userLogin);
        //    //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
        //    //string uid = user.UserId.ToString();
        //    string uid = userLogin.UserLoginId.ToString();
        //    string vemail = newmanageuse.Getusername(long.Parse(uid));
        //    vendorMaster = newmanageuse.GetVendorByEmail(vemail);
        //    string VendorId = vendorMaster.Id.ToString();
        //    mngvendor.vendorId = VendorId;
        //    string msg = string.Empty;
        //    mngvendor.registereddate = DateTime.Now;
        //    mngvendor.updateddate = DateTime.Now;
        //    if (command == "Save")
        //    {
        //        mngvendor = mngvendorservice.SaveVendor(mngvendor);
        //        msg = "Added New vendor";
        //    }
        //    else if (command == "Update")
        //    {
        //        mngvendor = mngvendorservice.UpdateVendor(mngvendor, int.Parse(id));
        //        msg = "Updated vendor";
        //    }
        //    return Json(msg);
        //}
        [HttpPost]
        [Route("api/suppliers/Insertsuppliers")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public IHttpActionResult Insertsuppliers([FromBody] ManageVendor mngvendor)
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
            //string msg = string.Empty;
            mngvendor.registereddate = DateTime.Now;
            mngvendor.updateddate = DateTime.Now;
            mngvendor = mngvendorservice.SaveVendor(mngvendor);
            return Json(mngvendor);
        }
        [HttpPost]
        [Route("api/suppliers/Getsuppliersbyid")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

        public IHttpActionResult Getsuppliersbyid([FromUri] string id)
        {
            var data = mngvendorservice.getvendorbyid(int.Parse(id));
            return Json(data);
        }

        [HttpPost]
        [Route("api/suppliers/updatesuppliers")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

        public IHttpActionResult updatesuppliers([FromBody] ManageVendor mngvendor,[FromUri] string id)
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
            //string msg = string.Empty;
            mngvendor.registereddate = DateTime.Now;
            mngvendor.updateddate = DateTime.Now;
            mngvendor = mngvendorservice.UpdateVendor(mngvendor, int.Parse(id));
            return Json(mngvendor);
        }
        
        [HttpPost]
        [Route("api/suppliers/checkVendoremail")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

        public IHttpActionResult checkVendoremail([FromUri] string email, [FromUri] string VendorId)
        {
            int query = mngvendorservice.checkvendoremail(email, VendorId);
            if (query == 0)
                return Json("success");
            else
                return Json("email is already existed select another email");
        }

        [HttpPost]
        [Route("api/suppliers/InsertsuppliersServices")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

        public IHttpActionResult InsertsuppliersServices([FromBody] AllSupplierServices supplierservices)
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
            supplierservices.VendorMasterID = VendorId;
            supplierservices.UpdatedDate = DateTime.Now;
            supplierservices = mngvendorservice.AddSupplierServices(supplierservices);
            return Json(supplierservices);
        }

        [HttpPost]
        [Route("api/suppliers/Getsupplierservice")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public IHttpActionResult Getsupplierservice([FromUri] string id)
        {
            var data = mngvendorservice.getsuplierservicesbyid(Convert.ToInt32(id));
            return Json(data);
        }
        [HttpPost]
        [Route("api/suppliers/UpdatesuppliersServices")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public IHttpActionResult UpdatesuppliersServices([FromBody] AllSupplierServices supplierservices, [FromUri] string id)
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
            supplierservices.VendorMasterID = VendorId;
            supplierservices.UpdatedDate = DateTime.Now;
            supplierservices = mngvendorservice.updatesupplierservices(supplierservices, Convert.ToInt32(id));
            return Json(supplierservices);
        }

        [HttpPost]
        [Route("api/suppliers/checksupplierservices")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]

        public IHttpActionResult checksupplierservices([FromUri] string servicename, [FromUri] string vid)
        {
            int services = mngvendorservice.checksupplierservices(servicename, vid);
            if (services == 0)
            {
                return Json("success");
            }
            else { return Json("service is already existed"); }
        }
    }
}
