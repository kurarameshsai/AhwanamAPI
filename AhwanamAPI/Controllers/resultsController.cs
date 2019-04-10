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
            public string service_price { get; set; }
        }

        public class price1
        {
            public string Rentalprice { get; set; }
            public string minprice { get; set; }
            public string maxprice { get; set; }
            public string vegprice { get; set; }
            public string nonvegprice { get; set; }
        }

        public class param
        {
            public long vendor_masterId { get; set; }
            public long vendor_serviceId { get; set; }
            //public int category_Id { get; set; }
            public string name { get; set; }
            public string page_name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public string description { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string city { get; set; }
            public prices price { get; set; }
            public string pic_url { get; set; }
            public string min_guest { get; set; }
            public string max_guest { get; set; }
        }

        //public class param2
        //{
        //    public long vendor_Id { get; set; }
        //    //public long vendor_serviceId { get; set; }
        //    public int category_Id { get; set; }
        //    public string name { get; set; }
        //    public string category_name { get; set; }
        //    public string reviews_count { get; set; }
        //    public string description { get; set; }
        //    public decimal rating { get; set; }
        //    public string charge_type { get; set; }
        //    public string city { get; set; }
        //    public price1 price { get; set; }
        //    public string pic_url { get; set; }
        //}

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
            //public List<value> value { get; set; }
            //public bool is_mutliple_selection { get; set; }
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
        [Route("api/results/getall")]                                                                                                                                                                                                                                       //Catering                                  //photography
        public IHttpActionResult getrecords(string type, int? city = 0, int? locality = 0, int? page = 0, int? capacity = 0, int? price_per_plate_or_rental = 0, int? offset = 0, int? sortby = 0, int? space_preference = 0, int? rating = 0, int? venue_type = 0,int? budget=0,int? dietary_prefernces = 0,int? services = 0)
        {
            string IP = HttpContext.Current.Request.UserHostAddress;
            int status = checktoken(IP); // Checking Token
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            
            //Type Venue
            if (type == "venue")
                dict = VenueRecords(type, city, locality, page, capacity, price_per_plate_or_rental, offset, sortby, space_preference, rating, venue_type);
            //Type Caterer
            else if (type == "catering")
                dict = CatererRecords(type, page, offset, rating,budget,dietary_prefernces,city);
            //Type Photography
            else if (type == "photography")
                dict = PhotographerRecords(type, page, offset, rating,city,services,budget);
            //Type Decorator
            else if (type == "decorator")
                dict = DecoratorRecords(type, page, offset, rating,budget,city);
            //Type Other
            else if (type == "pandit" || type == "mehendi")
                dict = OtherRecords(type, page, offset, rating);
            dict1.Add("status", true);
            dict1.Add("message", "Success");
            dict1.Add("data", dict);
            return Json(dict1);
        }

        #region Venue_Records
        public Dictionary<string, object> VenueRecords(string type, int? city = 0, int? locality = 0, int? page = 0, int? capacity = 0, int? price_per_plate_or_rental = 0, int? offset = 0, int? sortby = -1, int? space_preference = 0, int? rating = 0, int? venue_type = 0)
        {
            type = (type == "venue") ? "Venue" : type;
            int count = 0;
            //cookies((int)capacity, "venue");
            string guestsvalue = (capacity != 0 && capacity != null) ? getvalue((int)capacity) : null;
            string cityvalue = (city != 0 && city != null) ? getcity((int)city) : null;
            string localityvalue = (locality != 0 && locality != null) ? getvalue((int)locality) : null;
            string pricevalue = (price_per_plate_or_rental != 0 && price_per_plate_or_rental != null) ? getvalue((int)price_per_plate_or_rental) : null;
            string spacevalue = (space_preference != 0 && space_preference != null) ? getvalue((int)space_preference) : null;
            //string sortbyvalue = (sortby != -1 && sortby != null) ? getvalue((int)sortby) : null;
            string ratingvalue = (rating != 0 && rating != null) ? getvalue((int)rating) : null;
            string vtypevalue = (venue_type != 0 && venue_type != null) ? getvalue((int)venue_type) : null;
            page = (page == null) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;

            if (ratingvalue == "All Ratings") ratingvalue = "2";
            else if (ratingvalue == "Rated 3.0+") ratingvalue = "3";
            else if (ratingvalue == "Rated 4.0+") ratingvalue = "4";
            else if (ratingvalue == "Rated 5.0+") ratingvalue = "5";

            //Retrieve Cookie value
            var data = resultsPageService.GetAllVendors(type);//.Skip(page*6).ToList();
            count = data.Count();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            else
                data = data.Take((int)offset).ToList();
            if (cityvalue != null)
                data = data.Where(m => m.City == cityvalue).ToList();
            if (localityvalue != null && cityvalue != null)
                data = data.Where(m => m.Landmark == localityvalue).ToList();
            if (guestsvalue != null)
                data = data.Where(m => m.Minimumseatingcapacity > int.Parse(guestsvalue)).ToList();
            if (pricevalue != null)
                data = data.Where(m => m.cost1 > decimal.Parse(pricevalue)).ToList();
            if (sortby != null)
            {
                if(sortby == 1)
                //if (sortbyvalue == "price-high-to-low")
                    data = data.OrderByDescending(m => m.cost1).ToList();
            }
            //if (ratingvalue != null)
            //    data = data.Where(m => m.rat == localityvalue).ToList();
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
                    price.service_price = item.ServiceCost.ToString();

                    //Data Section
                    p.vendor_masterId = item.Id;
                    p.vendor_serviceId = item.subid;
                    p.name = item.BusinessName;
                    p.page_name = item.page_name;
                    p.category_name = item.ServicType;
                    ReviewService reviewService = new ReviewService();
                    p.reviews_count = reviewService.GetReview(int.Parse(item.Id.ToString())).Where(m => m.Sid == long.Parse(item.subid.ToString())).Count().ToString();
                    p.description = item.Description;
                    p.rating = (trating != 0) ? decimal.Parse((trating/3).ToString().Substring(0,4)) : 0;
                    p.charge_type = "Per Day";
                    p.latitude = "17.385044";
                    p.longitude = "78.486671";
                    p.city = item.City;
                    p.pic_url = "https://api.ahwanam.com/vendorimages/" + item.image;
                    p.price = price;
                    p.min_guest = item.Minimumseatingcapacity.ToString();
                    p.max_guest = item.Maximumcapacity.ToString();                    
                    param.Add(p);
                }
            }
            var records = param;
            if (ratingvalue != null)
                records = records.Where(m => m.rating >= decimal.Parse(ratingvalue)).ToList();
            #endregion

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", records);
            dict.Add("total_count", count);
            dict.Add("offset", (offset == null) ? 6 : offset);
            dict.Add("no_of_pages", ((count - 1) / offset) + 1);
            dict.Add("sort_options", (sortby == null) ? 0 : sortby);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Caterer_Records
        public Dictionary<string, object> CatererRecords(string type, int? page = 0, int? offset = 0, int? rating = 0,int? budget =0,int? dietary_prefernces=0, int? city = 0)
        {
            int count = 0;
            string cityvalue = (city != 0 && city != null) ? getcity((int)city) : null;
            string budgetvalue = (budget != 0) ? getvalue((int)budget) : null;
            string ratingvalue = (rating != 0) ? getvalue((int)rating) : null;
            string dietvalue = (dietary_prefernces != 0) ? getvalue((int)dietary_prefernces) : null;
            page = (page == null) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;

            if (ratingvalue == "All Ratings") ratingvalue = "2";
            else if (ratingvalue == "Rated 3.0+") ratingvalue = "3";
            else if (ratingvalue == "Rated 4.0+") ratingvalue = "4";
            else if (ratingvalue == "Rated 5.0+") ratingvalue = "5";
            //retrive the data
            var data = resultsPageService.GetAllCaterers();
            count = data.Count();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            else
                data = data.Take((int)offset).ToList();
            if (cityvalue != null)
                data = data.Where(m => m.City == cityvalue).ToList();
            if (budgetvalue != null)
                data = data.Where(m => decimal.Parse(m.MinOrder) >= decimal.Parse(budgetvalue)).ToList();

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
                    price.actual_price = item.Veg.ToString();
                    price.offer_price = item.Veg.ToString(); // Add Normal Days price here
                    price.service_price = "";

                    //Data Section
                    p.vendor_masterId = item.Id;
                    p.vendor_serviceId = item.subid;
                    p.name = item.BusinessName;
                    p.page_name = item.page_name;
                    p.category_name = item.ServicType;
                    ReviewService reviewService = new ReviewService();
                    p.reviews_count = reviewService.GetReview(int.Parse(item.Id.ToString())).Where(m => m.Sid == long.Parse(item.subid.ToString())).Count().ToString();
                    p.description = item.Description;
                    p.rating = (trating != 0) ? decimal.Parse((trating / 3).ToString().Substring(0, 4)) : 0;
                    p.charge_type = "Per Day";
                    p.latitude = "17.385044";
                    p.longitude = "78.486671";
                    p.city = item.City;
                    p.pic_url = "https://api.ahwanam.com/vendorimages/" + item.image;
                    p.price = price;
                    //p.min_guest = item.Minimumseatingcapacity.ToString();
                    //p.max_guest = item.Maximumcapacity.ToString();

                    param.Add(p);
                }
            }
            var records = param;
            if (ratingvalue != null)
                records = records.Where(m => m.rating >= decimal.Parse(ratingvalue)).ToList();
            #endregion

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", records);
            dict.Add("total_count", count);
            dict.Add("offset", (offset == null) ? 6 : offset);
            dict.Add("no_of_pages", ((count - 1) / offset) + 1);
            //dict.Add("sort_options", (sortby == null) ? 1 : sortby);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Decorator_Records
        public Dictionary<string, object> DecoratorRecords(string type, int? page = 0, int? offset = 0, int? rating = 0,int? budget =0, int? city = 0)
        {
            int count = 0;
            page = (page == null) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;
            var data = resultsPageService.GetAllDecorators();
            count = data.Count();
            string cityvalue = (city != 0 && city != null) ? getcity((int)city) : null;
            string budgetvalue = (budget != 0) ? getvalue((int)budget) : null;
            string ratingvalue = (rating != 0) ? getvalue((int)rating) : null;
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            else
                data = data.Take((int)offset).ToList();

            if (ratingvalue == "All Ratings") ratingvalue = "2";
            else if (ratingvalue == "Rated 3.0+") ratingvalue = "3";
            else if (ratingvalue == "Rated 4.0+") ratingvalue = "4";
            else if (ratingvalue == "Rated 5.0+") ratingvalue = "5";

            if (cityvalue != null)
                data = data.Where(m => m.City == cityvalue).ToList();
            if (budgetvalue != null)
                data = data.Where(m => m.cost1 >= decimal.Parse(budgetvalue)).ToList();
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
                    price.offer_price = item.cost1.ToString(); // Add Normal Days price here
                    price.service_price = "";

                    //Data Section
                    p.vendor_masterId = item.Id;
                    p.vendor_serviceId = item.subid;
                    p.name = item.BusinessName;
                    p.page_name = item.page_name;
                    p.category_name = item.ServicType;
                    ReviewService reviewService = new ReviewService();
                    p.reviews_count = reviewService.GetReview(int.Parse(item.Id.ToString())).Where(m => m.Sid == long.Parse(item.subid.ToString())).Count().ToString();
                    p.description = item.Description;
                    p.rating = (trating != 0) ? decimal.Parse((trating / 3).ToString().Substring(0, 4)) : 0;
                    p.charge_type = "Per Day";
                    p.latitude = "17.385044";
                    p.longitude = "78.486671";
                    p.city = item.City;
                    p.pic_url = "https://api.ahwanam.com/vendorimages/" + item.image;
                    p.price = price;
                    //p.min_guest = item.Minimumseatingcapacity.ToString();
                    //p.max_guest = item.Maximumcapacity.ToString();

                    param.Add(p);
                }
            }
            var records = param;
            if (ratingvalue != null)
                records = records.Where(m => m.rating >= decimal.Parse(ratingvalue)).ToList();
            #endregion
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", records);
            dict.Add("total_count", count);
            dict.Add("offset", (offset == null) ? 6 : offset);
            dict.Add("no_of_pages", ((count - 1) / offset) + 1);
            //dict.Add("sort_options", (sortby == null) ? 1 : sortby);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Photographer_Records
        public Dictionary<string, object> PhotographerRecords(string type, int? page = 0, int? offset = 0, int? rating = 0, int? city = 0, int? services = 0, int? budget = 0)
        {
            int count = 0;
            string servicesvalue = (services != 0) ? getcity((int)services) : null;
            string cityvalue = (city != 0 && city != null) ? getcity((int)city) : null;
            string budgetvalue = (budget != 0) ? getvalue((int)budget) : null;
            string ratingvalue = (rating != 0) ? getvalue((int)rating) : null;
            page = (page == null) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;

            if (ratingvalue == "All Ratings") ratingvalue = "2";
            else if (ratingvalue == "Rated 3.0+") ratingvalue = "3";
            else if (ratingvalue == "Rated 4.0+") ratingvalue = "4";
            else if (ratingvalue == "Rated 5.0+") ratingvalue = "5";

            var data = resultsPageService.GetAllPhotographers();
            count = data.Count();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            else
                data = data.Take((int)offset).ToList();

          
            if (cityvalue != null)
                data = data.Where(m => m.City == cityvalue).ToList();
            if (budgetvalue != null)
                data = data.Where(m => m.cost1 >= decimal.Parse(budgetvalue)).ToList();
            if (cityvalue != null)
                data = data.Where(m => m.City == cityvalue).ToList();
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
                    price.offer_price = item.cost1.ToString(); // Add Normal Days price here
                    price.service_price = "";

                    //Data Section
                    p.vendor_masterId = item.Id;
                    p.vendor_serviceId = item.subid;
                    p.name = item.BusinessName;
                    p.page_name = item.page_name;
                    p.category_name = item.ServicType;
                    ReviewService reviewService = new ReviewService();
                    p.reviews_count = reviewService.GetReview(int.Parse(item.Id.ToString())).Where(m => m.Sid == long.Parse(item.subid.ToString())).Count().ToString();
                    p.description = item.Description;
                    p.rating = (trating != 0) ? decimal.Parse((trating / 3).ToString().Substring(0, 4)) : 0;
                    p.charge_type = "Per Day";
                    p.latitude = "17.385044";
                    p.longitude = "78.486671";
                    p.city = item.City;
                    p.pic_url = "https://api.ahwanam.com/vendorimages/" + item.image;
                    p.price = price;
                    //p.min_guest = item.Minimumseatingcapacity.ToString();
                    //p.max_guest = item.Maximumcapacity.ToString();

                    param.Add(p);
                }
            }
            var records = param;
            if (ratingvalue != null)
                records = records.Where(m => m.rating >= decimal.Parse(ratingvalue)).ToList();
            #endregion
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", records);
            dict.Add("total_count", count);
            dict.Add("offset", (offset == null) ? 6 : offset);
            dict.Add("no_of_pages", ((count - 1) / offset) + 1);
            //dict.Add("sort_options", (sortby == null) ? 1 : sortby);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        #region Other_Records
        public Dictionary<string, object> OtherRecords(string type, int? page = 0, int? offset = 0, int? rating = 0)
        {
            int count = 0;
            string ratingvalue = (rating != 0) ? getvalue((int)rating) : null;
            page = (page == null) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;
            if (ratingvalue == "All Ratings") ratingvalue = "2";
            else if (ratingvalue == "Rated 3.0+") ratingvalue = "3";
            else if (ratingvalue == "Rated 4.0+") ratingvalue = "4";
            else if (ratingvalue == "Rated 5.0+") ratingvalue = "5";
            var data = resultsPageService.GetAllOthers(type);
            count = data.Count();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            else
                data = data.Take((int)offset).ToList();
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
                    price.actual_price = item.ItemCost.ToString();
                    price.offer_price = item.ItemCost.ToString(); // Add Normal Days price here
                    price.service_price = "";

                    //Data Section
                    p.vendor_masterId = item.Id;
                    p.vendor_serviceId = item.subid;
                    p.name = item.BusinessName;
                    p.page_name = item.page_name;
                    p.category_name = item.ServicType;
                    ReviewService reviewService = new ReviewService();
                    p.reviews_count = reviewService.GetReview(int.Parse(item.Id.ToString())).Where(m => m.Sid == long.Parse(item.subid.ToString())).Count().ToString();
                    p.description = item.Description;
                    p.rating = (trating != 0) ? decimal.Parse((trating / 3).ToString().Substring(0, 4)) : 0;
                    p.charge_type = "Per Day";
                    p.latitude = "17.385044";
                    p.longitude = "78.486671";
                    p.city = item.City;
                    p.pic_url = "https://api.ahwanam.com/vendorimages/" + item.image;
                    p.price = price;
                    //p.min_guest = item.Minimumseatingcapacity.ToString();
                    //p.max_guest = item.Maximumcapacity.ToString();

                    param.Add(p);
                }
            }
            var records = param;
            //if (ratingvalue != null)
            //  records = records.Where(m => m.rating == ratingvalue).ToList();
            if (ratingvalue != null)
                records = records.Where(m => m.rating >= decimal.Parse(ratingvalue)).ToList();
            #endregion
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("results", records);
            dict.Add("total_count", count);
            dict.Add("offset", (offset == null) ? 6 : offset);
            dict.Add("no_of_pages", ((count - 1) / offset) + 1);
            //dict.Add("sort_options", (sortby == null) ? 1 : sortby);
            dict.Add("service_type", type);
            return dict;
        }
        #endregion

        [HttpGet]
        [Route("api/results/getfilters1")]
        public IHttpActionResult getfilters1(string type)
        {
            type = (type == null) ? "Venue" : type;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            // Header Section
            header headers = new header();
            if (type == "venue")
                headers.header_text = "Best Wedding Venues";
            else if (type == "catering")
                headers.header_text = "Best Catering Vendors";
            else if (type == "decorator")
                headers.header_text = "Best Decorator Vendors";
            else if (type == "photography")
                headers.header_text = "Best Photography Vendors";
            else if (type == "pandit")
                headers.header_text = "Best Pandit Vendors";
            else if (type == "mehendi")
                headers.header_text = "Best Mehendi Vendors";
            headers.sub_text = "Sub Text";
            headers.image = "https://api.ahwanam.com/images/header1.png";
            dict.Add("header", headers);

            List<newfilter> filter = new List<newfilter>();
            newfilter f = new newfilter();
            //Sort Section
            List<sortby> sort1 = new List<sortby>();
            //string slist = "price-low-to-high!price-high-to-low";
            string slist = "Price low to high!Price high to low";
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
            //f.is_mutliple_selection = true;
            filter.Add(f);

            if (type == "venue" || type == "Photography" || type == "Catering")
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
                else if (type == "Photography")
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
                //f.is_mutliple_selection = true;
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
                //f.is_mutliple_selection = true;
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
                //f.is_mutliple_selection = true;
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
                //f.is_mutliple_selection = true;
                filter.Add(f);
            }
            if (type == "Photography" || type == "Decorator" || type == "Catering")
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
                //f.is_mutliple_selection = true;
                filter.Add(f);
            }

            //City Section
            VendorMasterService vendorMasterService = new VendorMasterService();
            var data = vendorMasterService.SearchVendors();
            var citylist = data.Select(m => m.City).Distinct().ToList();
            f = new newfilter();
            f.name = "city";
            f.display_name = "City";
            List<values> val1 = new List<values>();
            //List<newcity> city = new List<newcity>();

            for (int i = 0; i < citylist.Count; i++)
            {
                values c = new values();
                c.name = citylist[i];
                c.id = i;
                //var landmark = data.Where(m => m.City == c.name).Select(m => m.Landmark).Distinct().ToList();
                //List<localities> locality1 = new List<localities>();
                //for (int j = 0; j < landmark.Count; j++)
                //{
                //    localities loc = new localities();
                //    loc.name = landmark[j];
                //    loc.id = j;
                //    locality1.Add(loc);
                //}
                //c.localities = locality1;
                //city.Add(c);
                val1.Add(c);
            }
            //f.values = city;
            f.values = val1;
            //f.is_mutliple_selection = true;
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
        [Route("api/results/getfilters")]
        public IHttpActionResult getfilters(string type)
        {
            type = (type == null) ? "venue" : type;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> d1 = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            // Header Section
            header headers = new header();
            if (type == "venue")
                headers.header_text = "Best Wedding Venues";
            else if (type == "catering")
                headers.header_text = "Best Catering Vendors";
            else if (type == "decorator")
                headers.header_text = "Best Decorator Vendors";
            else if (type == "photography")
                headers.header_text = "Best Photography Vendors";
            else if (type == "pandit")
                headers.header_text = "Best Pandit Vendors";
            else if (type == "mehendi")
                headers.header_text = "Best Mehendi Vendors";
            headers.sub_text = "Sub Text";
            headers.image = "https://api.ahwanam.com/images/header1.png";
            d1.Add("header", headers);

            List<newfilter> filter = new List<newfilter>();
            newfilter f = new newfilter();
            //Sort Section
            List<sortby> sort1 = new List<sortby>();
            //string slist = "price-low-to-high!price-high-to-low";
            string slist = "Price low to high!Price high to low";
            for (int i = 0; i < slist.Split('!').Count(); i++)
            {
                sortby sort = new sortby();
                sort.name = slist.Split('!')[i];
                sort.id = i;
                sort1.Add(sort);
            }
            d1.Add("sort_options", sort1);
            FilterServices filterServices = new FilterServices();
            var categories = filterServices.AllCategories(); //Retrieving All Categories
            int value = categories.Where(m => m.display_name == type).FirstOrDefault().servicType_id; // Retrieving All Filters for a category
            var filters = filterServices.AllFilters(value);
            // Retrieving All Filter values for a category
            // newfilter f = new newfilter();
            //City & Locality Section
            VendorMasterService vendorMasterService = new VendorMasterService();
            var data = vendorMasterService.SearchVendors();
            var citylist = data.Select(m => m.City).Distinct().ToList();
            f = new newfilter();
            f.name = "city";
            f.display_name = "City";
            List<values> val1 = new List<values>();
            for (int i = 1; i < citylist.Count; i++)
            {
                values c = new values();
                c.name = citylist[i];
                c.id = i;
                //var landmark = data.Where(m => m.City == c.name).Select(m => m.Landmark).Distinct().ToList();
                //List<localities> locality1 = new List<localities>(); //locality
                //for (int j = 0; j < landmark.Count; j++)
                //{
                //    localities loc = new localities();
                //    loc.name = landmark[j];
                //    loc.id = j;
                //    locality1.Add(loc);
                //}
                //c.localities = locality1;
                val1.Add(c);
            }
            f.values = val1;
            //f.is_mutliple_selection = true;
            filter.Add(f);

            for (int i = 0; i < filters.Count; i++)
            {
                f = new newfilter();
                var filtervalue = filterServices.FilterValues(filters[i].filter_id);
                f.name = filters[i].name;
                f.display_name = filters[i].display_name;
                List<values> test = new List<values>();
                for (int j = 0; j < filtervalue.Count; j++)
                {
                    values v = new values();
                    v.name = filtervalue[j].name;
                    v.id = filtervalue[j].id;
                    test.Add(v);
                }
                f.values = test;
                //f.is_mutliple_selection = true;
                filter.Add(f);
            }
            d1.Add("filters", filter);
            //var data = filters(type,filter,dict);
            dict.Add("data", d1);
            return Json(dict);
        }

        //you can remove this code if not needed
        public Dictionary<string,object> filters(string type,List<newfilter> filter, Dictionary<string, object> dict)
        {
            FilterServices filterServices = new FilterServices();
            var categories = filterServices.AllCategories(); //Retrieving All Categories
            int value = categories.Where(m => m.display_name == type).FirstOrDefault().servicType_id; // Retrieving All Filters for a category
            var filters = filterServices.AllFilters(value);
            // Retrieving All Filter values for a category
            newfilter f = new newfilter();
            for (int i = 0; i < filters.Count; i++)
            {
                f = new newfilter();
                var filtervalue = filterServices.FilterValues(filters[i].filter_id);
                f.name = filters[i].name;
                f.display_name = filters[i].display_name;
                List<values> test = new List<values>();
                for (int j = 0; j < filtervalue.Count; j++)
                {
                    values v = new values();
                    v.name = filtervalue[j].name;
                    v.id = filtervalue[j].id;
                    test.Add(v);
                }
                f.values = test;
                //f.is_mutliple_selection = true;
                filter.Add(f);
            }
            //City & Locality Section
            VendorMasterService vendorMasterService = new VendorMasterService();
            var data = vendorMasterService.SearchVendors();
            var citylist = data.Select(m => m.City).Distinct().ToList();
            f = new newfilter();
            f.name = "city";
            f.display_name = "City";
            List<values> val1 = new List<values>();
            for (int i = 1; i < citylist.Count; i++)
            {
                values c = new values();
                c.name = citylist[i];
                c.id = i;
                //var landmark = data.Where(m => m.City == c.name).Select(m => m.Landmark).Distinct().ToList();
                //List<localities> locality1 = new List<localities>();
                //for (int j = 0; j < landmark.Count; j++)
                //{
                //    localities loc = new localities();
                //    loc.name = landmark[j];
                //    loc.id = j;
                //    locality1.Add(loc);
                //}
                //c.localities = locality1;
                val1.Add(c);
            }
            f.values = val1;
            //f.is_mutliple_selection = true;
            filter.Add(f);
            dict.Add("filters", filter);
            return dict;
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
        public string getvalue(int id)
        {
            FilterServices filterServices = new FilterServices();
            return filterServices.ParticularFilterValue(id).name;
        }

        public string getcity(int id)
        {
            VendorMasterService vendorMasterService = new VendorMasterService();
            var data = vendorMasterService.SearchVendors();
            var citylist = data.Select(m => m.City).Distinct().ToList();
            List<value> val1 = new List<value>();
            for (int i = 1; i < citylist.Count; i++)
            {
                value c = new value();
                c.name = citylist[i];
                c.id = i;
                val1.Add(c);
            }
            string city = val1.Where(m => m.id == id).FirstOrDefault().name;
            return city;
        }

        //can remove this methos if not needed just kept for reference purpose
        public string cookies(int first, string type, int? second = 0)
        {
            string returnvalue = string.Empty;
            //var filterdata = filters(type).Values.ToList();
            //if (filterdata.Count > 0)
            //{
            //    var rating = filterdata.FirstOrDefault().
            //    //d1 = filterdata[0];
            //}
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
                //city.value = fil[5].value;
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



        public int checktoken(string IP)
        {
            var re = Request;
            var customheader = re.Headers;
            if (customheader.Contains("token"))
            {
                string token = customheader.GetValues("token").First();
                UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
                int count = userlogindetailsservice.checktoken(token, IP);
                if (count != 0) return 1;
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
