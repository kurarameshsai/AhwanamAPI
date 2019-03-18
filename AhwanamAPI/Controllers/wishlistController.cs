using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class wishlistController : ApiController
    {

        public class wishlistItems
        {
            public long UserId { get; set; }
            public string Name { get; set; }
            public string Event { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }
            public long vendorId { get; set; }
            public long vendorsubId { get; set; }
            public string BusinessName { get; set; }
            public string servicetype { get; set; }
            public string IPAddress { get; set; }
            public string Status { get; set; }
            public long NotesId { get; set; }
            public string Ceremony { get; set; }
            public string Budjet { get; set; }
            public DateTime? EventStartDate { get; set; }
            public DateTime? EventEndDate { get; set; }
            public long collabratorid { get; set; }
        }

        [HttpPost]
        [Route("api/wishlist/Addwishlist")]
        public IHttpActionResult Addwishlist(wishlistItems list)
        {
            WhishListService wishlistservice = new WhishListService();
            wishlist wishlists = new wishlist();
            Userwishlist userwishlistdetails = new Userwishlist();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            wishlists.StartDate= userwishlistdetails.EventStartDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            wishlists.EndDate = userwishlistdetails.EventEndDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            wishlists.UserId = list.UserId;
            wishlists.Name = list.Name;
            userwishlistdetails.vendorId = list.vendorId;
            userwishlistdetails.vendorsubId = list.vendorsubId;
            userwishlistdetails.BusinessName = list.BusinessName;
            userwishlistdetails.servicetype = list.servicetype;
            userwishlistdetails.Ceremony = list.Ceremony;
            userwishlistdetails.Budjet = list.Budjet;
            userwishlistdetails.collabratorid = list.collabratorid;
            var responce = "";
            responce = wishlistservice.AddAllwishlist(wishlists, userwishlistdetails);
            if (responce == "success")
            {
                dict.Add("status", true);
                dict.Add("message", "Success");
            }
            else
            {
                dict.Add("status", false);
                dict.Add("message", "failed");
            }
            return Json(dict);
        }
    }
}
