using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using MaaAahwanam.Service;
//using MaaAahwanam.Models;
//using MaaAahwanam.Utility;
//using System.IO;

namespace AhwanamAPI.Controllers
{
    public class wishlistController : ApiController
    {
        WhishListService wishlistservice = new WhishListService();
        UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();


        public class wishlist
        {
            public long wishlist_id { get; set; }
            public long user_id { get; set; }
            public string name { get; set; }
            public List<wishlistitems> wishlistitems { get; set; }
            public List<sharedwishlist> shared_wishlists { get; set; }
            public List<collaborators> collaborators { get; set; }
           
        }

        public class sharedwishlist
        {
            public string name { get; set; }
            public long wishlist_id { get; set; }
        }
        public class collaborators
        {
            public long collaborator_id { get; set; }
            public string collaborator_name { get; set; }
            public long user_id { get; set; }
            public string collaborator_email { get; set; }
        }
        public class listItems
        {
            public long vendor_id { get; set; }
            public long wishlist_id { get; set; }
            //public long NotesId { get; set; }
            //public long collabratorid { get; set; }
        }
        public class AddNote
        {
            public long wishlist_id { get; set; }
            public long vendor_id { get; set; }
            public string note { get; set; }
        }

        public class EditNote
        {
            public long note_id { get; set; }
            public string note { get; set; }
        }

        public class detailids
        {
            public long vendor_id { get; set; }
            public long wishlist_id { get; set; }
            //public long user_id { get; set; }
        }

        public class UserCollaborator
        {
            //public long user_id { get; set; }
            public long wishlist_id { get; set; }
            public string email { get; set; }
            public string phoneNo { get; set; }
            public string name { get; set; }
            //public DateTime UpdatedDate { get; set; }
        }

        public class DetailsCollaborator
        {
            public long collaborator_id { get; set; }
            public long user_id { get; set; }
            public long wishlist_id { get; set; }
            public string email { get; set; }
            public string phoneNo { get; set; }
            public string code { get; set; }
            public string collaborator_name { get; set; }
            //public DateTime UpdatedDate { get; set; }
        }
        //public class WishList
        //{
        //    public long UserId { get; set; }
        //    public string Name { get; set; }
        //    public string Event { get; set; }
        //    public DateTime? StartDate { get; set; }
        //    public DateTime? EndDate { get; set; }
        //    public string Description { get; set; }
        //    public DateTime UpdatedDate { get; set; }
        //}

        public class category
        {
            public long category_id { get; set; }
            public string category_name { get; set; }
            public vendor vendor { get; set; }
            
        }

        public class vendor
        {
            public string name { get; set; }
            public long vendor_id { get; set; }
            public int category_id { get; set; }
            public string category_name { get; set; }
            public long userwishlist_id { get; set; }
            public long reviews_count { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public price price { get; set; }
            public string pic_url { get; set; }
            public long? collaborator_id { get; set; }
            public List<notes> notes { get; set; }
        }
        public class notes
        {
            public long notes_id { get; set; }
            public string notes_text { get; set; }
            public DateTime updatedatetime { get; set; }
        }

        public class userwishlist
        {
            public long wishlist_id { get; set; }
            public long user_id { get; set; }
            public string name { get; set; }
            public List<wishlistitems> wishlistitems { get; set; }
        }

        public class wishlistitems
        {
            public int category_id { get; set; }
            public string category_name { get; set; }
            public string page_name { get; set; }
            public List<vendors> vendors { get; set; } 
        }
        public class price
        {
            //public string Rentalprice { get; set; }
            public decimal minimum_price { get; set; }
            //public string maxprice { get; set; }
            //public string vegprice { get; set; }
            //public string nonvegprice { get; set; }
        }
        public class vendors
        {
            public long vendor_id { get; set; }
            //public long vendor_serviceId { get; set; }
            public int category_id { get; set; }
            public string name { get; set; }
            public string category_name { get; set; }
            public long reviews_count { get; set; }
            public decimal rating { get; set; }
            public string charge_type { get; set; }
            public string city { get; set; }
            public price price { get; set; }
            public string pic_url { get; set; }
            public  long contributor_id { get; set; }
        }

        public class UseNotes
        {
            public long notes_id { get; set; }
            public long wishlist_id { get; set; }
            public  long vendor_id { get; set; }
            public string note { get; set; }
            public long contributorId { get; set; }
            public string author_name { get; set; }
            public DateTime added_datetime { get; set; }
            public DateTime edited_datetime { get; set; }
        }

        [HttpGet]
        [Route("api/wishlist/getmywishlist")]
        public IHttpActionResult Addusertowishlist()
        {
            WishlistDetails wishlists = new WishlistDetails();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var details = userlogindetailsservice.Getmyprofile(token);
                if(details!=null)
                {
                    WhishListService wishlistservice1 = new WhishListService();
                    wishlists.UserId = details.UserLoginId;
                    wishlists.Name = details.FirstName + ' ' + details.LastName;
                    wishlists.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                  var userdata = wishlistservice1.Getuserfromwishlistbyuserid(wishlists.UserId);
                    if(userdata == null)
                    {
                    var data = wishlistservice1.AddUserwishlist(wishlists);
                    if (data != null)
                    {
                        wishlist list = new wishlist();
                        list.wishlist_id = data.WishlistdetailId;
                        list.user_id = data.UserId;
                        list.name = data.Name;
                       FilterServices filterServices = new FilterServices();
                       var categories = filterServices.AllCategories();
                            List<wishlistitems> categorys = new List<wishlistitems>();
                            for (int i = 0; i < categories.Count(); i++)
                            {
                                wishlistitems category = new wishlistitems();
                                category.category_id = categories[i].servicType_id;
                                category.category_name = categories[i].name;
                                category.page_name = categories[i].display_name;
                                var vendordata = wishlistservice1.getwishlistvendors(list.wishlist_id, category.category_id);
                                if(vendordata!=null)
                                {
                                    List<vendors> vendorslst = new List<vendors>();
                                    for (int j = 0; j < vendordata.Count(); j++)
                                    {
                                        vendors v = new vendors();
                                       v.vendor_id = vendordata[j].VendormasterId;
                                        v.category_id = vendordata[j].Category_TypeId;
                                        v.category_name = vendordata[j].name;
                                        v.name = vendordata[j].BusinessName;
                                        v.city = vendordata[j].City;
                                        v.rating = vendordata[j].Rating;
                                        v.reviews_count = vendordata[j].ReviewsCount;
                                        v.charge_type = vendordata[j].Type_of_price;
                                        price p = new price();
                                        if (vendordata[j].name == "Venues" || vendordata[j].name == "Caterers")
                                        {
                                            
                                            p.minimum_price = vendordata[j].VegPrice;
                                        }
                                        else
                                        {
                                            p.minimum_price = vendordata[j].MinPrice;
                                        }
                                        v.price = p;
                                        v.pic_url = "https://api.ahwanam.com/images/" + v.vendor_id + "/main.jpg";
                                        v.contributor_id = vendordata[j].UserId;
                                        vendorslst.Add(v);
                                    }
                                    category.vendors = vendorslst;
                                }
                                
                                categorys.Add(category);
                            }
                            list.wishlistitems = categorys;
                            List<sharedwishlist> swlist = new List<sharedwishlist>();
                            var sharedwishlistdata = wishlistservice.getsharedwishlist(details.AlternativeEmailID);
                            if(sharedwishlistdata!=null)
                            {
                                foreach (var item in sharedwishlistdata)
                                {
                                    sharedwishlist s = new sharedwishlist();
                                    s.name = item.Name;
                                    s.wishlist_id = item.WishlistdetailId;
                                    swlist.Add(s);
                                }
                                list.shared_wishlists = swlist;
                            }
                            else { list.shared_wishlists = swlist; }
                            List<collaborators> clist = new List<collaborators>();
                            var collaboratordata = wishlistservice.Getcollabrators(data.UserId);
                            if(collaboratordata!=null)
                            {
                               
                                foreach(var item in collaboratordata)
                                {
                                    collaborators c = new collaborators();
                                    c.collaborator_id = item.Id;
                                    c.collaborator_name = item.collabratorname;
                                    c.user_id = item.UserId;
                                    c.collaborator_email = item.Email;
                                    clist.Add(c);
                                }
                                list.collaborators = clist;
                            }
                            else { list.collaborators = clist; }
                                dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", list);
                    }
                     else
                     {
                        dict.Add("status", false);
                        dict.Add("message", "failed");

                     }
                  }
                    else 
                    {
                        wishlist list = new wishlist();
                        list.wishlist_id = userdata.WishlistdetailId;
                        list.user_id = userdata.UserId;
                        list.name = userdata.Name;
                        FilterServices filterServices = new FilterServices();
                        var categories = filterServices.AllCategories();
                        List<wishlistitems> categorys = new List<wishlistitems>();
                        for (int i = 0; i < categories.Count(); i++)
                        {
                            wishlistitems category = new wishlistitems();
                            category.category_id = categories[i].servicType_id;
                            category.category_name = categories[i].name;
                            category.page_name = categories[i].display_name;
                            var vendordata = wishlistservice1.getwishlistvendors(list.wishlist_id, category.category_id);
                            if (vendordata != null)
                            {
                                List<vendors> vendorslst = new List<vendors>();
                                for (int j = 0; j < vendordata.Count(); j++)
                                {
                                    vendors v = new vendors();
                                    v.vendor_id = vendordata[j].VendormasterId;
                                    v.category_id = vendordata[j].Category_TypeId;
                                    v.category_name = vendordata[j].name;
                                    v.name = vendordata[j].BusinessName;
                                    v.city = vendordata[j].City;
                                    v.rating = vendordata[j].Rating;
                                    v.reviews_count = vendordata[j].ReviewsCount;
                                    v.charge_type = vendordata[j].Type_of_price;
                                    price p = new price();
                                    if (vendordata[j].name == "Venues" || vendordata[j].name == "Caterers")
                                    {
                                        p.minimum_price = vendordata[j].VegPrice;
                                    }
                                    else
                                    {
                                        p.minimum_price = vendordata[j].MinPrice;
                                    }
                                    v.price = p;
                                    v.pic_url = vendordata[j].pic_url;
                                    v.contributor_id = vendordata[j].UserId;
                                    vendorslst.Add(v);
                                }
                                category.vendors = vendorslst;
                            }

                            categorys.Add(category);
                        }
                        list.wishlistitems = categorys;
                        List<sharedwishlist> swlist = new List<sharedwishlist>();
                        var sharedwishlistdata = wishlistservice.getsharedwishlist(details.AlternativeEmailID);
                        if (sharedwishlistdata != null)
                        {
                            foreach (var item in sharedwishlistdata)
                            {
                                sharedwishlist s = new sharedwishlist();
                                s.name = item.Name.Trim(' ');
                                s.wishlist_id = item.WishlistdetailId;
                                swlist.Add(s);
                            }
                            list.shared_wishlists = swlist;
                        }
                        else { list.shared_wishlists = swlist; }
                        List<collaborators> clist = new List<collaborators>();
                        var collaboratordata = wishlistservice.Getcollabrators(userdata.UserId);
                        if (collaboratordata != null)
                        {

                            foreach (var item in collaboratordata)
                            {
                                collaborators c = new collaborators();
                                c.collaborator_id = item.Id;
                                c.collaborator_name = item.collabratorname;
                                c.user_id = item.UserId;
                                c.collaborator_email = item.Email;
                                clist.Add(c);
                            }
                            list.collaborators = clist;
                        }
                        else { list.collaborators = clist; }
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", list);
                    }
                }
            }
            return Json(dict);
        }
     

        [HttpGet]
        [Route("api/wishlist/getsharedwishlist")]
        public IHttpActionResult getsharedwishlist(long wishlist_id)
        {
            WishlistDetails wishlists = new WishlistDetails();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var details = userlogindetailsservice.Getmyprofile(token);
                if (details.Token == token)
                {
                    WhishListService wishlistservice1 = new WhishListService();
                    var wishdetails = wishlistservice1.getwishlistdetails(wishlist_id);
                    wishlist list = new wishlist();
                    if (wishdetails != null)
                    {
                        list.wishlist_id = wishdetails.WishlistdetailId;
                        list.user_id = wishdetails.UserId;
                        list.name = wishdetails.Name;
                        FilterServices filterServices = new FilterServices();
                        var categories = filterServices.AllCategories();
                        List<wishlistitems> categorys = new List<wishlistitems>();
                        for (int i = 0; i < categories.Count(); i++)
                        {
                            wishlistitems category = new wishlistitems();
                            category.category_id = categories[i].servicType_id;
                            category.category_name = categories[i].name;
                            category.page_name = categories[i].display_name;
                            var vendordata = wishlistservice1.getwishlistvendors(list.wishlist_id, category.category_id);
                            if (vendordata != null)
                            {
                                List<vendors> vendorslst = new List<vendors>();
                                for (int j = 0; j < vendordata.Count(); j++)
                                {
                                    vendors v = new vendors();
                                    v.vendor_id = vendordata[j].VendormasterId;
                                    v.category_id = vendordata[j].Category_TypeId;
                                    v.category_name = vendordata[j].name;
                                    v.name = vendordata[j].BusinessName;
                                    v.city = vendordata[j].City;
                                    v.rating = vendordata[j].Rating;
                                    v.reviews_count = vendordata[j].ReviewsCount;
                                    v.charge_type = vendordata[j].Type_of_price;
                                    price p = new price();
                                    if (vendordata[j].name == "Venues" || vendordata[j].name == "Caterers")
                                    {

                                        p.minimum_price = vendordata[j].VegPrice;
                                    }
                                    else
                                    {
                                        p.minimum_price = vendordata[j].MinPrice;
                                    }
                                    v.price = p;
                                    v.pic_url = "https://api.ahwanam.com/images/" + v.vendor_id + "/main.jpg";
                                    v.contributor_id = vendordata[j].UserId;
                                    vendorslst.Add(v);
                                }
                                category.vendors = vendorslst;
                            }

                            categorys.Add(category);
                        }
                        list.wishlistitems = categorys;
                        List<collaborators> clist = new List<collaborators>();
                        var collaboratordata = wishlistservice.Getcollabrators(list.user_id);
                        if (collaboratordata != null)
                        {

                            foreach (var item in collaboratordata)
                            {
                                collaborators c = new collaborators();
                                c.collaborator_id = item.Id;
                                c.collaborator_name = item.collabratorname;
                                c.user_id = item.UserId;
                                c.collaborator_email = item.Email;
                                clist.Add(c);
                            }
                            list.collaborators = clist;
                        }
                        else { list.collaborators = clist; }
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", list);
                    }
                    else
                    {
                        dict.Add("status", false);
                        dict.Add("message", "failed");
                        dict.Add("data", list);

                    }
                
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "failed");

                }
                
            }
            return Json(dict);
       }


        [HttpPost]
        [Route("api/wishlist/additem")]
        public IHttpActionResult AddItemToWishlist([FromBody]listItems list)
        {
            Userwishlistdetails userwishlistdetails = new Userwishlistdetails();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            WhishListService wishlistservices = new WhishListService();
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if(userdetails.Token == token)
                { 
                long item = wishlistservices.Getvendordetailsbyvendorid(list.vendor_id,list.wishlist_id);
                if (item == 0)
                {
                    var details = wishlistservices.Getwishlistdetail(list.wishlist_id);
                    ResultsPageService resultsPageService = new ResultsPageService();
                    category categorys = new category();
                    vendor v = new vendor();
                        var vdata = wishlistservices.Getdetailsofvendorbyid(list.vendor_id);
                        userwishlistdetails.UserId = details.UserId;
                    userwishlistdetails.wishlistId = list.wishlist_id;
                    userwishlistdetails.vendorId = list.vendor_id;
                        userwishlistdetails.categoryid = vdata.Category_TypeId;
                    userwishlistdetails.IPAddress = HttpContext.Current.Request.UserHostAddress;
                    userwishlistdetails.WhishListedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    var data = wishlistservices.Adduserwishlistitem(userwishlistdetails);
                    if (data != null)
                    {
                       
                        categorys.category_id = vdata.Category_TypeId;
                        categorys.category_name = vdata.name;
                        v.vendor_id = vdata.VendorId;
                        v.category_id = vdata.Category_TypeId;
                        v.category_name = vdata.name;
                        v.userwishlist_id = vdata.wishlistId;
                        v.name = vdata.BusinessName;
                        v.city = vdata.City;
                        v.charge_type = vdata.Type_of_price;
                        v.collaborator_id = vdata.UserId;
                        v.rating = vdata.Rating;
                        v.reviews_count = vdata.ReviewsCount;
                            price p = new price();
                        if (vdata.name == "Venues" || vdata.name == "Caterers")
                        {
                                p.minimum_price = vdata.VegPrice;
                        }
                        else
                        {
                                p.minimum_price = vdata.MinPrice;
                        }
                           v.price = p;
                         v.pic_url = "https://api.ahwanam.com/images/" + v.vendor_id + "/main.jpg"; ;
                        v.notes = null;
                        categorys.vendor = v;
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", categorys);
                    }
                       else
                        {
                            dict.Add("status", false);
                            dict.Add("message", "failed");
                        }
                    }
                    else
                    {
                        dict.Add("status", false);
                        dict.Add("message", "This service already existed in wishlist");
                    }
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "failed");
                }
            }      
            return Json(dict);
        }

        [HttpPost]
        [Route("api/wishlist/removeitem")]
        public IHttpActionResult RemoveWishList(long vendor_id,long wishlist_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            WhishListService wishlistservices = new WhishListService();

            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails.Token == token)
                {
                    if (userdetails != null)
                    {
                        int count = wishlistservice.Removeitem(vendor_id, wishlist_id, userdetails.UserLoginId);
                        if (count != 0)
                        {
                            dict.Add("status", true);
                            dict.Add("message", "Success");
                        }
                        else
                        {
                            dict.Add("status", true);
                            dict.Add("message", "This service is already removed");
                        }
                    }
                    else
                    {
                        dict.Add("status", false);
                        dict.Add("message", "Failed");
                    }

                  }
                }
            return Json(dict);
        }
        //var details = wishlistservices.Getwishlistdetail(ids.wishlist_id);
        // if(details!=null)
        // {
        //    long user_id = details.UserId;
        // int count = wishlistservice.Removeitem(ids.vendor_id,ids.wishlist_id,user_id);
        // if(count!=0)
        // {
        //     dict.Add("status", true);
        //     dict.Add("message", "Success");
        // }
        //}
        // else
        // {
        //     dict.Add("status", false);
        //     dict.Add("message", "Failed");
        // }

        [HttpGet]
        [Route("api/getallnotes")]
        public IHttpActionResult Getnote(long wishlist_id, long vendor_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails.Token == token)
                {
                    var data = wishlistservice.Getnote(wishlist_id, vendor_id);
                    if(data!=null)
                    {
                        List<UseNotes> usernotes = new List<UseNotes>();
                        foreach (var item in data)
                        {
                            UseNotes usernote = new UseNotes();
                            usernote.notes_id = item.NotesId;
                            usernote.wishlist_id = item.wishlistId;
                            usernote.vendor_id = item.VendorId;
                            usernote.note = item.Notes;
                            usernote.contributorId = userdetails.UserLoginId;
                            usernote.author_name = userdetails.name;
                            usernote.added_datetime = item.AddedDate;
                            usernote.edited_datetime = item.UpdatedDate;
                            usernotes.Add(usernote);
                        }

                        Dictionary<string, object> dict1 = new Dictionary<string, object>();
                        dict1.Add("results", usernotes);
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", dict1);
                    }
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "Failed");
                }

            }
            return Json(dict);

        }

        [HttpPost]
        [Route("api/wishlist/addnote")]
        public IHttpActionResult AddNotes([FromBody]AddNote note)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails.Token == token)
                {
                    Note notes = new Note();
                    notes.wishlistId = note.wishlist_id;
                    notes.VendorId = note.vendor_id;
                    notes.UserId = userdetails.UserLoginId;
                    notes.Notes = note.note;
                    notes.Name = userdetails.name;
                    notes.AddedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    notes.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    var notedata = wishlistservice.AddNotes(notes);
                    UseNotes usernotes = new UseNotes();
                    usernotes.notes_id = notedata.NotesId;
                    usernotes.wishlist_id = notedata.wishlistId;
                    usernotes.vendor_id = notedata.VendorId;
                    usernotes.note = notedata.Notes;
                    usernotes.contributorId = userdetails.UserLoginId;
                    usernotes.author_name = userdetails.name;
                    usernotes.added_datetime = notedata.AddedDate;
                    if (notedata != null)
                    {
                        // Dictionary<string, object> dict1 = new Dictionary<string, object>();
                        //dict1.Add("results", usernotes);
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", usernotes);
                        
                    }
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "Failed");
                }
            }

            return Json(dict);
        }

        [HttpPost]
        [Route("api/wishlist/updatenote")]
        public IHttpActionResult UpdateNote([FromBody] EditNote enote)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            Note notes = new Note();
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails!=null || userdetails.Token == token)
                {
                    Note note = new Note();
                    note.Notes = enote.note;
                    note.UserId = userdetails.UserLoginId;
                    note.Name = userdetails.name;
                    note.UpdatedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    var notedata = wishlistservice.UpdateNotes(note, enote.note_id);
                    if (notedata != null)
                    {
                        UseNotes usernotes = new UseNotes();
                        usernotes.notes_id = notedata.NotesId;
                        usernotes.wishlist_id = notedata.wishlistId;
                        usernotes.vendor_id = notedata.VendorId;
                        usernotes.note = notedata.Notes;
                        usernotes.contributorId = userdetails.UserLoginId;
                        usernotes.author_name = userdetails.name;
                        //usernotes.added_datetime = notedata.AddedDate;
                        usernotes.edited_datetime = notedata.UpdatedDate;
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", usernotes);
                        return Json(dict);
                    }
                }
                else { 
                dict.Add("status", false);
                dict.Add("message", "Failed");
                }
            }
                 
          
            return Json(dict);
        }
        [HttpPost]
        [Route("api/wishlist/removenote")]
        public IHttpActionResult RemoveNotes([FromUri] long note_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails.Token == token)
                {
                    int count = wishlistservice.RemoveNotes(note_id);
                    if (count != 0)
                    {
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                    }
                    else
                    {
                        dict.Add("status", false);
                        dict.Add("message", "note already removed");
                    }
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "Failed");
                }
            }
                   
            return Json(dict);

        }


        public void TriggerEmail(string txtto, string txtmsg, string subject, HttpPostedFileBase attachment)
        {
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Wordpress_Email(txtto, txtmsg, subject, attachment);
        }

        [HttpPost]
        [Route("api/addcollaborator")]
        public IHttpActionResult Addcollabrator(UserCollaborator collaborator)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            Collabrator collbratordata = new  Collabrator();
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails.Token == token)
                {
                    collbratordata.UserId = userdetails.UserLoginId;
                    collbratordata.wishlistid = collaborator.wishlist_id;
                    collbratordata.collabratorname = collaborator.name;
                    collbratordata.PhoneNo = collaborator.phoneNo;
                    collbratordata.Email = collaborator.email;
                    collbratordata.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    string name = collbratordata.UserId.ToString() + ',' + collbratordata.wishlistid.ToString() + ',' + collaborator.email;
                    encptdecpt encrypt = new encptdecpt();
                    string encrypted = encrypt.Encrypt(name);
                    collbratordata.wishlistlink = encrypted;
                    long details = wishlistservice.GetcollabratorDetailsByEmail(collaborator.email, userdetails.UserLoginId);
                    if(details == 0)
                    { var data = wishlistservice.AddCollabrator(collbratordata);
                        DetailsCollaborator cdetails = new DetailsCollaborator();
                        if (data!=null)
                        {

                            //string url = "http://sandbox.ahwanam.com/verify?activation_code=" + userlogin.ActivationCode + "&email=" + userlogin.UserName;
                            string url = "http://sandbox.ahwanam.com/addcollabrator?wishlist_id=" + data.wishlistid + "&email=" + data.Email;
                            FileInfo File = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("/mailtemplate/welcome.html"));
                            string readFile = File.OpenText().ReadToEnd();
                            readFile = readFile.Replace("[ActivationLink]", url);
                            //readFile = readFile.Replace("[name]", data.Email);
                            //readFile = readFile.Replace("[phoneno]", data.PhoneNo);
                            TriggerEmail(data.Email, readFile, "Account Invitation", null);
                            cdetails.email = data.Email;
                            cdetails.phoneNo = data.PhoneNo;
                            cdetails.collaborator_name = data.collabratorname;
                            cdetails.user_id = data.UserId;
                            cdetails.collaborator_id = data.Id;
                            cdetails.wishlist_id = data.wishlistid;
                            cdetails.code = data.wishlistlink;
                            //string url = "http://sandbox.ahwanam.com/verify?activation_code=" + userlogin.ActivationCode + "&email=" + userlogin.UserName;
                            //FileInfo File = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("/mailtemplate/welcome.html"));
                            //string readFile = File.OpenText().ReadToEnd();
                            //readFile = readFile.Replace("[ActivationLink]", url);
                            //readFile = readFile.Replace("[name]", data.Email);
                            //readFile = readFile.Replace("[phoneno]", data.PhoneNo);
                            //TriggerEmail(data.Email, readFile, "Account Activation", null);
                            dict.Add("status", true);
                            dict.Add("message", "Success");
                            dict.Add("data", cdetails);
                        }
                        else
                        {
                            dict.Add("status", false);
                            dict.Add("message", "failed");
                          
                        }
                    }
                    //string decrypted = encrypt.Decrypt(encrypted);
                    else
                    {
                        dict.Add("status", false);
                        dict.Add("message", "This collaborator already added");
                    }
                }
            }
            return Json(dict);
        }


        [HttpPost]
        [Route("api/removecollaborator")]
        public IHttpActionResult removecollaborator(long collaborator_id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            var re = Request;
            var customheader = re.Headers;
            UserLoginDetailsService userlogindetailsservice = new UserLoginDetailsService();
            if (customheader.Contains("Authorization"))
            {
                string token = customheader.GetValues("Authorization").First();
                var userdetails = userlogindetailsservice.Getmyprofile(token);
                if (userdetails.Token == token)
                {
                    int count = wishlistservice.RemoveCollabrator(collaborator_id);
                    if (count != 0)
                    {
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                    }
                    else if (count == 0)
                    {
                        dict.Add("status", false);
                        dict.Add("message", "collaborator already removed");
                    }
                }
                else
                {
                    dict.Add("status", false);
                    dict.Add("message", "Failed");
                }
            }

            return Json(dict);

        }
    }
}
