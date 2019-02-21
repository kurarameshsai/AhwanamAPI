using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using System.Text;
using System.Collections;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Web;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using MaaAahwanam.Utility;

namespace AhwanamAPI.Controllers
{
    //[Authorize]
    public class resultsController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();

        #region classes
        public class header
        {
            public string header_text { get; set; }
            public string sub_text { get; set; }
            public string image { get; set; }
        }

        public class prices
        {
            public string actual_price { get; set; }
            public string offer_price { get; set; }
        }

        public class param
        {
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public string description { get; set; }
            public string rating { get; set; }
            public string charge_type { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string city { get; set; }
            public prices price { get; set; }
            public string pic_url { get; set; }
            public string min_guest { get; set; }
            public string max_guest { get; set; }
            public string service_price { get; set; }
        }

        public class localities
        {
            public string name { get; set; }
            public long id { get; set; }
        }

        public class values
        {
            public string name { get; set; }
            public long id { get; set; }
            //public List<localities> localities { get; set; }
        }

        public class value
        {
            public string name { get; set; }
            public long id { get; set; }
            public List<localities> localities { get; set; }
        }

        public class newfilter
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public List<values> values { get; set; }
            public List<value> value { get; set; }
            public bool is_mutliple_selection { get; set; }
        }

        public class newcity
        {
            public string name { get; set; }
            public long id { get; set; }
            public List<localities> localities { get; set; }
        }

        //public class cookie
        //{
        //    public header header { get; set; }
        //    public sortby sortby { get; set; }
        //    public rating rating { get; set; }
        //    public List<venue_type> venue_type { get; set; }
        //    public List<guests> guests { get; set; }
        //    public List<price_per_plate> price_per_plate { get; set; }
        //    public List<space_preference> space_preference { get; set; }
        //    public List<city> city { get; set; }
        //}
        public class cookie
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public List<values> values { get; set; }
            //Venue Filters
            public sortby sort_options { get; set; }
            public city city { get; set; }
            public string is_mutliple_selection { get; set; }
            public rating rating { get; set; }
            public venue_type venuetype { get; set; }
            public guests guests { get; set; }
            public price_per_plate priceperplate { get; set; }
            public space_preference space_preference { get; set; }
        }

        #region common filters
        public class city
        {
            public string display_name { get; set; }
            public string name { get; set; }
            public List<value> value { get; set; }
            //public string name { get; set; }
            //public long id { get; set; }
            //public List<localities> localities { get; set; }
        }

        public class locality
        {
            public string name { get; set; }
            public long id { get; set; }
        }

        public class rating
        {
            public string display_name { get; set; }
            public string name { get; set; }
            public List<values> value { get; set; }
        }

        public class sortby
        {
            public string name { get; set; }
            public long id { get; set; }
        }
        #endregion

        #region venue filter
        public class venue_type
        {
            public string display_name { get; set; }
            public string name { get; set; }
            public List<values> value { get; set; }
        }

        public class guests
        {
            public string display_name { get; set; }
            public string name { get; set; }
            public List<values> value { get; set; }
        }

        public class price_per_plate
        {
            public string display_name { get; set; }
            public string name { get; set; }
            public List<values> value { get; set; }
        }

        public class space_preference
        {
            public string display_name { get; set; }
            public string name { get; set; }
            public List<values> value { get; set; }
        }
        #endregion

        #endregion

        #region API
        [HttpGet]
        [Route("api/results/getall")]
        public IHttpActionResult getrecords(string type, int? city = -1, int? locality = -1, int? page = 0, int? capacity = -1, int? price_per_plate_or_rental = -1, int? offset = 0, int? sortby = -1, int? space_preference = -1, int? rating = -1, int? venue_type = -1)
        {
            int status = checktoken(); // Checking Token
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            type = (type == null) ? "Venue" : type;
            //Type Venue
            if (type == "Venue")
                dict = VenueRecords(type, city, locality, page, capacity, price_per_plate_or_rental, offset, sortby, space_preference, rating, venue_type);
            //Type Caterer
            else if (type == "Catering")
                dict = CatererRecords(type, page, offset, rating);
            //Type Decorator
            else if (type == "Photographer")
                dict = PhotographerRecords(type, page, offset, rating);
            //Type photographer
            else if (type == "Decorator")
                dict = DecoratorRecords(type, page, offset, rating);
            //Type Other
            else if (type == "Pandit" || type == "Mehendi")
                dict = OtherRecords(type, page, offset, rating);
            dict1.Add("status", true);
            dict1.Add("message", "Success");
            dict1.Add("data", dict);
            return Json(dict1);
        }

        #region Venue_Records
        public Dictionary<string, object> VenueRecords(string type, int? city = -1, int? locality = -1, int? page = 0, int? capacity = -1, int? price_per_plate_or_rental = -1, int? offset = 0, int? sortby = -1, int? space_preference = -1, int? rating = -1, int? venue_type = -1)
        {
            string guestsvalue = (capacity != -1) ? cookies((int)capacity, "guests") : null;
            string cityvalue = (city != -1) ? cookies((int)city, "city") : null;
            string localityvalue = (locality != -1) ? cookies((int)locality, "locality") : null;
            string pricevalue = (price_per_plate_or_rental != -1) ? cookies((int)price_per_plate_or_rental, "price") : null;
            string spacevalue = (space_preference != -1) ? cookies((int)space_preference, "space") : null;
            string sortbyvalue = (sortby != -1) ? cookies((int)sortby, "sort") : null;
            string ratingvalue = (rating != -1) ? cookies((int)rating, "rating") : null;
            string vtypevalue = (venue_type != -1) ? cookies((int)venue_type, "vtype") : null;
            page = (page == null) ? 1 : page;
            int takecount = (int)page * (int)offset;

            //Retrieve Cookie value
            var data = resultsPageService.GetAllVendors(type);//.Skip(page*6).ToList();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            if (cityvalue != null)
                data = data.Where(m => m.City == cityvalue).ToList();
            if (localityvalue != null && cityvalue != null)
                data = data.Where(m => m.Landmark == localityvalue).ToList();
            if (guestsvalue != null)
                data = data.Where(m => m.Minimumseatingcapacity > int.Parse(guestsvalue)).ToList();
            if (pricevalue != null)
                data = data.Where(m => m.cost1 > decimal.Parse(pricevalue)).ToList();
            //if (ratingvalue != null)
            //data = data.Where(m => m. == localityvalue).ToList();
            #region Format API
            List<param> param = new List<param>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    decimal trating = (item.fbrating != null && item.googlerating != null && item.jdrating != null) ? decimal.Parse(item.fbrating) + decimal.Parse(item.googlerating) + decimal.Parse(item.jdrating) : 0;
                    param p = new param();

                    //prices Section
                    prices price = new prices();
                    price.actual_price = item.cost1.ToString();
                    price.offer_price = item.normaldays;

                    //Data Section
                    p.name = item.BusinessName;
                    p.category_name = item.ServicType;
                    p.reviews_count = "58";
                    p.description = item.Description;
                    p.rating = (trating != 0) ? (trating/3).ToString().Substring(0,4) : "0";
                    p.charge_type = "Per Day";
                    p.latitude = "17.385044";
                    p.longitude = "78.486671";
                    p.city = item.City;
                    p.pic_url = "https://api.ahwanam.com/vendorimages/" + item.image;
                    p.price = price;
                    p.min_guest = item.Minimumseatingcapacity.ToString();
                    p.max_guest = item.Maximumcapacity.ToString();
                    p.service_price = item.ServiceCost.ToString();
                    param.Add(p);
                }
            }
            var records = param;
            #endregion

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", records);
            dict.Add("total_count", data.Count);
            dict.Add("offset", (offset == null) ? 12 : offset);
            dict.Add("no_of_pages", data.Count / 6);
            dict.Add("sort_options", (sortby == null) ? 1 : sortby);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Caterer_Records
        public Dictionary<string, object> CatererRecords(string type, int? page = 0, int? offset = 0, int? rating = -1)
        {
            page = (page == null) ? 1 : page;
            int takecount = (int)page * (int)offset;
            var data = resultsPageService.GetAllCaterers();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", data);
            dict.Add("total_count", data.Count);
            dict.Add("offset", (offset == null) ? 12 : offset);
            dict.Add("no_of_pages", data.Count / 6);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Decorator_Records
        public Dictionary<string, object> DecoratorRecords(string type, int? page = 0, int? offset = 0, int? rating = -1)
        {
            page = (page == null) ? 1 : page;
            int takecount = (int)page * (int)offset;
            var data = resultsPageService.GetAllDecorators();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", data);
            dict.Add("total_count", data.Count);
            dict.Add("offset", (offset == null) ? 12 : offset);
            dict.Add("no_of_pages", data.Count / 6);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Photographer_Records
        public Dictionary<string, object> PhotographerRecords(string type, int? page = 0, int? offset = 0, int? rating = -1)
        {
            page = (page == null) ? 1 : page;
            int takecount = (int)page * (int)offset;
            var data = resultsPageService.GetAllPhotographers();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", data);
            dict.Add("total_count", data.Count);
            dict.Add("offset", (offset == null) ? 12 : offset);
            dict.Add("no_of_pages", data.Count / 6);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Other_Records
        public Dictionary<string, object> OtherRecords(string type, int? page = 0, int? offset = 0, int? rating = -1)
        {
            page = (page == null) ? 1 : page;
            int takecount = (int)page * (int)offset;
            var data = resultsPageService.GetAllOthers(type);
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", data);
            dict.Add("total_count", data.Count);
            dict.Add("offset", (offset == null) ? 12 : offset);
            dict.Add("no_of_pages", data.Count / 6);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        [HttpGet]
        [Route("api/results/getfilters")]
        public IHttpActionResult getfilters(string type)
        {
            type = (type == null) ? "Venue" : type;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            // Header Section
            header headers = new header();
            if (type == "Venue")
                headers.header_text = "Best Wedding Venues";
            else if (type == "Catering")
                headers.header_text = "Best Catering Vendors";
            else if (type == "Decorator")
                headers.header_text = "Best Decorator Vendors";
            else if (type == "Photography")
                headers.header_text = "Best Photography Vendors";
            else if (type == "Pandit")
                headers.header_text = "Best Pandit Vendors";
            else if (type == "Mehendi")
                headers.header_text = "Best Mehendi Vendors";
            headers.sub_text = "Sub Text";
            headers.image = "https://api.ahwanam.com/images/header1.png";
            dict.Add("header", headers);

            List<newfilter> filter = new List<newfilter>();
            newfilter f = new newfilter();
            //Sort Section
            List<sortby> sort1 = new List<sortby>();
            string slist = "price-low-to-high!price-high-to-low";
            for (int i = 0; i < slist.Split('!').Count(); i++)
            {
                sortby sort = new sortby();
                sort.name = slist.Split('!')[i];
                sort.id = i;
                sort1.Add(sort);
            }
            dict.Add("sort_options", sort1);
            // Rating Section
            string list = "All Ratings!Rated 3.0+!Rated 4.0+!Rated 5.0+";
            f.name = "rating";
            f.display_name = "Rating";
            List<values> val = new List<values>();
            for (int i = 0; i < list.Split('!').Count(); i++)
            {
                values v = new values();
                v.name = list.Split('!')[i];
                v.id = i;
                val.Add(v);
            }
            f.values = val;
            f.is_mutliple_selection = true;
            filter.Add(f);

            if (type == "Venue" || type == "Photographer" || type == "Catering")
            {
                //Venue Type Section
                f = new newfilter();
                val = new List<values>();
                if (type == "Venue")
                {
                    list = "4 Star & Above Hotels!Banquet Hall!Lawn / Farmhouse!Hotels!Country / Golf Club!Resort!Restaurant / Lounge Bar!Heritage property";
                    f.name = "venue_type";
                    f.display_name = "Venue Type";
                }
                else if (type == "Photographer")
                {
                    list = "Photography (candid + traditional)!Photography + Videography";
                    f.name = "services";
                    f.display_name = "Services";
                }
                else if (type == "Catering")
                {
                    list = "Vegetarian!Street food!South Indian!North Indian";
                    f.name = "dietary_preferences";
                    f.display_name = "Dietary Preferences";
                }
                
                for (int i = 0; i < list.Split('!').Count(); i++)
                {
                    values v = new values();
                    v.name = list.Split('!')[i];
                    v.id = i;
                    val.Add(v);
                }
                f.values = val;
                f.is_mutliple_selection = true;
                filter.Add(f);
            }

            if (type == "Venue")
            {
                //Guests Section
                f = new newfilter();
                val = new List<values>();
                list = "< 100!100-250!250-500!500-1000!> 1000";
                f.name = "capacity";
                f.display_name = "No. of Guests";
                for (int i = 0; i < list.Split('!').Count(); i++)
                {
                    values v = new values();
                    v.name = list.Split('!')[i];
                    v.id = i;
                    val.Add(v);
                }
                f.values = val;
                f.is_mutliple_selection = true;
                filter.Add(f);

                //Price per plate Section
                f = new newfilter();
                val = new List<values>();
                list = "< 1000!1000-1500!1500-2000!2000-3000!> 3000,Rental";
                f.name = "price_per_plate_or_rental";
                f.display_name = "Price per plate";
                for (int i = 0; i < list.Split('!').Count(); i++)
                {
                    values v = new values();
                    v.name = list.Split('!')[i];
                    v.id = i;
                    val.Add(v);
                }
                f.values = val;
                f.is_mutliple_selection = true;
                filter.Add(f);

                //Space preference Section
                f = new newfilter();
                val = new List<values>();
                list = "Indoor!Outdoor!Indoor with outdoor!Terrace!Poolside";
                f.name = "space_preference";
                f.display_name = "Space Preference";
                for (int i = 0; i < list.Split('!').Count(); i++)
                {
                    values v = new values();
                    v.name = list.Split('!')[i];
                    v.id = i;
                    val.Add(v);
                }
                f.values = val;
                f.is_mutliple_selection = true;
                filter.Add(f);
            }
            if (type == "Photographer" || type == "Decorator" || type == "Catering")
            {
                //Budget per plate Section
                f = new newfilter();
                val = new List<values>();
                list = "< 50000!50000 - 200000!200000 - 350000!350000 - 450000!> 450000";
                f.name = "budget_per_plate";
                f.display_name = "Budget per plate";
                for (int i = 0; i < list.Split('!').Count(); i++)
                {
                    values v = new values();
                    v.name = list.Split('!')[i];
                    v.id = i;
                    val.Add(v);
                }
                f.values = val;
                f.is_mutliple_selection = true;
                filter.Add(f);
            }

            //City Section
            VendorMasterService vendorMasterService = new VendorMasterService();
            var data = vendorMasterService.SearchVendors();
            var citylist = data.Select(m => m.City).Distinct().ToList();
            f = new newfilter();
            f.name = "city";
            f.display_name = "City";
            List<value> val1 = new List<value>();
            //List<newcity> city = new List<newcity>();

            for (int i = 0; i < citylist.Count; i++)
            {
                value c = new value();
                c.name = citylist[i];
                c.id = i;
                var landmark = data.Where(m => m.City == c.name).Select(m => m.Landmark).Distinct().ToList();
                List<localities> locality1 = new List<localities>();
                for (int j = 0; j < landmark.Count; j++)
                {
                    localities loc = new localities();
                    loc.name = landmark[j];
                    loc.id = j;
                    locality1.Add(loc);
                }
                c.localities = locality1;
                //city.Add(c);
                val1.Add(c);
            }
            //f.values = city;
            f.value = val1;
            f.is_mutliple_selection = true;
            filter.Add(f);

            dict.Add("filters", filter);
            //Cookie Section
            HttpContext.Current.Response.Cookies["filters"].Value = JsonConvert.SerializeObject(dict); //new JavaScriptSerializer().Serialize(dict);
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            dict1.Add("status", true);
            dict1.Add("message", "Success");
            dict1.Add("data", dict);
            return Json(dict1);
        }

        [HttpGet]
        [Route("api/results/getallsearch")]
        public IHttpActionResult getallsearch(string type, string loc, string eventtype, string count, string date)
        {
            type = (type == null) ? "Venue" : type;
            var data = resultsPageService.GetAllVendors(type);
            return Json(data);
        }
        [HttpGet]
        [Route("api/results/search")]
        public IHttpActionResult searchvendor(string name, string type)
        {
            //Filter Vendors by Name
            type = (type == null) ? "Venue" : type;
            var data = resultsPageService.GetVendorsByName(type, name);
            return Json(data);
        }

        [HttpGet]
        [Route("api/results/load")]
        public IHttpActionResult loadmore(string type, string range)
        {
            // CheckBox Checked
            type = (type == null) ? "Venue" : type;
            var selectedservices = type.Split(',');
            var data = vendorlist(6, selectedservices, "first", 6);
            if (range != null)
                data = data.Where(m => m.cost1 >= decimal.Parse(range.Split(',')[0]) && m.cost1 <= decimal.Parse(range.Split(',')[1])).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("api/results/lazy")]
        public IHttpActionResult lazyload(string count, string type, string range)
        {
            // On Scroll/Lazy Load
            if (range == null) range = "0,0";
            int takecount = (count == "" || count == null) ? 6 : int.Parse(count) * 6;
            List<GetVendors_Result> vendorslist = vendorlist(6, type.Split(','), "next", takecount).Where(m => m.cost1 >= decimal.Parse(range.Split(',')[0]) && m.cost1 <= decimal.Parse(range.Split(',')[1])).ToList();
            return Json(vendorslist);
        }

        [HttpGet]
        [Route("api/results/ratings")]
        public IHttpActionResult ratings()
        {
            RatingsServices ratingsServices = new RatingsServices();
            var list = ratingsServices.GetRatings();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("data", list);
            return Json(dict);
        }

        [HttpGet]
        [Route("api/results/ratingcount")]
        public IHttpActionResult ratingcount()
        {
            RatingsServices ratingsServices = new RatingsServices();
            var list = ratingsServices.GetRatings();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("count", list.Count);
            return Json(dict);
        }

        public List<GetVendors_Result> vendorlist(int count, string[] selectedservices, string command, int takecount)
        {
            List<GetVendors_Result> list = new List<GetVendors_Result>();
            int recordcount = count / selectedservices.Count();
            takecount = takecount / selectedservices.Count();
            for (int i = 0; i < selectedservices.Count(); i++)
            {
                selectedservices[i] = (selectedservices[i] == "Convention" || selectedservices[i] == "Banquet" || selectedservices[i] == "Function") ? selectedservices[i] + " Hall" : selectedservices[i];
                var getrecords = resultsPageService.GetAllVendors(selectedservices[i]);
                if (command == "next")
                    getrecords = getrecords.Skip(takecount).Take(recordcount).ToList();
                else if (command == "first")
                    getrecords = getrecords.Take(recordcount).ToList();
                list.AddRange(getrecords);
            }
            return list;
        }
        #endregion

        #region reference
        public string cookies(int first, string type, int? second = 0)
        {
            string returnvalue = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["filters"];
            if (cookie != null)
            {
                string headerdata = cookie.Value.ToString();
                Dictionary<string, object> filter = new Dictionary<string, object>();
                var filters = JsonConvert.DeserializeObject<Dictionary<string, object>>(headerdata).ToList();
                cookie coki = new cookie();
                var header = filters[0].Value;
                var sort = JsonConvert.DeserializeObject<List<sortby>>(filters[1].Value.ToString()).ToList();
                var fil = JsonConvert.DeserializeObject<List<newfilter>>(filters[2].Value.ToString()).ToList();

                //Rating Section
                rating r = new rating();
                r.name = fil[0].name;
                r.display_name = fil[0].display_name;
                r.value = fil[0].values;
                coki.rating = r;

                //Venue Type Section
                venue_type venue_type = new venue_type();
                venue_type.name = fil[1].name;
                venue_type.display_name = fil[1].display_name;
                venue_type.value = fil[1].values;
                coki.venuetype = venue_type;

                //Guests Section
                guests guests = new guests();
                guests.name = fil[2].name;
                guests.display_name = fil[2].display_name;
                guests.value = fil[2].values;
                coki.guests = guests;

                //Price Section
                price_per_plate price = new price_per_plate();
                price.name = fil[3].name;
                price.display_name = fil[3].display_name;
                price.value = fil[3].values;
                coki.priceperplate = price;

                //Space Section
                space_preference space = new space_preference();
                space.name = fil[4].name;
                space.display_name = fil[4].display_name;
                space.value = fil[4].values;
                coki.space_preference = space;

                //City Section
                city city = new city();
                city.name = fil[5].name;
                city.display_name = fil[5].display_name;
                city.value = fil[5].value;
                coki.city = city;

                // Cookie filter Section
                if (type == "guests")
                    returnvalue = coki.guests.value.Where(m => m.id == first).FirstOrDefault().name;
                else if (type == "city")
                    returnvalue = coki.city.value.Where(m => m.id == first).FirstOrDefault().name;
                else if (type == "locality")
                    returnvalue = coki.city.value.Where(m => m.id == first).FirstOrDefault().localities.Where(m => m.id == second).FirstOrDefault().name;
                else if (type == "rating")
                    returnvalue = coki.rating.value.Where(m => m.id == first).FirstOrDefault().name;
                else if (type == "vtype")
                    returnvalue = coki.venuetype.value.Where(m => m.id == first).FirstOrDefault().name;
                else if (type == "price")
                    returnvalue = coki.priceperplate.value.Where(m => m.id == first).FirstOrDefault().name;
                else if (type == "space")
                    returnvalue = coki.space_preference.value.Where(m => m.id == first).FirstOrDefault().name;
                else if (type == "sort")
                    returnvalue = sort.Where(m => m.id == first).FirstOrDefault().name;
            }
            return returnvalue;
        }

        public int checktoken()
        {
            var re = Request;
            var customheader = re.Headers;
            if (customheader.Contains("token"))
            {
                string token = customheader.GetValues("token").First();
                encptdecpt encrypt = new encptdecpt();
                string descrypt = encrypt.Decrypt(token);
                UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
                long data = userlogindetailsservice.GetLoginDetailsByEmail(descrypt);
                if (data != 0) return 1;
            }
            return 0;
        }
        #endregion

        //[HttpGet]
        //[Route("api/results/getfilters1")]
        //public IHttpActionResult getfilters1(string type = "Venue")
        //{
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    // Header Section
        //    header headers = new header();
        //    headers.header_text = "Best Wedding Venues";
        //    headers.sub_text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud";
        //    headers.image = "https://api.ahwanam.com/convention1.jpg";
        //    dict.Add("header", headers);

        //    filters filter = new filters();

        //    #region Common Filters
        //    //Sort Section
        //    List<sortby> sort1 = new List<sortby>();
        //    string slist = "price-low-to-high!price-high-to-low";
        //    for (int i = 0; i < slist.Split('!').Count(); i++)
        //    {
        //        sortby sort = new sortby();
        //        sort.name = slist.Split('!')[i];
        //        sort.id = i;
        //        sort1.Add(sort);
        //    }
        //    filter.sort_options = sort1;

        //    //City Section
        //    VendorMasterService vendorMasterService = new VendorMasterService();
        //    var data = vendorMasterService.SearchVendors();
        //    var citylist = data.Select(m => m.City).Distinct().ToList();
        //    List<city> city1 = new List<city>();
        //    city city = new city();
        //    city.name = "city";
        //    city.display_name = "City";
        //    List<value> value = new List<value>();
        //    for (int i = 0; i < citylist.Count; i++)
        //    {
        //        List<localities> locality1 = new List<localities>();
        //        value v = new value();
        //        v.name = citylist[i];
        //        v.id = i;
        //        var landmark = data.Where(m => m.City == v.name).Select(m => m.Landmark).Distinct().ToList();
        //        for (int j = 0; j < landmark.Count; j++)
        //        {
        //            localities loc = new localities();
        //            loc.name = landmark[j];
        //            loc.id = j;
        //            locality1.Add(loc);
        //        }
        //        v.localities = locality1;
        //        value.Add(v);
        //    }
        //    city.value = value;
        //    city1.Add(city);
        //    filter.city = city1;

        //    //Rating Section
        //    List<rating> rating1 = new List<rating>();
        //    rating rating = new rating();
        //    rating.name = "rating";
        //    rating.display_name = "Rating";
        //    string rlist = "All Ratings!Rated 3.0+!Rated 4.0+!Rated 5.0+";
        //    List<values> val = new List<values>();
        //    for (int i = 0; i < rlist.Split('!').Count(); i++)
        //    {
        //        values v = new values();
        //        v.name = rlist.Split('!')[i];
        //        v.id = i;
        //        val.Add(v);
        //    }
        //    rating.value = val;
        //    rating1.Add(rating);
        //    filter.rating = rating1;
        //    #endregion

        //    #region Venue
        //    if (type == "Venue")
        //    {
        //        val = new List<values>();
        //        //Venue Type Section
        //        List<venue_type> venue_type1 = new List<venue_type>();
        //        venue_type venue_type = new venue_type();
        //        venue_type.name = "venue_type";
        //        venue_type.display_name = "Venue Type";

        //        string list = "4 Star & Above Hotels!Banquet Hall!Lawn / Farmhouse!Hotels!Country / Golf Club!Resort!Restaurant / Lounge Bar!Heritage property";
        //        for (int i = 0; i < list.Split('!').Count(); i++)
        //        {
        //            values v = new values();
        //            v.name = list.Split('!')[i];
        //            v.id = i;
        //            val.Add(v);
        //        }
        //        venue_type.value = val;
        //        venue_type1.Add(venue_type);
        //        filter.venuetype = venue_type1;

        //        //Guests Section
        //        val = new List<values>();
        //        List<guests> guests1 = new List<guests>();
        //        guests guests = new guests();
        //        guests.name = "capacity";
        //        guests.display_name = "No. of Guests";
        //        string glist = "< 100!100-250!250-500!500-1000!> 1000";
        //        for (int i = 0; i < glist.Split('!').Count(); i++)
        //        {
        //            values v = new values();
        //            v.name = glist.Split('!')[i];
        //            v.id = i;
        //            val.Add(v);
        //        }
        //        guests.value = val;
        //        guests1.Add(guests);
        //        filter.guests = guests1;

        //        //Price per plate Section
        //        val = new List<values>();
        //        List<price_per_plate> price_per_plate1 = new List<price_per_plate>();
        //        price_per_plate price_per_plate = new price_per_plate();
        //        price_per_plate.display_name = "Price per plate";
        //        price_per_plate.name = "price_per_plate_or_rental";
        //        string pricelist = "< 1000!1000-1500!1500-2000!2000-3000!> 3000,Rental";
        //        for (int i = 0; i < pricelist.Split('!').Count(); i++)
        //        {
        //            values v = new values();
        //            v.name = pricelist.Split('!')[i];
        //            v.id = i;
        //            val.Add(v);
        //        }
        //        price_per_plate.value = val;
        //        price_per_plate1.Add(price_per_plate);
        //        filter.priceperplate = price_per_plate1;

        //        //Space preference Section
        //        val = new List<values>();
        //        List<space_preference> space_preference1 = new List<space_preference>();
        //        space_preference space_preference = new space_preference();
        //        space_preference.name = "space_preference";
        //        space_preference.display_name = "Space Preference";
        //        string spacelist = "Indoor!Outdoor!Indoor with outdoor!Terrace!Poolside";
        //        for (int i = 0; i < spacelist.Split('!').Count(); i++)
        //        {
        //            values v = new values();
        //            v.name = spacelist.Split('!')[i];
        //            v.id = i;
        //            val.Add(v);
        //        }
        //        space_preference.value = val;
        //        space_preference1.Add(space_preference);
        //        filter.space_preference = space_preference1;
        //    }
        //    #endregion

        //    dict.Add("filters", filter);
        //    return Json(dict);
        //}
    }
}
