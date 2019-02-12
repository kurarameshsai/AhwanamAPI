using AhwanamAPI.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.Http.Cors;
using System.Collections;
using AhwanamAPI.Models;

namespace AhwanamAPI.Controllers
{
    public class UserAuthController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
        VenorVenueSignUpService venorvenuesignupservice = new VenorVenueSignUpService();
        cartservices cartserve = new cartservices();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorsCatering vendorsCatering = new VendorsCatering();
        VendorsDecorator vendorsDecorator = new VendorsDecorator();
        VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
        VendorsPhotography vendorsPhotography = new VendorsPhotography();
        VendorVenue vendorVenue = new VendorVenue();
        VendorsOther vendorsOther = new VendorsOther();
        VendorCateringService vendorCateringService = new VendorCateringService();
        VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        VendorOthersService vendorOthersService = new VendorOthersService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendormaster = new Vendormaster();

        public class outputresponse
        {
            public string status { get; set; }
            public string message { get; set; }
            public userdata userdata { get; set; }
        }

        public class userdata
        {
            public string email { get; set; }
            public string password { get; set; }
            public string phoneno { get; set; }
            public string name { get; set; }
        }

        public class loginresponse
        {
            public string status { get; set; }
            public string message { get; set; }
            public loginuser loginuser { get; set; }
        }

        public class loginuser
        {
            public string email { get; set; }
            public string password { get; set; }
            public string phoneno { get; set; }
            public string name { get; set; }
        }

        [HttpPost]
        [Route("api/UserAuth/login")]
        public IHttpActionResult login([FromBody]registerdetails details)//(UserLogin userlogin)
        {
            UserLogin userlogin = new UserLogin();
            userlogin.UserName = details.email;
            userlogin.Password = details.password;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            UserLogin data = new UserLogin();
            userdata user = new userdata();
            Dictionary<string, object> u1 = new Dictionary<string, object>();
            if (userlogin.UserName == null || userlogin.Password == null)
            {
                dict.Add("status", false);
                dict.Add("message", "Field missing");
                //loginuser loginuser = new loginuser();
                //loginuser.email = userlogin.UserName;
                //loginuser.password = userlogin.Password;
                u1.Add("user", null);
                dict.Add("data", u1);
                return Json(dict);
            }
            var userResponce = resultsPageService.GetUserLogin(userlogin);
            if (userResponce != null)
            {
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                var userdetails = userLoginDetailsService.GetUser((int)userResponce.UserLoginId);
                encptdecpt encrypt = new encptdecpt();
                string encrypted = encrypt.Encrypt(userResponce.UserName);
                dict.Add("status", true);
                dict.Add("message", "Login Success");
                loginuser loginuser = new loginuser();
                loginuser.email = userlogin.UserName;
                //loginuser.password = userlogin.Password;
                loginuser.name = userdetails.FirstName;
                loginuser.phoneno = userdetails.UserPhone;
                u1.Add("token", encrypted);
                u1.Add("user", loginuser);
                dict.Add("data", u1);
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "email and password doesnot match");
                //loginuser loginuser = new loginuser();
                //loginuser.email = userlogin.UserName;
                //loginuser.password = userlogin.Password;
                u1.Add("user", null);
                dict.Add("data", u1);
            }
            
            return Json(dict);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/UserAuth/register")]
        //public IHttpActionResult register(string customerphoneno, string customername, string password, string email)
        public IHttpActionResult register([FromBody]registerdetails details)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            userdata user = new userdata();
            Dictionary<string, object> u1 = new Dictionary<string, object>();
            if (details.name == null || details.email == null || details.phoneno == null || details.password == null)
            {
                dict.Add("status", false);
                dict.Add("message", "Field missing");
                //user.email = details.email;
                //user.name = details.personname;
                //user.password = details.password;
                //user.phoneno = details.phoneno;
                u1.Add("User", null);
                dict.Add("data", u1);
                return Json(dict);
            }
            //string msg = "";
            UserLogin userlogin = new UserLogin();
            UserDetail userdetail = new UserDetail();
            userlogin.ActivationCode = Guid.NewGuid().ToString();
            userdetail.FirstName = details.name;
            userdetail.UserPhone = details.phoneno;
            userdetail.AlternativeEmailID = details.email;
            userlogin.Password = details.password;
            userlogin.UserName = details.email;
            userlogin.Status = "InActive";
            var responce = "";
            userlogin.UserType = "User";
            long data = userlogindetailsservice.GetLoginDetailsByEmail(details.email);
            if (data == 0)
            { responce = userlogindetailsservice.AddUserDetails(userlogin, userdetail); }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Email already used");
                u1.Add("User", null);
            }
            if (responce == "sucess")
            {
                //msg = "success";
                string url = "http://api.ahwanam.com/api/home/ActivateEmail1?ActivationCode=" + userlogin.ActivationCode + "&&Email=" + userlogin.UserName;
                FileInfo File = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("/mailtemplate/welcome.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", Capitalise(userdetail.FirstName));
                readFile = readFile.Replace("[phoneno]", userdetail.UserPhone);
                TriggerEmail(userlogin.UserName, readFile, "Account Activation", null); // A Mail will be triggered
                //return Json(msg);
                dict.Add("status", true);
                dict.Add("message", "Successfully registered");
                user.email = details.email;
                user.name = details.name;
                user.password = details.password;
                user.phoneno = details.phoneno;
                u1.Add("User", user);
            }
            dict.Add("data", u1);
            return Json(dict);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/UserAuth/activateemail")]
        public IHttpActionResult activateemail(string activatecode, string email)
        {
            string msg = "";
            if (activatecode == "") { activatecode = null; }
            var userresponce = venorvenuesignupservice.GetUserdetails(email);
            if (activatecode == userresponce.ActivationCode)
            {
                msg = "success";
                return Json(msg);
            }
            msg = "failed";
            return Json(msg);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/UserAuth/ActivateEmail1")]
        public IHttpActionResult ActivateEmail1(string ActivationCode, string Email)
        {
            try
            {
                UserLogin userLogin = new UserLogin();
                UserDetail userDetails = new UserDetail();
                if (ActivationCode == "")
                { ActivationCode = null; }
                var userResponse = venorvenuesignupservice.GetUserdetails(Email);
                if (userResponse.Status != "Active")
                {
                    if (ActivationCode == userResponse.ActivationCode)
                    {
                        userLogin.Status = "Active";
                        userDetails.Status = "Active";
                        string email = userLogin.UserName;
                        var userid = userResponse.UserLoginId;
                        userlogindetailsservice.changestatus(userLogin, userDetails, (int)userid);
                        if (userResponse.UserType == "Vendor")
                        {
                            vendormaster = vendorMasterService.GetVendorByEmail(email);
                            string vid = vendormaster.Id.ToString();
                            if (vendormaster.ServicType == "Catering")
                            {
                                var catering = venorvenuesignupservice.GetVendorCatering(long.Parse(vid)).FirstOrDefault();
                                vendorsCatering.Status = vendormaster.Status = "Active";
                                vendorsCatering = vendorCateringService.activeCatering(vendorsCatering, vendormaster, long.Parse(catering.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendormaster.ServicType == "Decorator")
                            {
                                var decorators = venorvenuesignupservice.GetVendorDecorator(long.Parse(vid)).FirstOrDefault();
                                vendorsDecorator.Status = vendormaster.Status = "Active";
                                vendorsDecorator = vendorDecoratorService.activeDecorator(vendorsDecorator, vendormaster, long.Parse(decorators.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendormaster.ServicType == "Photography")
                            {
                                var photography = venorvenuesignupservice.GetVendorPhotography(long.Parse(vid)).FirstOrDefault();
                                vendorsPhotography.Status = vendormaster.Status = "Active";
                                vendorsPhotography = vendorPhotographyService.ActivePhotography(vendorsPhotography, vendormaster, long.Parse(photography.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendormaster.ServicType == "Venue")
                            {
                                var venues = venorvenuesignupservice.GetVendorVenue(long.Parse(vid)).FirstOrDefault();
                                vendorVenue.Status = vendormaster.Status = "Active";
                                vendorVenue = vendorVenueService.activeVenue(vendorVenue, vendormaster, long.Parse(venues.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendormaster.ServicType == "Other")
                            {
                                var others = venorvenuesignupservice.GetVendorOther(long.Parse(vid)).FirstOrDefault();
                                vendorsOther.Status = vendormaster.Status = "Active";
                                vendorsOther = vendorOthersService.activationOther(vendorsOther, vendormaster, long.Parse(others.Id.ToString()), long.Parse(vid));
                            }
                        }

                        return Json("Thanks for Verifying the Email");
                    }
                }
                else
                {
                    return Json("Your Account is already Verified Please login");
                }
                return Json("Email not found");
            }
            catch (Exception)
            {
                return Json("Error");
            }
        }

        public void TriggerEmail(string txtto, string txtmsg, string subject, HttpPostedFileBase attachment)
        {
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Wordpress_Email(txtto, txtmsg, subject, attachment);
        }

        #region References
        public IHttpActionResult checkAuthentication()
        {
            string name = "";
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userlogindetailsservice.GetUser((int)user.UserId);
                    if (userdata.FirstName != "" && userdata.FirstName != null)
                        name = userdata.FirstName;
                    else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                        name = "" + userdata.FirstName + " " + userdata.LastName + "";
                    else
                        name = userlogindetailsservice.GetUserId((int)user.UserId).UserName; //userdata.AlternativeEmailID;

                    var cart = cartserve.CartItemsList((int)user.UserId).Where(m => m.Status == "Active");

                    return Json(cart);
                }
            }
            return Json("error");
        }

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
        #endregion


        #region Password
        [AllowAnonymous]
        [HttpGet]
        [Route("api/UserAuth/updatepassword")]
        public IHttpActionResult updatepassword(string Email)
        {
            try
            {

                return Json(Email);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/UserAuth/changepassword")]

        public IHttpActionResult changepassword(UserLogin userLogin)
        {
            try
            {
                var userResponse = venorvenuesignupservice.GetUserdetails(userLogin.UserName);
                userlogindetailsservice.changepassword(userLogin, (int)userResponse.UserLoginId);
                var userdetails = userlogindetailsservice.GetUser(int.Parse(userResponse.UserLoginId.ToString()));
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/home";
                FileInfo File = new FileInfo(HttpContext.Current.Server.MapPath("/mailtemplate/change-email.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", Capitalise(userdetails.FirstName));
                TriggerEmail(userLogin.UserName, readFile, "Your Password is changed", null); // A mail will be triggered
                return Json("success");
            }
            catch (Exception)
            {
                return Json("Error");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/UserAuth/forgotpass")]
        public IHttpActionResult forgotpass(string Email)
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = Email;
            var userResponse = venorvenuesignupservice.GetUserLogdetails(userLogin);
            if (userResponse != null)
            {
                var userdetails = userlogindetailsservice.GetUser(int.Parse(userResponse.UserLoginId.ToString()));
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/Home/ActivateEmail?ActivationCode=" + userResponse.ActivationCode + "&&Email=" + Email;
                FileInfo File = new FileInfo(HttpContext.Current.Server.MapPath("/mailtemplate/mailer.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", Capitalise(userdetails.FirstName));
                TriggerEmail(Email, readFile, "Password reset information", null);// A mail will be triggered
                return Json("success");
            }
            return Json("success1");
        }
        #endregion

        //#region Signout
        //public IHttpActionResult SignOut()
        //{
        //    // Response.Cookies.Clear();
        //    FormsAuthentication.SignOut();
        //    return Json("logout");
        //}
        //#endregion
    }
}

