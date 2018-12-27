using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class invoiceController : ApiController
    {
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        OrderdetailsServices orderdetailservices = new OrderdetailsServices();

        ReceivePaymentService rcvpaymentservice = new ReceivePaymentService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ResultsPageService resultsPageService = new ResultsPageService();
        UserLogin userLogin = new UserLogin();

        [HttpPost]
        [Route("api/invoice/getinvoice")]
        public IHttpActionResult getinvoice([FromUri] string oid)
        {
            List<sp_allvendoruserorddisplay_Result> orderdetails1 = new List<sp_allvendoruserorddisplay_Result>();
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //string uid = user.UserId.ToString();
            string uid = userLogin.UserLoginId.ToString();
            string vemail = newmanageuse.Getusername(long.Parse(uid));
            vendorMaster = newmanageuse.GetVendorByEmail(vemail);
            string VendorId = vendorMaster.Id.ToString();
            if (oid != null && oid != "")
            {
                orderdetails1 = newmanageuse.allOrderList().Where(m => m.orderid == long.Parse(oid)).ToList();
             
            }
            return Json(orderdetails1);
        }

        [HttpPost]
        [Route("api/invoice/getpayments")]
        public IHttpActionResult getpayments([FromUri] string oid)
        {
            List<Payment> payments = new List<Payment>();
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            //string uid = user.UserId.ToString();
            string uid = userLogin.UserLoginId.ToString();
            string vemail = newmanageuse.Getusername(long.Parse(uid));
            vendorMaster = newmanageuse.GetVendorByEmail(vemail);
            string VendorId = vendorMaster.Id.ToString();
            if (oid != null && oid != "")
            {
                 payments = rcvpaymentservice.getPayments(oid).ToList();

            }
            return Json(payments);
        }
    }
}
