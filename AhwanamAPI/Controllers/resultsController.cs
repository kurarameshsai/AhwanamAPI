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

namespace AhwanamAPI.Controllers
{
    //[Authorize]
    public class resultsController : ApiController
    {
        ResultsPageService resultsPageService = new ResultsPageService();

        public class header
        {
            public string header_text { get; set; }
            public string sub_text { get; set; }
            public string image { get; set; }
        }

        public class prices
        {
            public string price { get; set; }
            public string offer_price { get; set; }
        }

        public class data
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
        }

        public class filters
        {
            public string name { get; set; }
            public string display_name { get; set; }
            public string values { get; set; }

        }

        public class param
        {
            public header header { get; set; }
            public data data { get; set; }
            public string total_count { get; set; }
            public string offset { get; set; }
            public string no_of_pages { get; set; }
            public string sortby { get; set; }
            public filters filters { get; set; }
            public string service_type { get; set; }
        }

        public List<param> FormatResult(string type, List<GetVendors_Result> data)
        {
            List<param> param = new List<param>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    param p = new param();
                    // Header Section
                    header headers = new header();
                    headers.header_text = item.BusinessName;
                    headers.sub_text = item.Description;
                    headers.image = "http://183.82.97.220/vendorimages/"+item.image;

                    //prices Section
                    prices price = new prices();
                    price.price = item.cost1.ToString();
                    price.offer_price = item.normaldays;

                    //Data Section
                    data vendor = new data();
                    vendor.name = item.BusinessName;
                    vendor.category_name = item.ServicType;
                    vendor.reviews_count = "58";
                    vendor.description = item.Description;
                    vendor.rating = "4.5";
                    vendor.charge_type = "Per Day";
                    vendor.latitude = "17.385044";
                    vendor.longitude = "78.486671";
                    vendor.city = item.City;
                    vendor.pic_url = "http://183.82.97.220/vendorimages/" + item.image;
                    vendor.price = price;

                    p.total_count = "99";
                    p.offset = "12";
                    p.no_of_pages = "18";
                    p.sortby = "price-low-to-high";
                    p.header = headers;
                    p.data = vendor;
                    p.service_type = item.subtype;
                    param.Add(p);
                }
            }
            return param;
        }

        [HttpGet]
        [Route("api/results/getall")]
        public IHttpActionResult getrecords(string type)
        {
            type = (type == null) ? "Venue" : type;
            var data = resultsPageService.GetAllVendors(type);
            var records = FormatResult(type, data);
            return Json(records);
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
    }
}
