using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System.Web;
using System.IO;
using MaaAahwanam.Models;
using System.Text;

namespace AhwanamAPI.Controllers
{
    public class homeController : ApiController
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
        VenorVenueSignUpService venorvenuesignupservice = new VenorVenueSignUpService();
        [HttpGet]
        [Route("api/home/services")]
        public IHttpActionResult GetVendorServicesList()
        {
            string list = "Venue,Catering,Decorator,Photography,Pandit,Mehendi";
            return Json(list.Split(','));
        }

        public class Quote
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string date { get; set; }
            public string city { get; set; }
            public string comments { get; set; }
            public string page { get; set; }
        }

        [HttpGet]
        [Route("api/home/sendquote")]
        public IHttpActionResult SendQuote(string name, string email, string phone, string page, string date = null, string city = null, string comments = null)
        {
            StringBuilder txtmsg = new StringBuilder();
            string txtto = "info@ahwanam.com,prabodh.dasari@xsilica.com";
            txtmsg.Append("Name:" + name + ",Email_ID:" + email + ",Phone Number:" + phone + "");
            if (page == "Wedding-Gold-Package") txtmsg = txtmsg.Append(",Selected Date:" + date + ",City:" + city + ",Comments:" + comments);
            string subject = "Get Quote from " + page + "";
            HttpPostedFileBase attachment = null;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Wordpress_Email(txtto, txtmsg.ToString().Replace(",", "<br/>"), subject, attachment);
            //emailSendingUtility.Email_maaaahwanam(txtto, txtmsg.ToString().Replace(",", "<br/>"), subject, attachment);
            return Json("Quote Sent Successfully");
        }

        [HttpPost]
        [Route("api/home/sendemail")]
        public IHttpActionResult Sendemail(string txtto, string txtmsg, string subject, HttpPostedFileBase attachment)
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = txtto;
            var userResponse = venorvenuesignupservice.GetUserLogdetails(userLogin);
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            var userdetails = userlogindetailsservice.GetUser(int.Parse(userResponse.UserLoginId.ToString()));
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/Home/ActivateEmail?ActivationCode=" + userResponse.ActivationCode + "&&Email=" + txtto;
            FileInfo File = new FileInfo(HttpContext.Current.Server.MapPath("/mailtemplate/mailer.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", Capitalise(userdetails.FirstName));
            txtmsg = readFile;
            if (emailSendingUtility != null)
            {
                emailSendingUtility.Email_maaaahwanam(txtto, txtmsg, subject, attachment);
                return Json("Success");
            }
            else
            {
                return Json("Failed");
            }

        }
        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
