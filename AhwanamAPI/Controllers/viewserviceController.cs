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
    public class viewserviceController : ApiController
    {
        viewservicesservice viewservicesss = new viewservicesservice();
        ResultsPageService resultsPageService = new ResultsPageService();

        [HttpGet]
        [Route("api/viewservice/gvbyname")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public IHttpActionResult getvendorbyname(string name, string type)
        {
            // Vendor Retrieval by Business Name
            GetVendors_Result vendor = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault();
            return Json(vendor);
        }

        [HttpGet]
        [Route("api/viewservice/gv")]
        [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
        public IHttpActionResult getvendor(string type, string id, string vid)
        {
            // Vendor Retrieval by masterid & subid
            GetVendors_Result vendor = resultsPageService.GetAllVendors(type).Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
            return Json(vendor);
        }

        [HttpGet]
        [Route("api/viewservice/gallery")]
        public IHttpActionResult vendorimages(string id)
        {
            // Retrieving all vendor images based on masterid
            var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
            return Json(allimages);
        }

        [HttpGet]
        [Route("api/viewservice/pgallery")]
        public IHttpActionResult particularimages(string id,string vid)
        {
            // Retrieving particular service images based on masterid and subid
            var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).Where(m=>m.VendorId == long.Parse(vid)).ToList();
            return Json(allimages);
        }

        [HttpGet]
        [Route("api/viewservice/fpkgs")]
        public IHttpActionResult foodpackages(string id,string vid)
        {
            // Retrieving all food packages based on masterid & subid
            var allpkgs = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).Where(p => p.type == "Package").ToList();
            return Json(allpkgs);
        }

        [HttpGet]
        [Route("api/viewservice/rpkgs")]
        public IHttpActionResult rentalpackages(string id, string vid)
        {
            // Retrieving all rental packages based on masterid & subid
            var allpkgs = viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(vid)).Where(p => p.type == "Rental").ToList();
            return Json(allpkgs);
        }

        [HttpGet]
        [Route("api/viewservice/amenity")]
        public IHttpActionResult Amenities(string id, string vid)
        {
            // Retrieving Available Amenities
            var venues = viewservicesss.GetVendorVenue(long.Parse(id)).Where(m=>m.Id == long.Parse(vid)).ToList();
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
            return Json(famenities);
        }

        [HttpGet]
        [Route("api/viewservice/policy")]
        public IHttpActionResult policy(string id, string vid)
        {
            // Retrieving Available Policies
            VendorMasterService vendorMasterService = new VendorMasterService();
            var policy = vendorMasterService.Getpolicy(id, vid);
            return Json(policy);
        }
    }
}
