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
    //[EnableCors(origins: "https://api.ahwanam.com", headers: "*", methods: "*")]
    public class homeController : ApiController
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
        VenorVenueSignUpService venorvenuesignupservice = new VenorVenueSignUpService();
        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
     
        public class services
        {
            public string name { get; set; }
            public string page_name { get; set; }
            public int category_id { get; set; }
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
            //public int ID { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string event_date { get; set; }
            public string time { get; set; }
            public string description { get; set; }
            public string city { get; set; }
            public string origin { get; set; }
            public string[] services { get; set; }
        }

        public class eventslist
        {
            public string ceremony_id { get; set; }
            public string ceremony_name { get; set; }
            public string thumb_image { get; set; }
            public string short_description { get; set; }
            public string page_name { get; set; }
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

        public class comments
        {
            public long commentid { get; set; }
            public string comment { get; set; }
        }
        public class socialcontact
        {
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string event_date { get; set; }
            public string description { get; set; }
            public string city { get; set; }
            public string enquiry_Regards { get; set; }
            public string leadsource { get; set; }
            public string status { get; set; }
            public string event_name { get;set; }
            public string enquiry_date { get; set; }
            public string campaign { get; set; }
        }

        //public class fbcontact
        //{
        //    public string name { get; set; }
        //    public string email { get; set; }
        //    public string phone { get; set; }
        //    public string event_date { get; set; }
        //    public string description { get; set; }
        //    public string city { get; set; }
        //    public string enquiry_Regards { get; set; }
        //    public string leadsource { get; set; }
        //    public string status { get; set; }
        //    public string event_name { get; set; }
        //    public string enquiry_date { get; set; }
        //    public string campaign { get; set; }
        //}


        [HttpGet]
            [Route("api/message")]
         public IHttpActionResult message()
        {
            return Json("success");
        }
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
                result.page_name = categories[i].display_name;
                result.category_id = categories[i].servicType_id;
                result.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages/" + categories[i].image;
                res.Add(result);
            }
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("results", res);
            //dict.Add("sort_options", true);
            return Json(dict);
        }

        [HttpGet]
        [Route("api/category/relatedcategories")]
        public IHttpActionResult GetVendorOtherServiceList([FromUri] long category_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            FilterServices filterServices = new FilterServices();
            var categories = filterServices.AllCategories().Where(m => m.servicType_id != category_id).Take(3).ToList();
            List<services> res = new List<services>();
            for (int i = 0; i < categories.Count(); i++)
            {
                services result = new services();
                result.name = categories[i].name;
                result.page_name = categories[i].display_name;
                result.category_id = categories[i].servicType_id;
                result.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages/" + categories[i].image;
                res.Add(result);
            }
            dict.Add("status", true);
            dict.Add("message", "Success");
            Dictionary<string, object> d1 = new Dictionary<string, object>();
            d1.Add("results", res);
            dict.Add("data", d1);
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
            pkg.imageUrl = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "package.png";
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
            pkg.imageUrl = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "package.png";
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
            pkg.imageUrl = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "package.png";
            pkg.targetUrl = "https://www.ahwanam.com/packages/wedding-gold-package/";
            package.Add(pkg);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("results", package);
            return Json(dict);
        }

        [HttpGet]
        [Route("api/home/ceremonies")]
        public IHttpActionResult events()
        {
            CeremonyServices ceremonyServices = new CeremonyServices();
            var list = ceremonyServices.Getall();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<eventslist> c1 = new List<eventslist>();
            for (int i = 0; i < list.Count; i++)
            {
                eventslist c = new eventslist();
                c.ceremony_id = list[i].Id.ToString();
                c.ceremony_name = list[i].Title;
                c.thumb_image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "ceremonies_images/" + list[i].Image;
                c.short_description = list[i].Description;
                c.page_name = list[i].page_name;
                c1.Add(c);
            }
            Dictionary<string, object> d1 = new Dictionary<string, object>();
            d1.Add("results", c1);
            dict.Add("status",true);
            dict.Add("data", d1);
            return Json(dict);
        }

        //[HttpGet]
        //[Route("api/home/diffceremonies")]
        //public IHttpActionResult ceremonybsdtype(string type)
        //{
        //    CeremonyServices ceremonyServices = new CeremonyServices();
        //    int ceremonytype = Convert.ToInt32(type);
        //    var list = ceremonyServices.Getallbasedtype(ceremonytype);
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    List<eventslist> c1 = new List<eventslist>();
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        eventslist c = new eventslist();
        //        c.ceremony_id = list[i].Id.ToString();
        //        c.ceremony_name = list[i].Title;
        //        c.thumb_image = list[i].Image;
        //        c.short_description = list[i].Description;
        //        c.page_name = list[i].page_name;
        //        c1.Add(c);
        //    }
        //    Dictionary<string, object> d1 = new Dictionary<string, object>();
        //    d1.Add("results", c1);
        //    dict.Add("status", true);
        //    dict.Add("data", d1);
        //    return Json(dict);
        //}

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

        public class comment
        {
            public string admin_comment { get; set; }
            public long enquiry_id { get; set; }
            public long user_id { get; set; }
        }

        public class enquirystatus
        {
            public long enquiry_id { get; set; }
            public string status { get; set; }
        }

        [HttpGet]
        [Route("api/getmsg")]
        public IHttpActionResult get()
        {
            string msg = "Hii";
            return Json(msg);
        }

        [HttpGet]
        [Route("api/GetEnquiry")]
        public IHttpActionResult GetEnquiry(long enquiry_id)
        {
            EnquiryService es = new EnquiryService();
            var data = es.getenquiry(enquiry_id);
            return Json(data);

        }

        [HttpPost]
        [Route("api/savecomment")]
        public IHttpActionResult savecomment(comment cmnt)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService es = new EnquiryService();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            Enquirycomment ecmnt = new Enquirycomment();
            ecmnt.comment = cmnt.admin_comment;
            ecmnt.enquiryid = cmnt.enquiry_id;
            ecmnt.userid = 121;
            ecmnt.commentedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            ecmnt.UpdatedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var data = es.saveenquirycomment(ecmnt);
            if(data!=null)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                dict.Add("result", data);
            }
           else
            {
                dict.Add("status", false);
                dict.Add("message", "failed");
                dict.Add("result", null);
            }
            return Json(dict);

        }

        [HttpGet]
        [Route("api/getcomments")]
        public IHttpActionResult getcomments(long enquiry_id)
        { 
            EnquiryService es = new EnquiryService();
            var data = es.Getcomment(enquiry_id).OrderByDescending(c=>c.commenId).ToList();
            return Json(data);

        }

        [HttpPost]
        [Route("api/updatecomment")]
        public IHttpActionResult updatecomment(comments cmnt)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService es1 = new EnquiryService();
            var data = es1.updatecomment(cmnt.commentid, cmnt.comment);
            if (data == "success")
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "failed");
            }
            return Json(dict);

        }

        [HttpDelete]
        [Route("api/deletecomment")]
        public IHttpActionResult deletecomment(long commentid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService es1 = new EnquiryService();
            int data = es1.Removecomment(commentid);
            if (data != 0)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "failed");
            }
            return Json(dict);

        }


        [HttpPost]
        [Route("api/updatestatus")]
        public IHttpActionResult Updatestatus(enquirystatus es)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService es1 = new EnquiryService();
            var data = es1.UpdateEnquriystatus(es.enquiry_id, es.status);
            if(data == "success")
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "failed");
            }

            return Json(dict);

        }


        [HttpGet]
        [Route("api/generalenquiries")]
        public IHttpActionResult GetallEnquiries()
        {
            EnquiryService es = new EnquiryService();
            var datalist = es.getallenquires().Where(a => a.Services == null).ToList();       
            return Json(datalist);
        }

        [HttpGet]
        [Route("api/Serviceenquiries")]
        public IHttpActionResult GetserviceEnquiries()
        {
            EnquiryService es = new EnquiryService();
            var datalist = es.getallenquires().Where(a=>a.Services!=null).ToList();
            return Json(datalist);
        }

        [HttpGet]
        [Route("api/googleleads")]
        public IHttpActionResult Getgoogleleads()
        {
            EnquiryService es = new EnquiryService();
            var datalist = es.getgoogleleads().ToList();
            return Json(datalist);
        }

        [HttpGet]
        [Route("api/fbleads")]
        public IHttpActionResult Getfbleads()
        {
            EnquiryService es = new EnquiryService();
            var datalist = es.getfblead().ToList();
            return Json(datalist);
        }

        [HttpGet]
        [Route("api/alluserdetails")]
        public IHttpActionResult GetUserdetailsforadmin()
        {
            EnquiryService es = new EnquiryService();
            var data = es.Getuserdataforadmin();
            return Json(data);
        }

        [HttpGet]
        [Route("api/allwishlist")]
        public IHttpActionResult GetUserwishlistdetailsforadmin()
        {
            EnquiryService es = new EnquiryService();
            var data = es.Getwishlistdataforadmin();
            return Json(data);
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

        public void TriggerEmail(string txtto, string txtmsg, string subject, HttpPostedFileBase attachment)
        {
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Wordpress_Email(txtto, txtmsg, subject, attachment);
        }


        [HttpPost]
        [Route("api/home/googlecontact")]
        public IHttpActionResult googlecontact([FromBody]socialcontact contact)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService enquiryService = new EnquiryService();
            Googlelead gcontact = new Googlelead();
            gcontact.Name = contact.name;
            gcontact.email = contact.email;
            gcontact.phoneno = contact.phone;
            gcontact.city = contact.city;
            gcontact.status = contact.status;
            gcontact.leadsource = contact.leadsource;
            gcontact.Event = contact.event_name;
            gcontact.EnquiryRegarding = contact.enquiry_Regards;
            gcontact.description = contact.description;
            if(!string.IsNullOrEmpty(contact.event_date))
            {
                if(CheckDate(contact.event_date) == true)
                {
                    gcontact.EventDate = DateTime.Parse(contact.event_date);
                }
            }
            if(!string.IsNullOrEmpty(contact.enquiry_date))
            {
                if(CheckDate(contact.enquiry_date) == true)
                {
                    gcontact.EnquiryDate = DateTime.Parse(contact.enquiry_date);
                }
            }
            gcontact.AddedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            gcontact.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var data = enquiryService.addgooglelead(gcontact);
            if(data!=null)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
            }
            return Json(dict);
        }

        [HttpPost]
        [Route("api/home/fbcontact")]
        public IHttpActionResult fbcontact([FromBody]socialcontact contact)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService enquiryService = new EnquiryService();
            Facebooklead fcontact = new Facebooklead();
            fcontact.Name = contact.name;
            fcontact.email = contact.email;
            fcontact.phoneno = contact.phone;
            fcontact.city = contact.city;
            fcontact.status = contact.status;
            fcontact.leadsource = contact.leadsource;
            fcontact.Event = contact.event_name;
            fcontact.EnquiryRegarding = contact.enquiry_Regards;
            fcontact.description = contact.description;
            fcontact.campaign = contact.campaign;
            if (!string.IsNullOrEmpty(contact.event_date))
            {
                if (CheckDate(contact.event_date) == true)
                {
                    fcontact.EventDate = DateTime.Parse(contact.event_date);
                }
            }
            if (!string.IsNullOrEmpty(contact.enquiry_date))
            {
                if (CheckDate(contact.enquiry_date) == true)
                {
                    fcontact.EnquiryDate = DateTime.Parse(contact.enquiry_date);
                }
            }
            fcontact.AddedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            fcontact.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var data = enquiryService.addfblead(fcontact);
            if (data != null)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
            }
            return Json(dict);
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
            enquiry.city = contact.city;
            enquiry.originfromurl = contact.origin;
           if(!string.IsNullOrEmpty(contact.event_date))
            {
               if( CheckDate(contact.event_date)==true )
                {
                    enquiry.EnquiryDate = DateTime.Parse(contact.event_date);
                }
            }
            enquiry.EnquiryDetails = contact.description;
            enquiry.EnquiryTitle = "Talk To Ahwanam";
            enquiry.EnquiryStatus = enquiry.Status = "Open";
            var s= (contact.services == null) ? null : string.Join(",", contact.services);
            enquiry.Services = s;
            enquiry.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var status = enquiryService.SaveallEnquiries(enquiry);
            var msg = sendemail(contact.name, contact.email);      
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            FileInfo File = new FileInfo(HttpContext.Current.Server.MapPath("/mailtemplate/talktoahwanam.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[name]", Capitalise(contact.name));
            readFile = readFile.Replace("[phoneno]", contact.phone);
            readFile = readFile.Replace("[email]", contact.email);
            readFile = readFile.Replace("[city]", contact.city);
            readFile = readFile.Replace("[origin]", contact.origin);
            readFile = readFile.Replace("[decription]", contact.description);
            readFile = readFile.Replace("[event]", contact.event_date);
            readFile = readFile.Replace("[service]", enquiry.Services);
            string txtmsg = readFile;
            string subj = "Knots&Vows User Information";
            string targetmails = "lakshmi.p@xsilica.com"; //sandbox
            //string targetmails = "nivita.priya@xsilica.com"; //prod
            emailSendingUtility.Email_maaaahwanam(targetmails, txtmsg, subj, null);
            if (status != null)
            {
                
               if(msg == "suceess")
                { 
                dict.Add("status", true);
            dict.Add("message", "Success");
                }
                else
                {   
                    dict.Add("status", false);
                    dict.Add("message", "Failed");
                }
            }
         
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
            }
               
            return Json(dict);
        }

        public string sendemail(string name, string email)
        {
            string msg;
            try { 
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            //FileInfo File = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("/mailtemplate/thankyou.html")); //for prod
          FileInfo File = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("/mailtemplate/sandboxthankyou.html"));  // for sandbox        
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[username]", Capitalise(name));
                string txtto = email;
                string subject = "Thank you for choosing Knots&Vows";
               string txtmsg = readFile;
                HttpPostedFileBase attachment = null;
            emailSendingUtility.Wordpress_Email(txtto, txtmsg, subject, attachment);
                //emailSendingUtility.testEmail_maaaahwanam(txtto, txtmsg, subject, attachment);
                msg = "suceess";
            }
            catch(Exception ex)
            {
                msg = "failed";
            }
            return msg;
        }

        [HttpPost]
        [Route("api/home/saveadminenqry")]
        public IHttpActionResult saveenquiryadmin([FromBody]contact contact)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService enquiryService = new EnquiryService();
            Enquiry enquiry = new Enquiry();
            enquiry.PersonName = contact.name;
            enquiry.SenderPhone = contact.phone;
            enquiry.SenderEmailId = contact.email;
            enquiry.city = contact.city;
            enquiry.originfromurl = contact.origin;
            if (!string.IsNullOrEmpty(contact.event_date))
            {
                if (CheckDate(contact.event_date) == true)
                {
                    enquiry.EnquiryDate = DateTime.Parse(contact.event_date);
                }
            }
            enquiry.EnquiryDetails = contact.description;
            enquiry.EnquiryTitle = "Talk To Ahwanam";
            enquiry.EnquiryStatus = enquiry.Status = "Open";
            var s = (contact.services == null) ? null : string.Join(",", contact.services);
            enquiry.Services = s;
            enquiry.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var status = enquiryService.SaveallEnquiries(enquiry);
            if (status != null)
            {
                    dict.Add("status", true);
                    dict.Add("message", "Success");
            }

            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
            }

            return Json(dict);
        }

        [HttpPost]
        [Route("api/home/updateadminenqry")]
        public IHttpActionResult updatenquiryadmin([FromBody]contact contact,long enquiery_id)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            EnquiryService enquiryService = new EnquiryService();
            Enquiry enquiry = new Enquiry();
            enquiry.PersonName = contact.name;
            enquiry.SenderPhone = contact.phone;
            enquiry.SenderEmailId = contact.email;
            enquiry.city = contact.city;
            if (!string.IsNullOrEmpty(contact.event_date))
            {
                if (CheckDate(contact.event_date) == true)
                {
                    enquiry.EnquiryDate = DateTime.Parse(contact.event_date);
                }
            }
            enquiry.EnquiryDetails = contact.description;
            var s = (contact.services == null) ? null : string.Join(",", contact.services);
            enquiry.Services = s;
            enquiry.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var status = enquiryService.updateGnrlLead(enquiry, enquiery_id);
            if (status != null)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }

            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
            }

            return Json(dict);

        }

       public bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
