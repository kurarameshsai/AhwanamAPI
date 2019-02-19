using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using System.Web.Http.Cors;


namespace AhwanamAPI.Controllers
{
    //[Authorize]
    public class viewserviceController : ApiController
    {
        viewservicesservice viewservicesss = new viewservicesservice();
        ResultsPageService resultsPageService = new ResultsPageService();

        [HttpGet]
        [Route("api/viewservice/gvbyname")]
        public IHttpActionResult getvendorbyname(string name, string type)
        {
            // Vendor Retrieval by Business Name
            GetVendors_Result vendor = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault();
            return Json(vendor);
        }

        [HttpGet]
        [Route("api/viewservice/gv")]
        public IHttpActionResult getvendor(string type, string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            // Vendor Retrieval by masterid & subid
            try
            {
                if (type == "Venue" || type == null)
                {
                    GetVendors_Result vendor = resultsPageService.GetAllVendors(type).Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
                    dict.Add("data", vendor);
                    //Ratings
                    decimal trating = (vendor.fbrating != null && vendor.googlerating != null && vendor.jdrating != null) ? decimal.Parse(vendor.fbrating) + decimal.Parse(vendor.googlerating) + decimal.Parse(vendor.jdrating) : 0;
                    dict.Add("rating", (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0");
                }
                else if (type == "Catering")
                {
                    GetCaterers_Result vendor = resultsPageService.GetAllCaterers().Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
                    dict.Add("data", vendor);
                }
                else if (type == "Decorator")
                {
                    GetDecorators_Result vendor = resultsPageService.GetAllDecorators().Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
                    dict.Add("data", vendor);
                }
                else if (type == "Photography")
                {
                    GetCaterers_Result vendor = resultsPageService.GetAllCaterers().Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
                    dict.Add("data", vendor);
                }
                else if (type == "Pandit")
                {
                    GetDecorators_Result vendor = resultsPageService.GetAllDecorators().Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
                    dict.Add("data", vendor);
                }

                // Gallery
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
                dict.Add("gallery", allimages);

                if (vid != null)
                {
                    //Particular Service Gallery
                    var allimages1 = viewservicesss.GetVendorAllImages(long.Parse(id)).Where(m => m.VendorId == long.Parse(vid)).ToList();
                    dict.Add("pgallery", allimages1);

                    //Food Packages
                    var fpkgs = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).Where(p => p.type == "Package").ToList();
                    dict.Add("food_packages", fpkgs);

                    //Rental Packages
                    var rpkgs = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).Where(p => p.type == "Rental").ToList();
                    dict.Add("rental_packages", rpkgs);

                    //Amenities
                    var amenities = Amenities(id, vid);
                    dict.Add("amenities", amenities);

                    //Policy
                    VendorMasterService vendorMasterService = new VendorMasterService();
                    var policy = vendorMasterService.Getpolicy(id, vid);
                    dict.Add("policy", policy);
                }
                
            }
            catch (Exception ex)
            {
                dict.Clear();
                dict.Add("status", false);
                dict.Add("message", "Failed");
            }
            return Json(dict);
        }

        [HttpGet]
        [Route("api/viewservice/gallery")]
        public IHttpActionResult vendorimages(string id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                // Retrieving all vendor images based on masterid
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
                dict.Add("data", allimages);
                return Json(dict);
            }
            catch (Exception)
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                return Json(dict);
            }
        }

        [HttpGet]
        [Route("api/viewservice/pgallery")]
        public IHttpActionResult particularimages(string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                // Retrieving particular service images based on masterid and subid
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).Where(m => m.VendorId == long.Parse(vid)).ToList();
                dict.Add("data", allimages);
                return Json(dict);
            }
            catch (Exception)
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                return Json(dict);
            }
        }

        [HttpGet]
        [Route("api/viewservice/fpkgs")]
        public IHttpActionResult foodpackages(string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                // Retrieving all food packages based on masterid & subid
                var allpkgs = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).Where(p => p.type == "Package").ToList();
                dict.Add("data", allpkgs);
                return Json(dict);
            }
            catch (Exception)
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                return Json(dict);
            }
        }

        [HttpGet]
        [Route("api/viewservice/rpkgs")]
        public IHttpActionResult rentalpackages(string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                // Retrieving all rental packages based on masterid & subid
                var allpkgs = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).Where(p => p.type == "Rental").ToList();
                dict.Add("data", allpkgs);
                return Json(dict);
            }
            catch (Exception)
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                return Json(dict);
            }
        }

        //[HttpGet]
        //[Route("api/viewservice/amenity")]
        public List<string> Amenities(string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            //try
            //{
                // Retrieving Available Amenities
                var venues = viewservicesss.GetVendorVenue(long.Parse(id)).Where(m => m.Id == long.Parse(vid)).ToList();
                var allamenities = venues.Where(m => m.Id == long.Parse(vid)).Select(m => new
                {
                    #region Venue amenities
                    m.AC,
                    m.TV,
                    m.Complimentary_Breakfast,
                    m.Geyser,
                    m.Parking_Facility,
                    m.Card_Payment,
                    m.Lift_or_Elevator,
                    m.Banquet_Hall,
                    m.Laundry,
                    m.CCTV_Cameras,
                    m.Swimming_Pool,
                    m.Conference_Room,
                    m.Bar,
                    m.Dining_Area,
                    m.Power_Backup,
                    m.Wheelchair_Accessible,
                    m.Room_Heater,
                    m.In_Room_Safe,
                    m.Mini_Fridge,
                    m.In_house_Restaurant,
                    m.Gym,
                    m.Hair_Dryer,
                    m.Pet_Friendly,
                    m.HDTV,
                    m.Spa,
                    m.Wellness_Center,
                    m.Electricity,
                    m.Bath_Tub,
                    m.Kitchen,
                    m.Netflix,
                    m.Kindle,
                    m.Coffee_Tea_Maker,
                    m.Sofa_Set,
                    m.Jacuzzi,
                    m.Full_Length_Mirrror,
                    m.Balcony,
                    m.King_Bed,
                    m.Queen_Bed,
                    m.Single_Bed,
                    m.Intercom,
                    m.Sufficient_Room_Size,
                    m.Sufficient_Washroom
                    #endregion
                }).ToList();
                List<string> famenities = new List<string>();
                foreach (var item in allamenities)
                {
                    string value = string.Join(",", item).Replace("{", "").Replace("}", "");
                    var availableamenities = value.Split(',');
                    value = "";
                    for (int i = 0; i < availableamenities.Length; i++)
                    {
                        if (availableamenities[i].Split('=')[1].Trim() == "Yes")
                            value = value + "," + availableamenities[i].Split('=')[0].Trim();
                    }
                    famenities.Add(value.TrimStart(','));
                }
                if (famenities.Count == 0) famenities.Add("No Amenities Available");
                //dict.Add("data", famenities);
                return famenities;
            //}
            //catch (Exception)
            //{
            //    dict.Add("status", false);
            //    dict.Add("message", "Failed");
            //    return Json(dict);
            //}
        }

        [HttpGet]
        [Route("api/viewservice/policy")]
        public IHttpActionResult policy(string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                // Retrieving Available Policies
                VendorMasterService vendorMasterService = new VendorMasterService();
                var policy = vendorMasterService.Getpolicy(id, vid);
                dict.Add("data", policy);
                return Json(dict);
            }
            catch (Exception)
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                return Json(dict);
            }
        }
    }
}
