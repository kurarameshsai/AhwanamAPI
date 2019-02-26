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
using System.Web.Http.Cors;

namespace AhwanamAPI.Controllers
{
    //[EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class homeController : ApiController
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
        VenorVenueSignUpService venorvenuesignupservice = new VenorVenueSignUpService();
        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public class services
        {
            public string name { get; set; }
            public int serviceId { get; set; }
            public string image { get; set; }
        }

        public class packages
        {
            public string name { get; set; }
            public string packageId { get; set; }
            public string description { get; set; }
            public price price { get; set; }
            public string imageUrl { get; set; }
            public string targetUrl { get; set; }
        }

        public class price
        {
            public long offer_price { get; set; }
            public long actual_price { get; set; }
            public string save_amount { get; set; }
            public string save_percentage { get; set; }
        }

        public class contact
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string event_date { get; set; }
            public string time { get; set; }
            public string description { get; set; }
        }

        //[HttpGet]
        //[Route("api/home/encrypt")]
        //public IHttpActionResult Encrypt(string val)
        //{
        //    encptdecpt encript = new encptdecpt();
        //    string encripted = encript.Encrypt(val);
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    if (val != null && val != "")
        //    {
        //        Dictionary<string, object> dict1 = new Dictionary<string, object>();
        //        dict.Add("status", true);
        //        dict.Add("message", "Success");
        //        dict1.Add("encrypt", encripted);
        //        dict.Add("data", dict1);
        //    }
        //    else
        //    {
        //        dict.Add("status", false);
        //        dict.Add("message", "Failed!!!");
        //    }
        //    return Json(dict);
        //}

        [HttpGet]
        [Route("api/home/encrypt")]
        public IHttpActionResult Decrypt(string val)
        {
            encptdecpt encript = new encptdecpt();
            string decrypted = encript.Decrypt(val);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict1.Add("encrypt", decrypted);
            dict.Add("data", dict1);
            return Json(dict);
        }

        [HttpGet]
        [Route("api/home/categories")]
        public IHttpActionResult GetVendorServicesList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            FilterServices filterServices = new FilterServices();
            var categories = filterServices.AllCategories();
            //string list = "Venue,Catering,Decorator,Photography,Pandit,Mehendi";
            List<services> res = new List<services>();
            for (int i = 0; i < categories.Count(); i++)
            {
                services result = new services();
                result.name = categories[i].name;
                result.serviceId = categories[i].servicType_id;
                result.image = categories[i].image;
                res.Add(result);
            }
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("results", res);
            //dict.Add("sort_options", true);
            return Json(dict);
        }

        [HttpGet]
        [Route("api/home/othercategories")]
        public IHttpActionResult GetVendorOtherServiceList([FromUri] string type)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<string> servicelist = new List<string> { "Venue", "Catering", "Decorator", "Photography", "Pandit", "Mehendi" };

            if (char.IsLower(type[0]))
            {
                var servicetype = char.ToUpper(type[0]) + type.Substring(1);
                var list = servicelist.Remove(servicetype);
            }
            else
            {
                var lst = servicelist.Remove(type);
            }
            List<services> res = new List<services>();
            for (int i = 0; i < servicelist.Count(); i++)
            {
                services result = new services();
                result.name = servicelist[i].Replace("-", " ");
                result.serviceId = i;
                result.image = "https://api.ahwanam.com/images/category.png";
                res.Add(result);
            }
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("results", res.Take(3));
            return Json(dict);
        }


        [HttpGet]
        [Route("api/home/deals")]
        public IHttpActionResult GetPackages()
        {
            List<packages> package = new List<packages>();
            packages pkg = new packages();
            pkg.name = "Silver Package";
            pkg.packageId = "P1";
            pkg.description = "The Silver Package is designed to be all inclusive.";
            price price = new price();
            price.offer_price = 16000000;
            price.actual_price = 18400000;
            price.save_amount = "2.4 Lakhs";
            price.save_percentage = "15";
            pkg.price = price;
            pkg.imageUrl = "https://api.ahwanam.com/images/package.png";
            pkg.targetUrl = "https://www.ahwanam.com/packages/wedding-gold-package/";
            package.Add(pkg);

            pkg = new packages();
            pkg.name = "Birthday Blast Package";
            pkg.packageId = "P2";
            pkg.description = "Birthday Blast Package is designed to be all inclusive.";
            price = new price();
            price.offer_price = 16000000;
            price.actual_price = 18400000;
            price.save_amount = "2.4 Lakhs";
            price.save_percentage = "15";
            pkg.price = price;
            pkg.imageUrl = "https://api.ahwanam.com/images/package.png";
            pkg.targetUrl = "https://www.ahwanam.com/packages/wedding-gold-package/";
            package.Add(pkg);

            pkg = new packages();
            pkg.name = "Golden Package";
            pkg.packageId = "P3";
            pkg.description = "The Golden Package is designed to be all inclusive.";
            price = new price();
            price.offer_price = 16000000;
            price.actual_price = 18400000;
            price.save_amount = "2.4 Lakhs";
            price.save_percentage = "15";
            pkg.price = price;
            pkg.imageUrl = "https://api.ahwanam.com/images/package.png";
            pkg.targetUrl = "https://www.ahwanam.com/packages/wedding-gold-package/";
            package.Add(pkg);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("results", package);
            return Json(dict);
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

        [HttpPost]
        [Route("api/home/savecontact")]
        public IHttpActionResult savecontact([FromBody]contact contact)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService enquiryService = new EnquiryService();
            Enquiry enquiry = new Enquiry();
            enquiry.PersonName = contact.name;
            enquiry.SenderPhone = contact.phone;
            enquiry.SenderEmailId = contact.email;
            //string date = contact.event_date + contact.time;
            //DateTime d1 = Convert.ToDateTime(contact.event_date);
            //d1.Add("time",contact.time);
            enquiry.EnquiryDate = DateTime.Parse(contact.event_date);
            enquiry.EnquiryDetails = contact.description;
            enquiry.EnquiryTitle = "Talk To Ahwanam";
            enquiry.EnquiryStatus = enquiry.Status = "Open";
            enquiry.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            string status = enquiryService.SaveEnquiries(enquiry);
            if (status == "Success")
                dict.Add("status", true);
            else
                dict.Add("status", true);
            dict.Add("message", status);
            return Json(dict);
        }
    }
}
