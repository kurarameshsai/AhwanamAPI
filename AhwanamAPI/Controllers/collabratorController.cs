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
    public class collabratorController : ApiController
    {
        WhishListService wishlistservice = new WhishListService();
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

        public class UseNotes
        {
            public long notes_id { get; set; }
            public long wishlist_id { get; set; }
            public long vendor_id { get; set; }
            public string note { get; set; }
            public long contributorId { get; set; }
            public string author_name { get; set; }
            public DateTime added_datetime { get; set; }
            public DateTime edited_datetime { get; set; }
        }

        [HttpPost]
         [Route("api/collabrator/addnote")]
        public IHttpActionResult addcollabratornotes(AddNote note)
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
                    collabratornotes notes = new collabratornotes();
                    notes.wishlist_id = note.wishlist_id;
                    notes.vendor_id = note.vendor_id;
                    notes.Userid = userdetails.UserLoginId;
                    notes.collabratorNote = note.note;
                    notes.Name = userdetails.name;
                    notes.AddedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    notes.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    var notedata = wishlistservice.addcollabratornote(notes);
                    UseNotes usernotes = new UseNotes();
                    usernotes.notes_id = notedata.collabratornotesId;
                    usernotes.wishlist_id = notedata.wishlist_id;
                    usernotes.vendor_id = notedata.vendor_id;
                    usernotes.note = notedata.collabratorNote;
                    usernotes.contributorId = notedata.Userid;
                    usernotes.author_name = notedata.Name;
                    usernotes.added_datetime = notedata.AddedDate;
                    if (notedata != null)
                    {
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
        [Route("api/collabrator/updatenote")]
        public IHttpActionResult Updatecollabratornote(EditNote enote)
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
                if (userdetails.Token == token)
                {
                    collabratornotes cn = new collabratornotes();
                    cn.collabratorNote = enote.note;
                    cn.Userid = userdetails.UserLoginId;
                    cn.Name = userdetails.name;
                    cn.UpdatedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                    var notedata = wishlistservice.UpdatecollabratorNotes(cn, enote.note_id);
                    if (notedata != null)
                    {
                        UseNotes usernotes = new UseNotes();
                        usernotes.notes_id = notedata.collabratornotesId;
                        usernotes.wishlist_id = notedata.wishlist_id;
                        usernotes.vendor_id = notedata.vendor_id;
                        usernotes.note = notedata.collabratorNote;
                        usernotes.contributorId = notedata.Userid;
                        usernotes.author_name = notedata.Name;
                        //usernotes.added_datetime = notedata.AddedDate;
                        usernotes.edited_datetime = notedata.UpdatedDate;
                        dict.Add("status", true);
                        dict.Add("message", "Success");
                        dict.Add("data", usernotes);
                        return Json(dict);
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

        [HttpDelete]
        [Route("api/collabrator/removenote")]
        public IHttpActionResult removecollabratornote([FromUri] long note_id)
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
                    int count = wishlistservice.RemovecollabratorNotes(note_id);
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
    }
}
