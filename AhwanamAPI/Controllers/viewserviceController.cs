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

        public class similarvendor
        {
            public string category_name { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public string name { get; set; }
            public string page_name { get; set; }
            public string pic_url { get; set; }
            public price price { get; set; }
            public string rating { get; set; }
            public string reviews_count { get; set; }
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
            public bool is_favourite { get; set; }
            public string id { get; set; }
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
        public IHttpActionResult productdetails(string type, string vendor)
        {
            string id = null;
            string vid = null;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            if (type == "venue")
            {
                //type =(type == "venue") ? "Venue" : type;
                GetVendors_Result data = resultsPageService.GetAllVendors("Venue").Where(m => m.page_name == vendor).FirstOrDefault();
                id = data.Id.ToString();
                vid = data.subid.ToString();
                string image = "https://api.ahwanam.com/images/cover_image.png";
                string img = (data.logo != null || data.logo != "") ? data.logo : data.image;
                vendordetail vdetail = new vendordetail();
                vdetail.cover_image = (img != null && img != "") ? "https://api.ahwanam.com/dataimages/" + img : image;
                vdetail.page_name = data.page_name;
                vdetail.name = data.BusinessName;
                vdetail.category_name = data.ServicType;
                ReviewService reviewService = new ReviewService();
                vdetail.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                decimal trating = (data.fbrating != null && data.googlerating != null && data.jdrating != null) ? decimal.Parse(data.fbrating) + decimal.Parse(data.googlerating) + decimal.Parse(data.jdrating) : 0;
                vdetail.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                vdetail.address = data.Address + "," + data.Landmark + "," + data.City + "," + data.State;
                location loc = new location();
                if (data.GeoLocation != null && data.GeoLocation != "")
                {
                    loc.latitude = data.GeoLocation.Split(',')[0];
                    loc.longitude = data.GeoLocation.Split(',')[1];
                }
                else
                {
                    loc.latitude = "17.385044";
                    loc.longitude = "78.486671";
                }
                vdetail.location = loc;
                //vdetail.charge_type=
                vdetail.city = data.City;
                price p = new price();
                p.actual_price = data.cost1.ToString();
                p.offer_price = data.normaldays; // change this price as per the date
                p.service_price = data.ServiceCost.ToString();
                vdetail.price = p;
                vdetail.about = data.Description;
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
                var amenities = Amenities(type, id, vid).FirstOrDefault().Split(',');
                for (int i = 0; i < amenities.Length; i++)
                {
                    amenities amenity = new amenities();
                    amenity.name = amenities[i];
                    amenity.icon_url = "https://api.ahwanam.com/Icons/venueicons/" + amenities[i] + ".png";
                    amenity1.Add(amenity);
                }
                vdetail.amenities = amenity1;

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
                    if (i == 0) g.is_favourite = true;
                    else g.is_favourite = false;
                    g.id = allimages[i].ImageId.ToString();
                    g1.Add(g);
                }
                vdetail.gallery = g1;
                dict.Add("data", vdetail);
            }
            else if (type == "catering")
            {
                GetCaterers_Result data = resultsPageService.GetAllCaterers().Where(m => m.page_name == vendor).FirstOrDefault();
                id = data.Id.ToString();
                vid = data.subid.ToString();
                string image = "https://api.ahwanam.com/images/cover_image.png";
                string img = (data.logo != null || data.logo != "") ? data.logo : data.image;
                vendordetail vdetail = new vendordetail();
                vdetail.cover_image = (img != null && img != "") ? "https://api.ahwanam.com/dataimages/" + img : image;
                vdetail.page_name = data.page_name;
                vdetail.name = data.BusinessName;
                vdetail.category_name = data.ServicType;
                ReviewService reviewService = new ReviewService();
                vdetail.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                decimal trating = (data.fbrating != null && data.googlerating != null && data.jdrating != null) ? decimal.Parse(data.fbrating) + decimal.Parse(data.googlerating) + decimal.Parse(data.jdrating) : 0;
                vdetail.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                vdetail.address = data.Address + "," + data.Landmark + "," + data.City + "," + data.State;
                location loc = new location();
                if (data.GeoLocation != null && data.GeoLocation != "")
                {
                    loc.latitude = data.GeoLocation.Split(',')[0];
                    loc.longitude = data.GeoLocation.Split(',')[1];
                }
                else
                {
                    loc.latitude = "17.385044";
                    loc.longitude = "78.486671";
                }
                vdetail.location = loc;
                //vdetail.charge_type=
                vdetail.city = data.City;
                price p = new price();
                p.actual_price = data.Veg.ToString();
                p.offer_price = data.Veg.ToString(); // change this price as per the date
                p.service_price = data.Veg.ToString();
                vdetail.price = p;
                vdetail.about = data.Description;
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
                //var amenities = Amenities(type, id, vid).FirstOrDefault().Split(',');
                //for (int i = 0; i < amenities.Length; i++)
                //{
                //    amenities amenity = new amenities();
                //    amenity.name = amenities[i];
                //    amenity.icon_url = "https://api.ahwanam.com/Icons/venueicons/" + amenities[i] + ".png";
                //    amenity1.Add(amenity);
                //}
                vdetail.amenities = amenity1;

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
                    if (i == 0) g.is_favourite = true;
                    else g.is_favourite = false;
                    g.id = allimages[i].ImageId.ToString();
                    g1.Add(g);
                }
                vdetail.gallery = g1;
                dict.Add("data", vdetail);
            }
            else if (type == "decorator")
            {
                GetDecorators_Result data = resultsPageService.GetAllDecorators().Where(m => m.page_name == vendor).FirstOrDefault();
                id = data.Id.ToString();
                vid = data.subid.ToString();
                string image = "https://api.ahwanam.com/images/cover_image.png";
                string img = (data.logo != null || data.logo != "") ? data.logo : data.image;
                vendordetail vdetail = new vendordetail();
                vdetail.cover_image = (img != null && img != "") ? "https://api.ahwanam.com/dataimages/" + img : image;
                vdetail.page_name = data.page_name;
                vdetail.name = data.BusinessName;
                vdetail.category_name = data.ServicType;
                ReviewService reviewService = new ReviewService();
                vdetail.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                decimal trating = (data.fbrating != null && data.googlerating != null && data.jdrating != null) ? decimal.Parse(data.fbrating) + decimal.Parse(data.googlerating) + decimal.Parse(data.jdrating) : 0;
                vdetail.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                vdetail.address = data.Address + "," + data.Landmark + "," + data.City + "," + data.State;
                location loc = new location();
                if (data.GeoLocation != null && data.GeoLocation != "")
                {
                    loc.latitude = data.GeoLocation.Split(',')[0];
                    loc.longitude = data.GeoLocation.Split(',')[1];
                }
                else
                {
                    loc.latitude = "17.385044";
                    loc.longitude = "78.486671";
                }
                vdetail.location = loc;
                //vdetail.charge_type=
                vdetail.city = data.City;
                price p = new price();
                p.actual_price = data.cost1.ToString();
                p.offer_price = data.cost1.ToString(); // change this price as per the date
                p.service_price = data.cost1.ToString();
                vdetail.price = p;
                vdetail.about = data.Description;
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
                vdetail.amenities = amenity1;

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
                    if (i == 0) g.is_favourite = true;
                    else g.is_favourite = false;
                    g.id = allimages[i].ImageId.ToString();
                    g1.Add(g);
                }
                vdetail.gallery = g1;
                dict.Add("data", vdetail);
            }
            else if (type == "photography")
            {
                GetPhotographers_Result data = resultsPageService.GetAllPhotographers().Where(m => m.page_name == vendor).FirstOrDefault();
                id = data.Id.ToString();
                vid = data.subid.ToString();
                string image = "https://api.ahwanam.com/images/cover_image.png";
                string img = (data.logo != null || data.logo != "") ? data.logo : data.image;
                vendordetail vdetail = new vendordetail();
                vdetail.cover_image = (img != null && img != "") ? "https://api.ahwanam.com/dataimages/" + img : image;
                vdetail.page_name = data.page_name;
                vdetail.name = data.BusinessName;
                vdetail.category_name = data.ServicType;
                ReviewService reviewService = new ReviewService();
                vdetail.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                decimal trating = (data.fbrating != null && data.googlerating != null && data.jdrating != null) ? decimal.Parse(data.fbrating) + decimal.Parse(data.googlerating) + decimal.Parse(data.jdrating) : 0;
                vdetail.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                vdetail.address = data.Address + "," + data.Landmark + "," + data.City + "," + data.State;
                location loc = new location();
                if (data.GeoLocation != null && data.GeoLocation != "")
                {
                    loc.latitude = data.GeoLocation.Split(',')[0];
                    loc.longitude = data.GeoLocation.Split(',')[1];
                }
                else
                {
                    loc.latitude = "17.385044";
                    loc.longitude = "78.486671";
                }
                vdetail.location = loc;
                //vdetail.charge_type=
                vdetail.city = data.City;
                price p = new price();
                p.actual_price = data.cost1.ToString();
                p.offer_price = data.cost1.ToString(); // change this price as per the date
                p.service_price = data.cost1.ToString();
                vdetail.price = p;
                vdetail.about = data.Description;
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
                vdetail.amenities = amenity1;

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
                    if (i == 0) g.is_favourite = true;
                    else g.is_favourite = false;
                    g.id = allimages[i].ImageId.ToString();
                    g1.Add(g);
                }
                vdetail.gallery = g1;
                dict.Add("data", vdetail);
            }
            else if (type == "pandit" || type == "mehendi")
            {
                type = (type == "pandit") ? "Pandit" : type;
                type = (type == "mehendi") ? "Mehendi" : type;
                GetOthers_Result data = resultsPageService.GetAllOthers(type).Where(m => m.page_name == vendor).FirstOrDefault();
                id = data.Id.ToString();
                vid = data.subid.ToString();
                string image = "https://api.ahwanam.com/images/cover_image.png";
                string img = (data.logo != null || data.logo != "") ? data.logo : data.image;
                vendordetail vdetail = new vendordetail();
                vdetail.cover_image = (img != null && img != "") ? "https://api.ahwanam.com/dataimages/" + img : image;
                vdetail.page_name = data.page_name;
                vdetail.name = data.BusinessName;
                vdetail.category_name = data.ServicType;
                ReviewService reviewService = new ReviewService();
                vdetail.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                decimal trating = (data.fbrating != null && data.googlerating != null && data.jdrating != null) ? decimal.Parse(data.fbrating) + decimal.Parse(data.googlerating) + decimal.Parse(data.jdrating) : 0;
                vdetail.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                vdetail.address = data.Address + "," + data.Landmark + "," + data.City + "," + data.State;
                location loc = new location();
                if (data.GeoLocation != null && data.GeoLocation != "")
                {
                    loc.latitude = data.GeoLocation.Split(',')[0];
                    loc.longitude = data.GeoLocation.Split(',')[1];
                }
                else
                {
                    loc.latitude = "17.385044";
                    loc.longitude = "78.486671";
                }
                vdetail.location = loc;
                //vdetail.charge_type=
                vdetail.city = data.City;
                price p = new price();
                p.actual_price = data.ItemCost.ToString();
                p.offer_price = data.ItemCost.ToString(); // change this price as per the date
                p.service_price = data.ItemCost.ToString();
                vdetail.price = p;
                vdetail.about = data.Description;
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
                vdetail.amenities = amenity1;

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
                    if (i == 0) g.is_favourite = true;
                    else g.is_favourite = false;
                    g.id = allimages[i].ImageId.ToString();
                    g1.Add(g);
                }
                vdetail.gallery = g1;
                dict.Add("data", vdetail);
            }
            else
            {
                dict.Clear();
                dict.Add("status", false);
                dict.Add("message", "Failed");
                dict.Add("data", null);
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
                    var amenities = Amenities(type, id, vid);
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
            List<string> d = new List<string>();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            List<similarvendor> d1 = new List<similarvendor>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            if (type == "venue" || type == null)
            {
                var data = resultsPageService.GetAllVendors(type).Where(m => m.page_name != vendor).Take(3).ToList(); //List<GetVendors_Result>
                for (int i = 0; i < data.Count; i++)
                {
                    string id = data[i].Id.ToString();
                    string vid = data[i].subid.ToString();
                    string image = "https://api.ahwanam.com/images/cover_image.png";
                    string img = (data[i].logo != null || data[i].logo != "") ? data[i].logo : data[i].image;
                    similarvendor similar = new similarvendor();
                    similar.category_name = data[i].ServicType;
                    similar.charge_type = "Per day";
                    similar.city = data[i].City;
                    similar.name = data[i].BusinessName;
                    similar.page_name = data[i].page_name;
                    similar.pic_url = (img != null && img != "") ? "https://api.ahwanam.com/vendorimages/" + img : image;
                    price p = new price();
                    p.actual_price = data[i].cost1.ToString();
                    p.offer_price = data[i].normaldays; // change this price as per the date
                    p.service_price = data[i].ServiceCost.ToString();
                    similar.price = p;
                    decimal trating = (data[i].fbrating != null && data[i].googlerating != null && data[i].jdrating != null) ? decimal.Parse(data[i].fbrating) + decimal.Parse(data[i].googlerating) + decimal.Parse(data[i].jdrating) : 0;
                    similar.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                    ReviewService reviewService = new ReviewService();
                    similar.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                    d1.Add(similar);
                }
                dict1.Add("results", d1);
                dict.Add("data", dict1);
            }
            else if (type == "catering")
            {
                List<GetCaterers_Result> data = resultsPageService.GetAllCaterers().Where(m => m.page_name != vendor).Take(3).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    string id = data[i].Id.ToString();
                    string vid = data[i].subid.ToString();
                    string image = "https://api.ahwanam.com/images/cover_image.png";
                    string img = (data[i].logo != null || data[i].logo != "") ? data[i].logo : data[i].image;
                    similarvendor similar = new similarvendor();
                    similar.category_name = data[i].ServicType;
                    similar.charge_type = "Per day";
                    similar.city = data[i].City;
                    similar.name = data[i].BusinessName;
                    similar.page_name = data[i].page_name;
                    similar.pic_url = (img != null && img != "") ? "https://api.ahwanam.com/vendorimages/" + img : image;
                    price p = new price();
                    p.actual_price = data[i].Veg.ToString();
                    p.offer_price = data[i].Veg.ToString(); // change this price as per the date
                    p.service_price = data[i].Veg.ToString();
                    similar.price = p;
                    decimal trating = (data[i].fbrating != null && data[i].googlerating != null && data[i].jdrating != null) ? decimal.Parse(data[i].fbrating) + decimal.Parse(data[i].googlerating) + decimal.Parse(data[i].jdrating) : 0;
                    similar.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                    ReviewService reviewService = new ReviewService();
                    similar.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                    d1.Add(similar);
                }
                dict1.Add("results", d1);
                dict.Add("data", dict1);
            }
            else if (type == "decorator")
            {
                List<GetDecorators_Result> data = resultsPageService.GetAllDecorators().Where(m => m.page_name != vendor).Take(3).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    string id = data[i].Id.ToString();
                    string vid = data[i].subid.ToString();
                    string image = "https://api.ahwanam.com/images/cover_image.png";
                    string img = (data[i].logo != null || data[i].logo != "") ? data[i].logo : data[i].image;
                    similarvendor similar = new similarvendor();
                    similar.category_name = data[i].ServicType;
                    similar.charge_type = "Per day";
                    similar.city = data[i].City;
                    similar.name = data[i].BusinessName;
                    similar.page_name = data[i].page_name;
                    similar.pic_url = (img != null && img != "") ? "https://api.ahwanam.com/vendorimages/" + img : image;
                    price p = new price();
                    p.actual_price = data[i].cost1.ToString();
                    p.offer_price = data[i].cost1.ToString(); // change this price as per the date
                    p.service_price = data[i].cost1.ToString();
                    similar.price = p;
                    decimal trating = (data[i].fbrating != null && data[i].googlerating != null && data[i].jdrating != null) ? decimal.Parse(data[i].fbrating) + decimal.Parse(data[i].googlerating) + decimal.Parse(data[i].jdrating) : 0;
                    similar.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                    ReviewService reviewService = new ReviewService();
                    similar.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                    d1.Add(similar);
                }
                dict1.Add("results", d1);
                dict.Add("data", dict1);
            }
            else if (type == "photography")
            {
                List<GetPhotographers_Result> data = resultsPageService.GetAllPhotographers().Where(m => m.page_name != vendor).Take(3).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    string id = data[i].Id.ToString();
                    string vid = data[i].subid.ToString();
                    string image = "https://api.ahwanam.com/images/cover_image.png";
                    string img = (data[i].logo != null || data[i].logo != "") ? data[i].logo : data[i].image;
                    similarvendor similar = new similarvendor();
                    similar.category_name = data[i].ServicType;
                    similar.charge_type = "Per day";
                    similar.city = data[i].City;
                    similar.name = data[i].BusinessName;
                    similar.page_name = data[i].page_name;
                    similar.pic_url = (img != null && img != "") ? "https://api.ahwanam.com/vendorimages/" + img : image;
                    price p = new price();
                    p.actual_price = data[i].cost1.ToString();
                    p.offer_price = data[i].cost1.ToString(); // change this price as per the date
                    p.service_price = data[i].cost1.ToString();
                    similar.price = p;
                    decimal trating = (data[i].fbrating != null && data[i].googlerating != null && data[i].jdrating != null) ? decimal.Parse(data[i].fbrating) + decimal.Parse(data[i].googlerating) + decimal.Parse(data[i].jdrating) : 0;
                    similar.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                    ReviewService reviewService = new ReviewService();
                    similar.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                    d1.Add(similar);
                }
                dict1.Add("results", d1);
                dict.Add("data", dict1);
            }
            else if (type == "pandit" || type == "mehendi")
            {
                List<GetOthers_Result> data = resultsPageService.GetAllOthers(type).Where(m => m.page_name != vendor).Take(3).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    string id = data[i].Id.ToString();
                    string vid = data[i].subid.ToString();
                    string image = "https://api.ahwanam.com/images/cover_image.png";
                    string img = (data[i].logo != null || data[i].logo != "") ? data[i].logo : data[i].image;
                    similarvendor similar = new similarvendor();
                    similar.category_name = data[i].ServicType;
                    similar.charge_type = "Per day";
                    similar.city = data[i].City;
                    similar.name = data[i].BusinessName;
                    similar.page_name = data[i].page_name;
                    similar.pic_url = (img != null && img != "") ? "https://api.ahwanam.com/vendorimages/" + img : image;
                    price p = new price();
                    p.actual_price = data[i].ItemCost.ToString();
                    p.offer_price = data[i].ItemCost.ToString(); // change this price as per the date
                    p.service_price = data[i].ItemCost.ToString();
                    similar.price = p;
                    decimal trating = (data[i].fbrating != null && data[i].googlerating != null && data[i].jdrating != null) ? decimal.Parse(data[i].fbrating) + decimal.Parse(data[i].googlerating) + decimal.Parse(data[i].jdrating) : 0;
                    similar.rating = (trating != 0) ? (trating / 3).ToString().Substring(0, 4) : "0";
                    ReviewService reviewService = new ReviewService();
                    similar.reviews_count = reviewService.GetReview(int.Parse(id)).Where(m => m.Sid == long.Parse(vid)).Count().ToString();
                    d1.Add(similar);
                }
                dict1.Add("results", d1);
                dict.Add("data", dict1);
            }

            //Format API

            return Json(dict);
        }

        [HttpGet]
        [Route("api/reviews")]
        public IHttpActionResult Review(string type, string vendor, int? page = 0, int? offset = 0)
        {
            int count = 0;
            page = (page == null || page == 0) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page -1) * (int)offset;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ReviewService reviewService = new ReviewService();
            var reviews = new List<Review>();
            if (type == "venue")
            {
                GetVendors_Result data = resultsPageService.GetAllVendors(type).Where(m => m.page_name == vendor).FirstOrDefault();
                reviews = reviewService.GetReview(int.Parse(data.Id.ToString())).Where(m => m.Sid == long.Parse(data.subid.ToString())).ToList();
                count = reviews.Count();
            }
            else if (type == "catering")
            {
                GetCaterers_Result data = resultsPageService.GetAllCaterers().Where(m => m.page_name == vendor).FirstOrDefault();
                reviews = reviewService.GetReview(int.Parse(data.Id.ToString())).Where(m => m.Sid == long.Parse(data.subid.ToString())).ToList();
                count = reviews.Count();
            }
            else if (type == "decorator")
            {
                GetDecorators_Result data = resultsPageService.GetAllDecorators().Where(m => m.page_name == vendor).FirstOrDefault();
                reviews = reviewService.GetReview(int.Parse(data.Id.ToString())).Where(m => m.Sid == long.Parse(data.subid.ToString())).ToList();
                count = reviews.Count();
            }
            else if (type == "photographer")
            {
                GetPhotographers_Result data = resultsPageService.GetAllPhotographers().Where(m => m.page_name == vendor).FirstOrDefault();
                reviews = reviewService.GetReview(int.Parse(data.Id.ToString())).Where(m => m.Sid == long.Parse(data.subid.ToString())).ToList();
                count = reviews.Count();
            }
            else if (type == "pandit" || type == "mehendi")
            {
                GetOthers_Result data = resultsPageService.GetAllOthers(type).Where(m => m.page_name == vendor).FirstOrDefault();
                reviews = reviewService.GetReview(int.Parse(data.Id.ToString())).Where(m => m.Sid == long.Parse(data.subid.ToString())).ToList();
                count = reviews.Count();
            }
            if (page > 1)
                reviews = reviews.Skip(takecount).Take((int)offset).ToList();
            else
                reviews = reviews.Take((int)offset).ToList();
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
                    re.rating = reviews[i].rating;
                    r.Add(re);
                }
                Dictionary<string, object> d1 = new Dictionary<string, object>();
                d1.Add("results", r);
                d1.Add("total_review_count", count);
                d1.Add("offset", (offset == 0) ? 6 : offset);
                d1.Add("page", page);
                d1.Add("no_of_pages", ((count -1)/ offset)+1);
                dict.Add("data", d1);
                
            }
            else
            {
                Dictionary<string, object> d1 = new Dictionary<string, object>();
                dict.Add("status", false);
                dict.Add("message", "No Reviews");
                //dict.Add("data", null);
                d1.Add("results", new List<Review>());
                d1.Add("total_review_count", 0);
                d1.Add("offset", (offset == 0) ? 6 : offset);
                d1.Add("page", page);
                d1.Add("no_of_pages", 0);
                dict.Add("data", d1);
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
        public List<string> Amenities(string type, string id, string vid)
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

        public List<string> vendorpolicies(string id, string vid)
        {
            VendorMasterService vendorMasterService = new VendorMasterService();
            var policy = vendorMasterService.Getpolicy(id, vid);
            List<string> fpolicies = new List<string>();
            if (policy == null)
            {
                fpolicies.Add("No Policies Available");
                return fpolicies;
            }
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
