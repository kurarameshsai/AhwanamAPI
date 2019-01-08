using AhwanamAPI.Models;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class pathnerregisterController : ApiController
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();

        [HttpPost]
        [Route("api/pathnerregister/PRegister")]
        public IHttpActionResult PRegister([FromBody] registerdetails rgdetails)
        {
            string msg = "";
            UserLogin userLogin1 = new UserLogin();
            UserDetail userDetail = new UserDetail();
            Vendormaster vendorMaster = new Vendormaster();
            VendorVenue vendorVenue = new VendorVenue();
            VendorsPhotography vendorsPhotography = new VendorsPhotography();
            VendorsDecorator vendorsDecorator = new VendorsDecorator();
            VendorOthersService other = new VendorOthersService();
            //userLogin1.IPAddress = HttpContext.Request.UserHostAddress;
            //rgdetails.ActivationCode = Guid.NewGuid().ToString();
            userLogin1.ActivationCode = rgdetails.ActivationCode; //Guid.NewGuid().ToString();
            userLogin1.Status = "InActive";
            userLogin1.UserType = "Vendor";
            userDetail.FirstName = rgdetails.personname;
            userDetail.UserPhone = rgdetails.phoneno;
            userLogin1.Password = rgdetails.password;
            userLogin1.UserName = rgdetails.email;
            vendorMaster.BusinessName = rgdetails.businessname;
            vendorMaster.ContactPerson = rgdetails.personname;
            vendorMaster.ContactNumber = rgdetails.phoneno;
            vendorMaster.ServicType = rgdetails.servicetype;
            vendorMaster.EmailId = rgdetails.email;
            vendorMaster.BusinessType = rgdetails.businesstype;
            long data = userLoginDetailsService.GetLoginDetailsByEmail(userLogin1.UserName);
            if (data == 0)
            {
                int query = vendorMasterService.checkemail(vendorMaster.EmailId);
                if (query == 0)
                {
                    vendorMaster = venorVenueSignUpService.AddvendorMaster(vendorMaster);
                    vendorMaster.EmailId = rgdetails.email;
                    userLogin1 = venorVenueSignUpService.AddUserLogin(userLogin1);
                    userDetail.UserLoginId = userLogin1.UserLoginId;
                    userDetail = venorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
                    addservice(vendorMaster);
                    msg = "Success";  
                }
                else
                {
                    msg = "Failed";
                }
            }
            return Json(msg);

        }
        public int addservice(Vendormaster vendorMaster)
        {
            int count = 0;
            if (vendorMaster.ServicType == "Venue")
            {
                VendorVenue vendorVenue = new VendorVenue();
                vendorVenue.VendorMasterId = vendorMaster.Id;
                vendorVenue = venorVenueSignUpService.AddVendorVenue(vendorVenue);
                if (vendorVenue.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Catering")
            {
                VendorsCatering vendorsCatering = new VendorsCatering();
                vendorsCatering.VendorMasterId = vendorMaster.Id;
                vendorsCatering = venorVenueSignUpService.AddVendorCatering(vendorsCatering);
                if (vendorsCatering.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Photography")
            {
                VendorsPhotography vendorsPhotography = new VendorsPhotography();
                vendorsPhotography.VendorMasterId = vendorMaster.Id;
                vendorsPhotography = venorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                if (vendorsPhotography.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Decorator")
            {
                VendorsDecorator vendorsDecorator = new VendorsDecorator();
                vendorsDecorator.VendorMasterId = vendorMaster.Id;
                vendorsDecorator = venorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                if (vendorsDecorator.Id != 0) count++;
            }
            if (vendorMaster.ServicType == "Other")
            {
                VendorsOther vendorsOther = new VendorsOther();
                vendorsOther.VendorMasterId = vendorMaster.Id;
                vendorsOther.MinOrder = "0";
                vendorsOther.MaxOrder = "0";
                vendorsOther.Status = "InActive";
                vendorsOther.UpdatedBy = 2;
                vendorsOther.UpdatedDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                // vendorsOther.type = vendorMaster.;
                vendorsOther = venorVenueSignUpService.AddVendorOther(vendorsOther);
                if (vendorsOther.Id != 0) count++;
            }
            return count;
        }

        [HttpGet]
        [Route("api/pathnerregister/checkvendoremail")]
       public IHttpActionResult checkVendoremail(string email)
        {
            int query = vendorMasterService.checkemail(email);
            if (query == 0)
                return Json("email is not existed");
            else
                return Json("email is already existed select another email");
        }
    }
}
