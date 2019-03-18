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

        public class wishlist
        {
            public long UserId { get; set; }
            public string Name { get; set; }
            public string Event { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }
            public DateTime UpdatedDate { get; set; }
        }
        public class wishlistItems
        {
            public long wishlistId { get; set; }
            public long vendorId { get; set; }
            public long vendorsubId { get; set; }
            public string BusinessName { get; set; }
            public string servicetype { get; set; }
            public long UserId { get; set; }
            public DateTime WhishListedDate { get; set; }
            public string IPAddress { get; set; }
            public string Status { get; set; }
            public long NotesId { get; set; }
            public string Ceremony { get; set; }
            public string Budjet { get; set; }
            public DateTime? EventStartDate { get; set; }
            public DateTime? EventEndDate { get; set; }
            public long collabratorid { get; set; }
        }
    }
}
