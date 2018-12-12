using AhwanamAPI.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class resellerController : ApiController
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        viewservicesservice viewservicesss = new viewservicesservice();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        PartnerService partnerservice = new PartnerService();
        const string imagepath = @"/partnerdocs/";



        public IHttpActionResult Index(string partid)
        {
            reseller resell = new reseller();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
                string vid = vendorMaster.Id.ToString();

                var resellers = partnerservice.GetPartners(VendorId);
                var resellerspack = partnerservice.getPartnerPackage(VendorId);
                var pkgs = viewservicesss.getvendorpkgs(VendorId).ToList();
                List<string> pppl = new List<string>();
                List<PartnerPackage> p = new List<PartnerPackage>();
                List<SPGETNpkg_Result> p1 = new List<SPGETNpkg_Result>();
                if (partid != "" && partid != null)
                {
                    var partnercontact = partnerservice.getPartnercontact(VendorId).Where(m => m.PartnerID == partid).ToList();
                    var resellerspacklist = resellerspack.Where(m => m.PartnerID == long.Parse(partid)).ToList();
                    var pkglist = resellerspacklist.Select(m => m.packageid).ToList();
                    foreach (var item in pkgs)
                    {
                        if (pkglist.Contains(item.PackageID.ToString()))
                            p.AddRange(resellerspack.Where(m => m.packageid == item.PackageID.ToString()).ToList());
                        else
                            p1.AddRange(pkgs.Where(m => m.PackageID == item.PackageID));
                    }
                    resell.partcontact = partnercontact;
                    resell.PartnerPackage = p;
                    resell.p1 = p1;
                    resell.part = resellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();
                    resell.PartnerFile = partnerservice.GetFiles(vid, partid);
                }
                resell.p2 = pkgs;
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).ToList();
                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid));
                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid));
                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid));
                resell.GetVendorVenue = venues;
                resell.GetVendorCatering = catering;
                resell.GetVendorPhotography = photography;
                resell.GetVendorDecorator = decorators;
                resell.GetVendorOther = others;
               resell.partid = partid;

                return Json(resell);
            }
            else
            {
                return Json("please login");
            }
        }

        public class reseller
        {
            public Partner part {get;set;}
            public List<PartnerContact> partcontact {get;set;}
    public List<PartnerPackage> PartnerPackage { get; set; }
            public List<PartnerFile> PartnerFile { get; set; }
          public  List<SPGETNpkg_Result> p1 { get; set; }
            public List<SPGETNpkg_Result> p2 { get; set; }
            public List<VendorsCatering> GetVendorCatering { get; set; }
            public List<VendorsDecorator> GetVendorDecorator { get; set; }
            public List<VendorsEventOrganiser> GetVendorEventOrganiser { get; set; }
            public List<VendorsOther> GetVendorOther { get; set; }
            public List<VendorsPhotography> GetVendorPhotography { get; set; }
            public List<VendorVenue> GetVendorVenue { get; set; }
            public string partid { get; set; }
        }

        [HttpPost]
        public IHttpActionResult Index(Partner partner, string command, string partid)
        {

            var emailid1 = partner.emailid;
            var getpartner1 = partnerservice.getPartner(emailid1);
            if (command == "Update")
            {
                partner.UpdatedDate = DateTime.Now.Date;
                partner.ExpiryDate = DateTime.Now.Date;

                partner = partnerservice.UpdatePartner(partner, partid);
            }
            else if (command == "Update1")
            {

                partner.UpdatedDate = DateTime.Now.Date;
                partner.ExpiryDate = DateTime.Now.Date;

                partner = partnerservice.UpdatePartner(partner, partid);
            }
            else if (command == "save")
            {
                if (getpartner1 == null)
                {
                    partner.UpdatedDate = DateTime.Now.Date;
                    partner.ExpiryDate = DateTime.Now.Date;
                    partner.RegisteredDate = DateTime.Now.Date; partner = partnerservice.AddPartner(partner);

                    var emailid = partner.emailid;
                    var getpartner = partnerservice.getPartner(emailid);
                    return Json(getpartner);
                }
                else { return Json(""); }

            }


            return Json(getpartner1);

        }


        [HttpPost]
        public IHttpActionResult PartnerPackage(PartnerPackage partnerPackage, string command, string partid)
        {
            partnerPackage.RegisteredDate = DateTime.Now.Date;
            partnerPackage.UpdatedDate = DateTime.Now.Date;

            if (command == "save") { partnerPackage = partnerservice.addPartnerPackage(partnerPackage); }
            
            return Json(partnerPackage);
        }

        [HttpPost]
        public IHttpActionResult Contacts(PartnerContact Partnercontact, PartnerContact Partnercontact1, string command, string partid)
        {
            Partnercontact.RegisteredDate = DateTime.Now;
            Partnercontact.UpdatedDate = DateTime.Now;
            Partnercontact1.RegisteredDate = DateTime.Now;
            Partnercontact1.UpdatedDate = DateTime.Now;
            // Partnercontact.PartnerID = partid;
            if (command == "Update")
            {
                Partnercontact = partnerservice.UpdatePartnercontact(Partnercontact);
                Partnercontact = partnerservice.UpdatePartnercontact(Partnercontact1);
            }
               return Json(Partnercontact);
        }



        [HttpPost]
        public IHttpActionResult UploadContractdoc(HttpPostedFileBase helpSectionImages, string command, string partid, string vid)
        {

            string fileName = string.Empty;
            string filename = string.Empty;
            //VendorImage vendorImage = new VendorImage();
            //Vendormaster vendorMaster = new Vendormaster();
            PartnerFile partnerFile = new PartnerFile();
            if (helpSectionImages != null)
            {
                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                int imageno = 0;
                int imagecount = 2;
                var list = partnerservice.GetFiles(vid, partid);
                var resellers = partnerservice.GetPartners(vid);
                var reellers1 = resellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();
                var context = HttpContext.Current.Request;

                if (list.Count <= imagecount && context.Files.Count <= imagecount - list.Count)
                {
                    //getting max imageno
                    if (list.Count != 0)
                    {
                        string lastimage = list.OrderByDescending(m => m.FileName).FirstOrDefault().FileName;
                        var splitimage = lastimage.Split('_', '.');
                        imageno = int.Parse(splitimage[3]);
                    }
                    //Uploading images in db & folder
                    for (int i = 0; i < context.Files.Count; i++)
                    {
                        int j = imageno + i + 1;
                        var file1 = context.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            filename = reellers1.PartnerName + "_" + vid + "_" + partid + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            partnerFile.FileName = filename;
                            //vendorImage.ImageType = type;//"Slider";
                            partnerFile.VendorID = vid;
                            partnerFile.PartnerID = partid;
                            partnerFile.UpdatedDate = DateTime.Now;
                            partnerFile.RegisteredDate = DateTime.Now;

                            // vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                            partnerFile = partnerservice.addPartnerfile(partnerFile);
                        }
                    }
                }
            }
            return Json(filename);

           
        }


    }
}