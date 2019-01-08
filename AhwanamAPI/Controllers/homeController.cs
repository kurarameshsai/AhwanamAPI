using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System.Web;

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

        [HttpGet]
        [Route("api/home/sendquote")]
        public IHttpActionResult SendQuote(string name, string email, string phone,string page)
        {
            string txtto = "info@ahwanam.com,prabodh.dasari@xsilica.com";
            string txtmsg = "Name:"+name+",Email ID:"+email+",Phone Number:"+phone+"";
            string subject = "Get Quote from "+page+"";
            HttpPostedFileBase attachment = null;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Wordpress_Email(txtto, txtmsg, subject, attachment);
            return Json("Quote Sent Successfully");
        }
    }
}
