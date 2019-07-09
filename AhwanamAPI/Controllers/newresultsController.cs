﻿using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using MaaAahwanam.Models;
using System.Globalization;

namespace AhwanamAPI.Controllers
{
    public class newresultsController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        WhishListService wishlistservice = new WhishListService();

        public class prices
        {
            
            public string minimum_price { get; set; }
            public string format_price { get; set; }
        }

         public class param2
        {
            public long vendor_id { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            //public string description { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public prices price { get; set; }
            public string pic_url { get; set; }
            public bool is_in_wishlist { get; set; }
        }

        public class param5
        {
            public long vendor_id { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            //public string description { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public prices price { get; set; }
            public string pic_url { get; set; }
            public bool is_in_wishlist { get; set; }
        }
        public class param
        {
            public long vendor_id { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public string description { get; set; }
            public string address { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public packages packages { get; set; }
            public string pic_url { get; set; }
        }
        public class param3
        {
            public long vendor_id { get; set; }
            //public long vendor_serviceId { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public string description { get; set; }
            public decimal rating { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public List<packages1> packages { get; set; }           
            public string pic_url { get; set; }
            public bool is_in_wishlist { get; set; }
            public metatag metatag { get; set; }
            public List<Amenities> amenities { get; set; }
            public List<Policies> policies { get; set; }
            public List<availableareas> availableareas { get; set; }

          
        }

        public class param4
        {
            public long vendor_id { get; set; }
            //public long vendor_serviceId { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public string description { get; set; }
            public decimal rating { get; set; }
            public string address { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public packages packages { get; set; }
            public string pic_url { get; set; }
            //public location location { get; set; }
            //public List<Amenities> amenities { get; set; }
            //public List<Policies> Policies { get; set; }
            //public List<availableareas> availableareas { get; set; }
        }

        public class location
        {
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class Amenities
        {
            public long amenity_id { get; set; }
            public string amenity_name { get; set; }
            public string amenity_icon { get; set; }
        }

        public class availableareas
        {
            public long area_id { get; set; }
            public string area_name { get; set; }
            public string area_icon { get; set; }
            public decimal rent_price { get; set; }
            public int seating_capacity { get; set; }
            public string type { get; set; }
        }

        public class Policies
        {
            public long policy_id { get; set; }
            public string policy { get; set; }
            public string policy_icon { get; set; }
        }

        public class metatag
        {
            public string title { get; set; }
            public string description { get; set; }
            public string keywords { get; set; }
        }

        public class header
        {
            //public metatag metatag { get; set; }
            public string category_name { get; set; }
            public string header_text { get; set; }
            public string sub_text { get; set; }
            public string image { get; set; }
            
        }
        public class Reviews
        {
            public string rating { get; set; }
            public string name { get; set; }
            public string review { get; set; }
          
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
            //public List<value> value { get; set; }
            //public bool is_mutliple_selection { get; set; }
        }

        public class newcity
        {
            public string name { get; set; }
            public long id { get; set; }
            public List<localities> localities { get; set; }
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

        public class packages1
        {
            public string name { get; set; }
            public string charge_type { get; set; }
            public string price { get; set; }
            public string format_price { get; set; }
        }

        //public class prices2

        public class services
        {
            public string name { get; set; }
            public string page_name { get; set; }
            public int category_id { get; set; }
            //public string image { get; set; }
            public List<param5> vendors { get; set; }
            //public List<Vendormasterdata> vendors { get; set; }
        }

        public class test
        {
            public long vendor_id { get; set; }
            public string name { get; set;}
            public int category_id { get; set; }
            public string category_name { get; set; }
            public string city { get; set; }
            public int priority { get; set; }
            public string charge_type { get; set; }
            public string ServiceType { get; set; }
            public decimal minimum_price { get; set; }
            public decimal veg_price { get; set; }
           
        }

        public class packages
        {
            public string Rentalprice { get; set; }
            public decimal minimum_price { get; set; }
            public decimal maximum_price { get; set; }
            public decimal veg_price { get; set; }
            public decimal nonveg_price { get; set; }
        }

        public class VImages
        {
            //public long vendor_id { get; set; }
            public string image { get; set; }
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

        public string getvalue(int id)
        {
            FilterServices filterServices = new FilterServices();
            return filterServices.ParticularFilterValue(id).name;
        }

        public string getvalue2(int id)
        {
            FilterServices filterServices = new FilterServices();
            return filterServices.newfiltervalue(id).name;
        }

        [HttpGet]
        [Route("api/getfilters")]
        public IHttpActionResult getfilters(int category_id)
        {
            string type = "";
            FilterServices filterServices1 = new FilterServices();
            var categories1 = filterServices1.getcategory(category_id);
               //var categories1 = filterServices1.category(category_id);
               type = (type == null) ? "venue" : type;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> d1 = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            // Header Section
            header headers = new header();
            if (categories1.name == "Venues")
            { headers.header_text = "Wedding Venues"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/venus_hero_image_generic.jpg"; }
            else if (categories1.name == "Caterers")
            { headers.header_text = "Catering Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/caterers_hero_image_generic.jpg"; }
            else if (categories1.name == "Decorators")
            { headers.header_text = "Decorator Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/decor_hero_image_generic.jpg"; }
            else if (categories1.name == "Photographers")
            { headers.header_text = "Photography Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/Photography_hero_image_generic.jpg"; }
            else if (categories1.name == "Pandits")
            { headers.header_text = "Pandit Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/pandit_hero_image_generic.jpg"; }
            else if (categories1.name == "Mehendi Artists")
            { headers.header_text = "Mehendi Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/mehendi_hero_image_generic.jpg"; }
            else if (categories1.name == "Makeup Artists")
            { headers.header_text = "Makeup Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/makeup_hero_image_generic.jpg"; }
            else if (categories1.name == "DJ")
            { headers.header_text = "DJ Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/dj_hero_image_generic.jpg"; }
            else if (categories1.name == "Choreographers")
            { headers.header_text = "Choreography Vendors"; headers.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "genericimages" + "/choreographer_hero_image_generic.jpg"; }
            headers.sub_text = "Sub Text";
            headers.category_name = categories1.name;
            metatag tag = new metatag(); //meta tags of category
            tag.title = categories1.MetatagTitle.Trim();
            tag.description = categories1.MetatagDesicription.Trim();
           tag.keywords = categories1.MetatagKeywords.Trim();
            d1.Add("metatag", tag);
            d1.Add("header", headers);

            List<newfilter> filter = new List<newfilter>();
            newfilter f = new newfilter();
            //Sort Section
            List<sortby> sort1 = new List<sortby>();
            string slist = "Price low to high!Price high to low";
            for (int i = 0; i < slist.Split('!').Count(); i++)
            {
                sortby sort = new sortby();
                sort.name = slist.Split('!')[i];
                sort.id = i;
                sort1.Add(sort);
            }
            d1.Add("sort_options", sort1);
            //FilterServices filterServices = new FilterServices();
            //var categories = filterServices.AllCategories(); //Retrieving All Categories
          /* int value = categories.Where(m => m.display_name == type).FirstOrDefault().servicType_id;*/ // Retrieving All Filters for a category
            int value = categories1.servicType_id;
            var filters = filterServices1.AllFilters(value);
          
            for (int i = 0; i < filters.Count; i++)
            {
                f = new newfilter();
                var filtervalue = filterServices1.newFilterValues(filters[i].filter_id);
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
                filter.Add(f);
            }
            d1.Add("filters", filter);
            dict.Add("data", d1);
            return Json(dict);
        }

        public string getcity(int id)
        {
            var city = "";
            ResultsPageService resultsPageService = new ResultsPageService();
            //var result = resultsPageService.Getvendormasterdata();
            //var citylist = result.Select(m => m.City).Distinct().ToList();
            var citylist = new List<string>() { "Hyderabad", "Secunderabad", "Vijayawada", "Vizag" };
            List<value> val1 = new List<value>();
            for (int i = 0; i <= citylist.Count; i++)
            {
                value c = new value();
                c.name = citylist[i];
                c.id = i;
                val1.Add(c);
            }
            var c3 = val1;
            //var city = val1.Where(m => m.id == id).FirstOrDefault().name;
            var city3 = val1.Where(m => m.id == id).FirstOrDefault();
            if (city3 != null)
            {
                city = city3.name;

            }
            else
            {
                city = "empty";
            }
            return city;
        }


        [HttpGet]
        [Route("api/allvendors")]
        public IHttpActionResult allvendors()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            var categories = resultsPageService.getallcategories();
            string token = null;
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                token = customheader.GetValues("Authorization").First();
            }
            var data = wishlistservice.getalldetailsofvendors(token).OrderByDescending(v => v.priority);
            List<services> res = new List<services>();
            foreach (var items in categories)
            {
                services result = new services();
                result.name = items.name;
                result.page_name = items.display_name;
                result.category_id = items.servicType_id;
                var details = data.Where(a => a.Category_TypeId == items.servicType_id).Take(7).ToList();
                /*var data = wishlistservice.getvendorsalldetails(token, items.servicType_id).OrderByDescending(v => v.priority).Take(7).ToList();*/ //dbcall
                List<param5> param = new List<param5>();
                foreach (var item in details)
                {
                    param5 p = new param5();
                    p.vendor_id = item.VendorID;
                    p.category_id = item.Category_TypeId;
                    p.category_name = item.name.Trim();
                    p.name = item.BusinessName.Trim();
                    p.city = item.City;
                    p.pic_url = System.Configuration.ConfigurationManager.AppSettings["imagename"] + item.VendorID + "/main.jpg";
                    //prices Section
                    prices price = new prices();
                    //price.Rentalprice = item.RentAmount.ToString();
                    if (item.ServiceType == "Function Hall")
                    {
                        int cost = (int)item.RentAmount;
                        price.minimum_price = Convert.ToString(cost);
                        if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                        if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                    }
                    if (p.category_name == "Venues" || p.category_name == "Caterers")
                    {
                        var mny = item.VegPrice.ToString("N", CultureInfo.CreateSpecificCulture("en-IN")).Split('.');
                        price.minimum_price = mny[0];
                        price.format_price = Convert.ToString(mny[0]);
                    }
                    else
                    {
                        int cost = (int)item.MinPrice;

                        price.minimum_price = Convert.ToString(cost);
                        if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                        if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                    }
                    p.price = price;
                    p.is_in_wishlist = Convert.ToBoolean(item.is_in_ishlist);
                    p.rating = item.Rating;
                    p.reviews_count = resultsPageService.Getreviews(p.vendor_id).Count.ToString(); //dbcall for reviews count
                    p.charge_type = item.Type_of_price;
                    param.Add(p);
                }
                result.vendors = param;
                res.Add(result);
            }
            dict1.Add("categories", res);
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("data", dict1);
            return Json(dict);
        }


        [HttpGet]
        [Route("api/vendors")]
        public IHttpActionResult vendors()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            var categories = resultsPageService.getallcategories();
            string token=null;
            List<services> res = new List<services>();
            foreach (var items in categories)
            {
                services result = new services();
                result.name = items.name;
                result.page_name = items.display_name;
                result.category_id = items.servicType_id;
                var re = Request;
                var customheader = re.Headers;
                UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
                if (customheader.Contains("Authorization"))
                {
                    token = customheader.GetValues("Authorization").First();
                }
                    var data = wishlistservice.getvendorsalldetails(token, items.servicType_id).OrderByDescending(v => v.priority).Take(7).ToList(); //dbcall
                    List<param5> param = new List<param5>();
                    foreach (var item in data)
                    {
                        param5 p = new param5();
                        p.vendor_id = item.VendorID;
                        p.category_id = item.Category_TypeId;
                        p.category_name = item.name.Trim();
                        p.name = item.BusinessName.Trim();
                        p.city = item.City;
                        p.pic_url = System.Configuration.ConfigurationManager.AppSettings["imagename"] + item.VendorID + "/main.jpg";
                        //prices Section
                        prices price = new prices();
                        //price.Rentalprice = item.RentAmount.ToString();
                        if (item.ServiceType == "Function Hall")
                        {
                            int cost = (int)item.RentAmount;
                            price.minimum_price = Convert.ToString(cost);
                            if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                            if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                        }
                        if (p.category_name == "Venues" || p.category_name == "Caterers")
                        {
                            var mny = item.VegPrice.ToString("N", CultureInfo.CreateSpecificCulture("en-IN")).Split('.');
                            price.minimum_price = mny[0];
                            price.format_price = Convert.ToString(mny[0]);
                        }
                        else
                        {
                            int cost = (int)item.MinPrice;

                            price.minimum_price = Convert.ToString(cost);
                            if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                            if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                        }
                        p.price = price;
                        p.is_in_wishlist = Convert.ToBoolean(item.is_in_ishlist);
                        p.rating = item.Rating;
                        p.reviews_count = resultsPageService.Getreviews(p.vendor_id).Count.ToString(); //dbcall for reviews count
                        p.charge_type = item.Type_of_price;
                        param.Add(p);
                    }
                    result.vendors = param;
                res.Add(result);
            }
            dict1.Add("categories", res);
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("data", dict1);
            return Json(dict);
        }


        [HttpGet]
        [Route("api/getall")]
        public IHttpActionResult GetallVendordetails(int category_id, int? city = -1, int? rating = 0, int? offset = 0, int? page = 0, int? capacity = 0, int? venue_type = 0, int? space_preference = 0, int? locality = 0, int? price_per_plate_or_rental = 0, int? sortby = -1, int? budget = 0)
        {
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            List<param2> param = new List<param2>();
            int count = 0;
            string token = null;
            string guestvalue1 = "";
            string pricevalue1 = "";
            string budgetvalue1 = "";
            string ratingvalue = (rating != 0 && rating != null) ? getvalue2((int)rating) : null;
            string cityvalue = (city != -1 && city != null) ? getcity((int)city) : null;
            string guestsvalue = (capacity != 0 && capacity != null) ? getvalue2((int)capacity) : null;
            string spacevalue = (space_preference != 0 && space_preference != null) ? getvalue2((int)space_preference) : null;
            string vtypevalue = (venue_type != 0 && venue_type != null) ? getvalue2((int)venue_type) : null;
            string localityvalue = (locality != 0 && locality != null) ? getvalue2((int)locality) : null;
            string pricevalue = (price_per_plate_or_rental != 0 && price_per_plate_or_rental != null) ? getvalue2((int)price_per_plate_or_rental) : null;
            string budgetvalue = (budget != 0 && budget != null) ? getvalue2((int)budget) : null;
            page = (page == null) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;

            if (budgetvalue == "< 50k") { budgetvalue = "0"; budgetvalue1 = "50000"; }
            else if (budgetvalue == "50k - 2L") { budgetvalue = "50000"; budgetvalue1 = "200000"; }
            else if (budgetvalue == "2L - 3.5L") { budgetvalue = "200000"; budgetvalue1 = "350000"; }
            else if (budgetvalue == "3.5L - 4.5L") { budgetvalue = "350000"; budgetvalue1 = "450000"; }
            else if (budgetvalue == "> 4.5L") { budgetvalue = "450000"; budgetvalue1 = "700000"; }

            if (pricevalue == "< 1,000") { pricevalue = "0"; pricevalue1 = "1000"; }
            else if (pricevalue == "1,000-1,500") { pricevalue = "1000"; pricevalue1 = "1500"; }
            else if (pricevalue == "1,500-2,000") { pricevalue = "1500"; pricevalue1 = "2000"; }
            else if (pricevalue == "2,000-3,000") { pricevalue = "2000"; pricevalue1 = "3000"; }
            else if (pricevalue == "> 3,000") { pricevalue = "3000"; pricevalue1 = "4000"; }
            else if (pricevalue == "Rental") { pricevalue = "Rent"; }

            if (guestsvalue == "< 100") { guestsvalue = "0"; guestvalue1 = "100"; }
            else if (guestsvalue == "100-250") { guestsvalue = "100"; guestvalue1 = "250"; }
            else if (guestsvalue == "250-500") { guestsvalue = "250"; guestvalue1 = "500"; }
            else if (guestsvalue == "500-1,000") { guestsvalue = "500"; guestvalue1 = "1000"; }
            else if (guestsvalue == "> 1,000") { guestsvalue = "1000"; guestvalue1 = "2000"; }

            if (ratingvalue == "Rated 2.0+") ratingvalue = "2";
            else if (ratingvalue == "Rated 3.0+") ratingvalue = "3";
            else if (ratingvalue == "Rated 4.0+") ratingvalue = "4";
            else if (ratingvalue == "Rated 5.0+") ratingvalue = "5";

            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                token = customheader.GetValues("Authorization").First();
            }
            var data = wishlistservice.getvendorsalldetails(token, category_id).OrderByDescending(v => v.priority).ToList();
            //var data = resultsPageService.GetvendorbycategoryId(category_id).OrderByDescending(v => v.priority).ToList();
            if (cityvalue != null || cityvalue == "empty")
                data = data.Where(m => m.City == cityvalue).ToList();
                     
            if (guestsvalue != null)
                   data = data.Where(m => m.Capacity > int.Parse(guestsvalue) && m.Capacity <= int.Parse(guestvalue1)).ToList();
            if (pricevalue != null)
            {
                if (category_id == 1 || category_id == 2)
                {
                    if (pricevalue == "Rent")
                        data = data.Where(m => m.Type_of_price == pricevalue).ToList();
                    else { 
                    data = data.Where(m => m.VegPrice >= decimal.Parse(pricevalue) && m.VegPrice <= decimal.Parse(pricevalue1)).ToList();
                    }
                }
            }
            if (budgetvalue != null)
            {
                data = data.Where(m => m.MinPrice >= decimal.Parse(budgetvalue) && m.MinPrice <= decimal.Parse(budgetvalue1)).ToList();
            }
                
            if (sortby != null && sortby != 0)
            {
                if (sortby == 1)
                {  //if (sortbyvalue == "price-high-to-low")
                    if (category_id == 1 || category_id == 2)
                        data = data.OrderByDescending(m => m.VegPrice).ToList();
                    else
                        data = data.OrderByDescending(m => m.MinPrice).ToList();
                }    
            }
            else if(sortby == 0)
            {
                if (category_id == 1 || category_id == 2)
                    data = data.OrderBy(m => m.VegPrice).ToList();
                else
                    data = data.OrderBy(m => m.MinPrice).ToList();
            }
            count = data.Count();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    param2 p = new param2();
                    p.vendor_id = item.VendorID;
                    p.category_id = item.Category_TypeId;
                    p.category_name = item.name.Trim();
                    p.name = item.BusinessName.Trim();
                    p.rating = item.Rating;
                    var data1 = resultsPageService.Getreviews(item.VendorID);
                    p.reviews_count = data1.Count().ToString();
                    //string d = (item.Description != null) ? item.Description.Trim() : null;
                    //p.description = d;
                    p.charge_type = item.Type_of_price;
                    p.city = item.City;
                    p.pic_url = System.Configuration.ConfigurationManager.AppSettings["imagename"] + item.VendorID + "/main.jpg";
                    //prices Section
                    prices price = new prices();
                    //price.Rentalprice = item.RentAmount.ToString();
                    if (item.name.Trim() == "Venues" || item.name.Trim() == "Caterers")
                    {
                        var mny = item.VegPrice.ToString("N", CultureInfo.CreateSpecificCulture("en-IN")).Split('.');
                        price.minimum_price = mny[0];
                        price.format_price = Convert.ToString(mny[0]);
                        if(item.ServiceType == "Function Hall")
                        {
                            int cost = (int)item.RentAmount;
                            price.minimum_price = Convert.ToString(cost);
                            if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                            if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                        }
                    }
                    else
                    {
                        int cost = (int)item.MinPrice;
                        price.minimum_price = Convert.ToString(cost);
                        if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                        if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }

                    }
                    p.price = price;
                    p.is_in_wishlist = Convert.ToBoolean(item.is_in_ishlist);
                    param.Add(p);
                }          
            var records = param;
                if (page > 1)
                    records = records.Skip(takecount).Take((int)offset).ToList();
                else
                    records = records.Take((int)offset).ToList();
                if (rating != 0)
                records = records.Where(m => m.rating >= decimal.Parse(ratingvalue)).ToList();
            dict1.Add("results", records);
            dict1.Add("total_count", count);
            dict1.Add("offset", (offset == null) ? 6 : offset);
            dict1.Add("no_of_pages", ((count - 1) / offset) + 1);
            dict1.Add("sort_options", (sortby == null) ? 0 : sortby);
            }
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("data", dict1);
            return Json(dict);
        }


        [HttpGet]
        [Route("api/getsimilarvendors")]
        public IHttpActionResult getsimilarsuppliers(long vendor_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var details = resultsPageService.Getsupplier(vendor_id);
            var data = resultsPageService.GetvendorbycategoryId(details.Category_TypeId).Where(v=>v.VendorId!= vendor_id).Take(3).ToList();
            List<param5> param = new List<param5>();
            for (int i = 0; i < data.Count; i++)
            {
                param5 p = new param5();
                p.vendor_id = data[i].VendorId;
                p.category_id = data[i].Category_TypeId;
                p.category_name = data[i].name.Trim();
                p.name = data[i].BusinessName.Trim();
                p.rating = data[i].Rating;
                var data1 = resultsPageService.Getreviews(data[i].VendorId);
                p.reviews_count = data1.Count().ToString();
                //p.reviews_count = data[i].ReviewsCount.ToString();
                //p.description = data[i].Description.Trim();
                p.charge_type = data[i].Type_of_price;
                p.city = data[i].City;
                p.pic_url = System.Configuration.ConfigurationManager.AppSettings["imagename"] + data[i].VendorId + "/main.jpg";
                //prices Section
                prices price = new prices();
                //price.Rentalprice = item.RentAmount.ToString();
                if (p.category_name == "Venues" || p.category_name == "Caterers")
                {
                    var mny = data[i].VegPrice.ToString("N", CultureInfo.CreateSpecificCulture("en-IN")).Split('.');
                    price.minimum_price = mny[0];
                    price.format_price = Convert.ToString(mny[0]);
                    if (data[i].ServiceType == "Function Hall")
                    {
                        int cost = (int)data[i].RentAmount;
                        price.minimum_price = Convert.ToString(cost);
                        if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                        if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                    }
                }
                else
                {
                    int cost = (int)data[i].MinPrice;
                    price.minimum_price = Convert.ToString(cost);
                    if(cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                    if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }

                }
                p.price = price;

                param.Add(p);
            }
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            if (param!=null)
            {
                dict1.Add("results", param);
                dict.Add("status", true);
                dict.Add("message", "Success");
                dict.Add("data", dict1);

            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                dict.Add("results", null);
            }
            return Json(dict);
        }


        [HttpGet]
        [Route("api/getallvendordetails")]
        public IHttpActionResult getdetails(long vendor_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            param3 p = new param3();
            var details = resultsPageService.Getsupplier(vendor_id);
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var detail = userlogindetailsservice.Getmyprofile(token);
                if (detail != null)
                {
                    var itemavailabe = wishlistservice.getwishlistitemdetail(detail.UserLoginId);
                    foreach (var a in itemavailabe)
                    {
                        if (a.vendorId == vendor_id)
                        {
                            p.is_in_wishlist = true;
                        }
                       
                    }

                }
            }
            else
            {
                p.is_in_wishlist = false;
            }
            p.category_id = details.Category_TypeId;
            p.vendor_id = details.VendorId;
            p.category_name = details.name;
            p.name = details.BusinessName;
            p.address = details.Address.Trim();
            p.city = details.City;
            string d = (details.Description != null) ? details.Description.Trim() : null;
            p.description = d;
            p.rating = details.Rating;
            var data1 = resultsPageService.Getreviews(details.VendorId);
            p.reviews_count = data1.Count().ToString();
            //p.reviews_count = details.ReviewsCount.ToString();
            p.pic_url = System.Configuration.ConfigurationManager.AppSettings["imagename"] + details.VendorId + "/banner.jpg"; ;
            //location lc = new location();
            //lc.latitude = "17.385044";
            //lc.longitude = "78.486671";
            //p.location = lc;
            List<packages1> pkg = new List<packages1>();
            //price.Rentalprice = details.RentAmount.ToString();
            
            if (details.name == "Venues" && details.ServiceType == "Function Hall")
            {
                packages1 price = new packages1();
                price = new packages1();
                price.name = "Rental Price";
                int cost = (int)details.RentAmount;
                price.price = Convert.ToString(cost);
                price.charge_type = details.Type_of_price;
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);
            }
            else { 
            if (p.category_name == "Venues" || p.category_name == "Caterers")
            {
                packages1 price = new packages1();
                price.name = "Vegetarian";
                price.charge_type= details.Type_of_price;
                var mny = details.VegPrice.ToString("N", CultureInfo.CreateSpecificCulture("en-IN")).Split('.');
                price.price = mny[0];
                price.format_price = Convert.ToString(mny[0]);
                pkg.Add(price);
                  //nonveg
                  price = new packages1();
                price.name = "Non Vegetarian";
                price.charge_type = details.Type_of_price;
                var mny1 = details.NonvegPrice.ToString("N", CultureInfo.CreateSpecificCulture("en-IN")).Split('.');
                price.price = mny1[0];
                price.format_price = Convert.ToString(mny1[0]);
                pkg.Add(price);

            }
            else if(p.category_name == "Photographers")
            {

                packages1 price = new packages1();
                price.name = "Photography";
                price.charge_type= details.Type_of_price;
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);
            } 
            else if(p.category_name == "Decorators")
            {
                packages1 price = new packages1();
                price.name = "Decoration";
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if(cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);
            }
            else if (p.category_name == "Pandits")
            {
                packages1 price = new packages1();
                price.name = "Pandits";
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);

            }
            else if (p.category_name == "Mehendi Artists")
            {
                packages1 price = new packages1();
                price.name = "Mehendi";
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);

            }
            else if(p.category_name == "Makeup Artists")
            {
                packages1 price = new packages1();
                price.name = "Makeup";
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);
            }
            else if (p.category_name == "DJ")
            {
                packages1 price = new packages1();
                price.name = "DJ";
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);
            }
            else if (p.category_name == "Choreographers")
            {
                packages1 price = new packages1();
                price.name = "Choreographers";
                int cost = (int)details.MinPrice;
                price.price = Convert.ToString(cost);
                if (cost >= 10000) { int value = cost / 1000; price.format_price = value.ToString() + 'k'; }
                if (cost >= 100000) { int value = cost / 100000; price.format_price = value.ToString() + 'L'; }
                pkg.Add(price);
            }
            }
            p.packages = pkg;
            metatag tag = new metatag();
            tag.description= details.MetatagDesicription.Trim();
            tag.keywords = details.MetatagKeywords.Trim();
            tag.title = details.MetatagTitle.Trim();
            p.metatag = tag;
            var amenitydetail= resultsPageService.GetAmenities(p.vendor_id);
            List<Amenities> amenitys = new List<Amenities>();
            foreach(var item in amenitydetail)
            {
                Amenities amenity = new Amenities();
                amenity.amenity_id = item.AmenityId;
                amenity.amenity_name = item.Amenity.Trim();
                amenity.amenity_icon = item.AmenityIcon;
                amenitys.Add(amenity);
            }
            p.amenities = amenitys;
            var policiesdeatils = resultsPageService.GetPolicies(p.vendor_id);
            List<Policies> policys = new List<Policies>();
            foreach(var policitem in policiesdeatils)
            {
                Policies policy = new Policies();
                policy.policy_id = policitem.PolicyId;
                policy.policy = policitem.Policy.Trim();
                policy.policy_icon = policitem.PolicyIcon;
                policys.Add(policy);
            }
            p.policies = policys;
            var detailareas = resultsPageService.GetavailableAreas(p.vendor_id);
            List<availableareas> listava = new List<availableareas>();
            foreach(var availableitem in detailareas)
            {
                availableareas area = new availableareas();
                area.area_id = availableitem.AreaId;
                area.area_name = availableitem.FeatureName.Trim();
                area.area_icon = availableitem.AreaIcon;
                area.seating_capacity = availableitem.SeatingCapacity;
                area.rent_price = availableitem.RentalPrice;
                area.type = availableitem.Area;
                listava.Add(area);
            }
            p.availableareas = listava;
            if (p != null)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                dict.Add("data", p);
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                dict.Add("data", p);
            }
            return Json(dict);
        }
  

        [HttpGet]
        [Route("api/availableareas")]
        public IHttpActionResult Getavailableareas(long vendor_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var detailareas = resultsPageService.GetavailableAreas(vendor_id);
            List<availableareas> listava = new List<availableareas>();
            if(detailareas.Count > 0)
            { 
            foreach (var availableitem in detailareas)
            {
                    availableareas area = new availableareas();
                    area.area_id = availableitem.AreaId;
                    area.area_name = availableitem.FeatureName.Trim();
                    area.area_icon = availableitem.AreaIcon;
                    area.seating_capacity = availableitem.SeatingCapacity;
                    area.rent_price = availableitem.RentalPrice;
                    area.type = availableitem.Area;
                    listava.Add(area);
                }
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("data", listava);
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                dict.Add("data", listava);
            }
            return Json(dict);

        }


        [HttpGet]
        [Route("api/amenities")]
        public IHttpActionResult GetAmenities(long vendor_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var deatils = resultsPageService.GetAmenities(vendor_id);
            List<Amenities> amenities = new List<Amenities>();
            if (deatils.Count > 0)
            {
                foreach (var item in deatils)
                {
                    Amenities a = new Amenities();
                    a.amenity_id = item.AmenityId;
                    a.amenity_name = item.Amenity.Trim();
                    a.amenity_icon = item.AmenityIcon;
                    amenities.Add(a);
                }
                dict.Add("status", true);
                dict.Add("message", "Success");
                dict.Add("data", amenities);
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "Failed");
                dict.Add("data", amenities);
            }
            return Json(dict);
        }

        [HttpGet]
        [Route("api/gallery")]
        public IHttpActionResult Getgalleryimages(long vendor_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            List<VImages> ilist = new List<VImages>();
            var gallaryimages = resultsPageService.Getimages(vendor_id);
                foreach(var item in gallaryimages)
                {
                    VImages i = new VImages();
                    //i.vendor_id = item.VendorId;
                    i.image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + vendor_id + "/" + item.ThumbnailUrl;
                    ilist.Add(i);
                }
                dict1.Add("gallery", ilist);
                dict.Add("status", true);
                dict.Add("message", "Success");
                dict.Add("data", dict1);
            
            return Json(dict);

        }
        [HttpGet]
        [Route("api/reviews")]
        public IHttpActionResult Getreviews(long vendor_id, int? page = 0, int? offset = 0)
        {
            int count = 0;
            page = (page == null || page == 0) ? 1 : page;
            offset = (offset == null || offset == 0) ? 6 : offset;
            int takecount = 6;
            if (((int)page - 1) > 0)
                takecount = ((int)page - 1) * (int)offset;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            var data = resultsPageService.Getreviews(vendor_id).OrderByDescending(r => r.rating).ToList();
            count = data.Count();
            if (page > 1)
                data = data.Skip(takecount).Take((int)offset).ToList();
            else
                data = data.Take((int)offset).ToList();
            List<Reviews> reviewlist = new List<Reviews>();
            if (data.Count > 0)
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
                foreach (var item in data)
                {
                    Reviews r = new Reviews();
                    r.name = item.FirstName;
                    r.review = item.Comments;
                    r.rating = item.rating;
                    reviewlist.Add(r);
                }
                Dictionary<string, object> d1 = new Dictionary<string, object>();
                d1.Add("results", reviewlist);
                d1.Add("total_review_count", count);
                d1.Add("offset", (offset == 0) ? 6 : offset);
                d1.Add("page", page);
                d1.Add("no_of_pages", ((count - 1) / offset) + 1);
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

    }
}
