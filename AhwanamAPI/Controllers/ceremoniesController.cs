using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class ceremoniesController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        WhishListService wishlistservice = new WhishListService();
        public class price
        {
            public string minimum_price { get; set; }
            public string format_price { get; set; }
        }

        public class vendors
        {
            public long vendor_id { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public string reviews_count { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public price price { get; set; }
            public string pic_url { get; set; }
            public bool is_in_wishlist { get; set; }
           
        }

        public class metatag
        {
            public string title { get; set; }
            public string description { get; set; }
            public string keywords { get; set; }
        }
        public class ceremony
        {
            public metatag metatag { get; set; }
            public long ceremony_id { get; set; }
            public string ceremony_name { get; set; }
            public string page_name { get; set; }
            public string description { get; set; }
            public string cermony_image { get; set; }
            public string city { get; set; }
            public List<categories> categories { get; set; }
        }
        public class categories
        {
            public string name { get; set; }
            public string sub_title { get; set; }
            public string thumb_image { get; set; }
            public string page_name { get; set; }
            public int category_id { get; set; }
            public List<vendors> vendors { get; set; }
        }
        public class ceremonys
        {
            public long ceremony_id { get; set; }
            public string ceremony_name { get; set; }
            public string thumb_image { get; set; }
            public string page_name { get; set; }
            public string description { get; set; }
        }

        public class filters
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public List<values> values { get; set; }
        }

        public class values
        {
            public string name { get; set; }
            public int id { get; set; }
        }

        public class allvendors
        {
            public long vendor_masterId { get; set; }
            public long vendor_serviceId { get; set; }
            public string category_name { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public string name { get; set; }
            public string page_name { get; set; }
            public string pic_url { get; set; }
            public decimal rating { get; set; }
            public string reviews_count { get; set; }
            public price price { get; set; }

        }
        public class value
        {
            public string name { get; set; }
            public long id { get; set; }
        }


        public string getcity(int id)
        {
            var city = "";
            ResultsPageService resultsPageService = new ResultsPageService();
            //var result = resultsPageService.Getvendormasterdata();
            //var citylist = result.Select(m => m.City).Distinct().ToList();
            var citylist = new List<string>() { "Hyderabad", "Secunderabad", "Vijayawada", "Vizag" };
            List<value> val1 = new List<value>();
            for (int i = 0; i < citylist.Count; i++)
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
        [Route("api/ceremonies/similarceremonies")]
        public IHttpActionResult similarceremonies(long ceremony_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            CeremonyServices ceremonyServices = new CeremonyServices();
            var list = ceremonyServices.Getall().Where(c=>c.Id != ceremony_id).Take(4).ToList();
            List<ceremonys> details = new List<ceremonys>();
            foreach(var item in list)
            {
                ceremonys ceremonydetail = new ceremonys();
                ceremonydetail.ceremony_id = item.Id;
                ceremonydetail.ceremony_name = item.Title;
                ceremonydetail.thumb_image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "ceremonies_images/" + item.Image;
                ceremonydetail.description = item.Description;
                ceremonydetail.page_name = item.page_name;
                details.Add(ceremonydetail);
            }

            Dictionary<string, object> d1 = new Dictionary<string, object>();
            d1.Add("results", details);
            dict.Add("status", true);
            dict.Add("data", d1);
            return Json(dict);
        }

        [HttpGet]
        [Route("api/ceremonies/details")]
        public IHttpActionResult details(long ceremony_id, int? city = -1)
        {
            ResultsPageService resultsPageService = new ResultsPageService();
            FilterServices filterServices = new FilterServices();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string cityvalue = (city != -1 && city != null) ? getcity((int)city) : null;
            string token = null;
            CeremonyServices ceremonyServices = new CeremonyServices();
            ceremony d = new ceremony();
            var details = ceremonyServices.Getceremonydetails(ceremony_id).ToList();
            var detail = details.FirstOrDefault();
            d.ceremony_id = detail.Id;
            d.ceremony_name = detail.Title;
            //d.thumb_image = detail.Image;
            d.page_name = detail.page_name1;
            d.description = detail.Description;
            d.cermony_image = System.Configuration.ConfigurationManager.AppSettings["imagename"] + "ceremonies_images/" + detail.ceremonyImage;
            metatag tag = new metatag();
            tag.title = detail.MetatagTitle.Trim();
            tag.description = detail.MetatagDesicription.Trim();
            tag.keywords = detail.MetatagKeywords.Trim();
            d.metatag = tag;
            d.city = cityvalue;
            List<categories> categories = new List<categories>();
            for (int i = 0; i < details.Count; i++)
            {
                categories categories1 = new categories();
                categories1.name = details[i].Category;
                categories1.sub_title = details[i].Description;
                categories1.page_name = details[i].page_name2;
                categories1.category_id = Convert.ToInt32(details[i].categoryId);
                List<vendors> param = new List<vendors>();
                var re = Request;
                var customheader = re.Headers;
                UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
                if (customheader.Contains("Authorization"))
                {
                    token = customheader.GetValues("Authorization").First();
                }
                var data = wishlistservice.getvendorsalldetails(token, categories1.category_id).OrderByDescending(v => v.priority).Take(7).ToList();
                if (cityvalue != null || cityvalue == "empty")
                  data = data.Where(m => m.City == cityvalue).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        vendors p = new vendors();
                        p.vendor_id = item.VendorID;
                        p.category_id = item.Category_TypeId;
                        p.category_name = item.name;
                        p.name = item.BusinessName;
                        p.charge_type = item.Type_of_price;
                        p.city = item.City;
                        p.pic_url = System.Configuration.ConfigurationManager.AppSettings["imagename"] + item.VendorID + "/main.jpg";
                        //prices Section
                        price price = new price();
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
                            //price.maxprice = item.MaxPrice.ToString();
                        }
                        p.price = price;
                        p.rating = item.Rating;
                        var data1 = resultsPageService.Getreviews(item.VendorID);
                        p.reviews_count = data1.Count().ToString();
                        p.is_in_wishlist = Convert.ToBoolean(item.is_in_ishlist);
                        param.Add(p);
                    }
                }
                var records = param;
                categories1.vendors = records;
                categories.Add(categories1);
            }
            d.categories = categories;
            if (d.categories!=null)
            { 
           
            dict.Add("status", true);
            dict.Add("message", "Success");
            dict.Add("data", d);
            
            }
            else
            {
                d.categories = categories;
                dict.Add("status", false);
                dict.Add("message", "Success");
                dict.Add("data", d);

            }
            return Json(dict);
        }

      
    }
  }
