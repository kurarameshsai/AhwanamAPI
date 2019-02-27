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

        public class vendordetail
        {
            public string terms_and_condition_url { get; set; }
            public string cancelation_policy_url { get; set; }
            public string cover_image { get; set; }
            public string page_name { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public string rating { get; set; }
            public string address { get; set; }
            public location location { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public price price { get; set; }
            public string about { get; set; }
            public List<areas> available_areas { get; set; }
            public List<amenities> amenities { get; set; }
            public List<policies> policies { get; set; }
            public List<gallery> gallery { get; set; }
            //Add gallery property
        }

        public class location
        {
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class price
        {
            public string actual_price { get; set; }
            public string offer_price { get; set; }
            public string service_price { get; set; }
        }

        public class areas
        {
            public string name { get; set; }
            public string seating_capacity { get; set; }
            public string type { get; set; }
        }

        public class amenities
        {
            public string name { get; set; }
            public string icon_url { get; set; }
        }

        public class policies
        {
            public string name { get; set; }
        }

        public class gallery
        {
            public string url { get; set; }
            //public string album_id { get; set; }
        }

        public class Reviews
        {
            public string name { get; set; }
            public string review { get; set; }
            public string rating { get; set; }
        }

        [HttpGet]
        [Route("api/viewservice/gvbyname")]
        public IHttpActionResult getvendorbyname(string name, string type)
        {
            // Vendor Retrieval by Business Name
            GetVendors_Result vendor = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault();
            return Json(vendor);
        }


        [HttpGet]
        [Route("api/details")]
        public IHttpActionResult productdetails(string type, string param)
        {
            string id = null;
            string vid = null;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            if (type == "venue")
            {
                //type =(type == "venue") ? "Venue" : type;
                GetVendors_Result vendor = resultsPageService.GetAllVendors("Venue").Where(m => m.page_name == param).FirstOrDefault();
                id = vendor.Id.ToString();
                vid = vendor.subid.ToString();
                //GetVendors_Result vendor = resultsPageService.GetAllVendors(type).Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).FirstOrDefault();
                vendordetail vdetail = new vendordetail();
                vdetail.cover_image = (vendor.logo != null || vendor.logo != "") ? vendor.logo : vendor.image;
                vdetail.page_name = vendor.page_name;
                vdetail.name = vendor.BusinessName;
                vdetail.category_name = vendor.ServicType;
                ReviewService reviewService = new ReviewService();
                vdetail.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m=>m.Sid == long.Parse(vid)).Count().ToString();
                decimal trating = (vendor.fbrating != null && vendor.googlerating != null && vendor.jdrating != null) ? decimal.Parse(vendor.fbrating) + decimal.Parse(vendor.googlerating) + decimal.Parse(vendor.jdrating) : 0;
                vdetail.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                vdetail.address = vendor.Address + "," + vendor.Landmark + "," + vendor.City + "," + vendor.State;
                location loc = new location();
                if (vendor.GeoLocation != null && vendor.GeoLocation != "")
                {
                    loc.latitude = vendor.GeoLocation.Split(',')[0];
                    loc.longitude = vendor.GeoLocation.Split(',')[1];
                }
                else
                {
                    loc.latitude = "17.385044";
                    loc.longitude = "78.486671";
                }
                vdetail.location = loc;
                //vdetail.charge_type=
                vdetail.city = vendor.City;
                price p = new price();
                p.actual_price = vendor.cost1.ToString();
                p.offer_price = vendor.normaldays; // change this price as per the date
                p.service_price = vendor.ServiceCost.ToString();
                vdetail.price = p;
                vdetail.about = vendor.Description;
                VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
                var subservices = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList();
                List<areas> a1 = new List<areas>();
                for (int i = 0; i < subservices.Count; i++)
                {
                    areas area = new areas();
                    area.name = subservices[i].name;
                    area.seating_capacity = subservices[i].Minimumseatingcapacity.ToString();
                    area.type = subservices[i].VenueType;
                    a1.Add(area);
                }
                vdetail.available_areas = a1;

                List<amenities> amenity1 = new List<amenities>();
                var amenities = Amenities(type,id, vid).FirstOrDefault().Split(',');
                for (int i = 0; i < amenities.Length; i++)
                {
                    amenities amenity = new amenities();
                    amenity.name = amenities[i];
                    amenity.icon_url = "https://api.ahwanam.com/Icons/venueicons/"+ amenities[i] + ".png";
                    amenity1.Add(amenity);
                }
                vdetail.amenities = amenity1;
                //VendorMasterService vendorMasterService = new VendorMasterService();
                //Policy policy = vendorMasterService.Getpolicy(id, vid);
                //vdetail.policies = policy;

                //vendorpolicies
                List<policies> policy1 = new List<policies>();
                var policies = vendorpolicies(id, vid).FirstOrDefault().Split(',');
                for (int i = 0; i < policies.Length; i++)
                {
                    policies policy = new policies();
                    policy.name = policies[i];
                    policy1.Add(policy);
                }
                vdetail.policies = policy1;

                // Gallery
                
                List<gallery> g1 = new List<gallery>();
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).Where(m => m.VendorId == long.Parse(vid)).ToList();
                for (int i = 0; i < allimages.Count; i++)
                {
                    gallery g = new gallery();
                    g.url = "https://api.ahwanam.com/vendorimages/" + allimages[i].ImageName;
                    g1.Add(g);
                }
                vdetail.gallery = g1;

                dict.Add("data", vdetail);
            }
            return Json(dict);
        }

        [HttpGet]
        [Route("api/viewservice/pd")]
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
                    var amenities = Amenities(type,id, vid);
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
        [Route("api/similarvendors")]
        public IHttpActionResult similar(string type, string vendor)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            if (type == "venue" || type == null)
            {
                List<GetVendors_Result> data = resultsPageService.GetAllVendors(type).Where(m => m.page_name != vendor).Take(3).ToList();
                dict.Add("data", data);
            }
            else if (type == "catering")
            {
                List<GetCaterers_Result> data = resultsPageService.GetAllCaterers().Where(m => m.page_name != vendor).Take(3).ToList();
                dict.Add("data", data);
            }
            else if (type == "decorator")
            {
                List<GetDecorators_Result> data = resultsPageService.GetAllDecorators().Where(m => m.page_name != vendor).Take(3).ToList();
                dict.Add("data", data);
            }
            else if (type == "photography")
            {
                List<GetCaterers_Result> data = resultsPageService.GetAllCaterers().Where(m => m.page_name != vendor).Take(3).ToList();
                dict.Add("data", data);
            }
            else if (type == "pandit")
            {
                List<GetDecorators_Result> data = resultsPageService.GetAllDecorators().Where(m => m.page_name != vendor).Take(3).ToList();
                dict.Add("data", data);
            }
            return Json(dict);
        }

        [HttpGet]
        [Route("api/reviews")]
        public IHttpActionResult Review(string type, string vendor,int? page = 0,int? offset=0)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ReviewService reviewService = new ReviewService();
            if (type=="venue")
            {
                GetVendors_Result data = resultsPageService.GetAllVendors(type).Where(m => m.page_name == vendor).FirstOrDefault();
                var reviews = reviewService.GetReview(int.Parse(data.Id.ToString())).Where(m => m.Sid == long.Parse(data.subid.ToString())).ToList();
                if (reviews.Count > 0)
                {
                    dict.Add("status", true);
                    dict.Add("message", "Success");
                    List<Reviews> r = new List<Reviews>();
                    for (int i = 0; i < reviews.Count; i++)
                    {
                        Reviews re = new Reviews();
                        re.name = reviews[i].FirstName;
                        re.review = reviews[i].Comments;
                        re.rating = "5";
                        r.Add(re);
                    }
                    Dictionary<string, object> d1 = new Dictionary<string, object>();
                    d1.Add("results", r);
                    dict.Add("data", d1);
                    dict.Add("total_review_count",reviews.Count);
                    dict.Add("offset", (offset == 0) ? 6 : offset);
                    dict.Add("page", (page == 0) ? 1 : page);
                    dict.Add("no_of_pages", reviews.Count / 6);
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "No Reviews");
                    dict.Add("data", null);
                }
            }
            
            return Json(dict);
        }

        [HttpGet]
        [Route("api/viewservice/similar1")]
        public IHttpActionResult similarvendors(string type, string id, string vid)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            if (type == "Venue" || type == null)
            {
                List<GetVendors_Result> vendor = resultsPageService.GetAllVendors(type).Where(m => m.Id != long.Parse(id) && m.subid != long.Parse(vid)).Take(3).ToList();
                dict.Add("data", vendor);
            }
            else if (type == "Catering")
            {
                List<GetCaterers_Result> vendor = resultsPageService.GetAllCaterers().Where(m => m.Id != long.Parse(id) && m.subid != long.Parse(vid)).Take(3).ToList();
                dict.Add("data", vendor);
            }
            else if (type == "Decorator")
            {
                List<GetDecorators_Result> vendor = resultsPageService.GetAllDecorators().Where(m => m.Id != long.Parse(id) && m.subid != long.Parse(vid)).Take(3).ToList();
                dict.Add("data", vendor);
            }
            else if (type == "Photography")
            {
                List<GetCaterers_Result> vendor = resultsPageService.GetAllCaterers().Where(m => m.Id != long.Parse(id) && m.subid != long.Parse(vid)).Take(3).ToList();
                dict.Add("data", vendor);
            }
            else if (type == "Pandit")
            {
                List<GetDecorators_Result> vendor = resultsPageService.GetAllDecorators().Where(m => m.Id == long.Parse(id) && m.subid == long.Parse(vid)).Take(3).ToList();
                dict.Add("data", vendor);
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
        public List<string> Amenities(string type,string id, string vid)
        {
            List<string> famenities = new List<string>();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            // Retrieving Available Amenities
            if (type == "venue")
            {
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
            }
            else if (type == "catering")
            {
                //Add Amenities Code here just replicate Venue code
                famenities.Add("No Amenities Available");
            }
            else if (type == "decorator")
            {
                //Add Amenities Code here just replicate Venue code
                famenities.Add("No Amenities Available");
            }
            else if (type == "photography")
            {
                //Add Amenities Code here just replicate Venue code
                famenities.Add("No Amenities Available");
            }
            else if (type == "pandit")
            {
                //Add Amenities Code here just replicate Venue code
                famenities.Add("No Amenities Available");
            }
            else if (type == "mehendi")
            {
                //Add Amenities Code here just replicate Venue code
                famenities.Add("No Amenities Available");
            }
            return famenities;
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

        public List<string> vendorpolicies(string id,string vid)
        {
            VendorMasterService vendorMasterService = new VendorMasterService();
            var policy = vendorMasterService.Getpolicy(id, vid);
            List<Policy> p = new List<Policy>();
            p.Add(policy);
            var allpolicies = p.Select(m => new
            {
                m.Advance_Amount,
                m.Alcohol_allowed,
                m.Ample_Parking,
                m.Available_Rooms,
                m.Baarat_Allowed,
                m.Cancellation,
                m.Changing_Rooms_AC,
                m.Complimentary_Changing_Room,
                m.Decoration_starting_costs,
                m.Decorators_allowed_with_royalty,
                m.Decor_provided,
                m.Fire_Crackers_Allowed,
                m.Food_provided,
                m.Halls_AC,
                m.Hawan_Allowed,
                m.Music_Allowed_Late,
                m.NonVeg_allowed,
                m.Outside_Alcohol_allowed,
                m.Outside_decorators_allowed,
                m.Outside_food_or_caterer_allowed,
                m.Overnight_wedding_Allowed,
                m.Parking_Space,
                m.Rooms_Count,
                m.Room_Average_Price,
                m.ServiceType,
                m.Tax,
                m.Valet_Parking
            }).ToList();
            List<string> fpolicies = new List<string>();
            foreach (var item in allpolicies)
            {
                string value = string.Join(",", item).Replace("{", "").Replace("}", "");
                var availableamenities = value.Split(',');
                value = "";
                for (int i = 0; i < availableamenities.Length; i++)
                {
                    if (availableamenities[i].Split('=')[1].Trim() == "Yes")
                        value = value + "," + availableamenities[i].Split('=')[0].Trim();
                }
                fpolicies.Add(value.TrimStart(','));
            }
            if (fpolicies.Count == 0) fpolicies.Add("No Policies Available");
            //dict.Add("data", famenities);
            return fpolicies;
        }
    }
}
