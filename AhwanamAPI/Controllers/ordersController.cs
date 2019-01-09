using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace AhwanamAPI.Controllers
{
    public class ordersController : ApiController
    {

        VendorVenueService vendorVenueService = new VendorVenueService();
        ResultsPageService resultsPageService = new ResultsPageService();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        PartnerService partnerservice = new PartnerService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mnguserservice = new VendorDashBoardService();

        // VendorProductsService vendorProductsService = new VendorProductsService();
        int tprice;
        int price1;

        // GET: ManageUser
        public IHttpActionResult Index(string VendorId, string select, string packageid)
        {
            var ksc = new ksc();

            UserLogin userLogin = new UserLogin();
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            string uid = userLogin.UserLoginId.ToString();
            //string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);

            if (select != null)
            {
                var select1 = select.Split(',');


                var guests = select1[1];
                DateTime date = Convert.ToDateTime(select1[2]);
                string date1 = date.ToString("dd-MM-yyyy");

                // var pid = select1[4];
                if (packageid != "" || packageid != null)
                {
                    var pakageid = packageid.Split(',');
                    int price; StringBuilder pakg = new StringBuilder();
                    for (int i = 0; i < pakageid.Count(); i++)
                    {
                        if (pakageid[i] == "" || pakageid[i] == null)
                        {
                            price1 = 0;
                        }
                        else
                        {
                            var pkgs = newmanageuse.getpartpkgs(pakageid[i]).FirstOrDefault();
                            if (pkgs.PackagePrice == null)
                            {
                                price = Convert.ToInt32(pkgs.price1);
                            }
                            else { price = Convert.ToInt32(pkgs.PackagePrice); }
                            price1 = price1 + price;

                            pakg.Append("pkgs.PackageName + ',' +");
                        }

                    }

                   
                    var total = Convert.ToInt64(guests) * Convert.ToInt64(price1);
                    ksc.guests = guests;
                    ksc.total = total.ToString();
                    ksc.price1 = price1.ToString();
                    ksc.packageid = packageid;
                    ksc.pakg = pakg.ToString();

                }
            }
            
            return Json(ksc);
        }

                public class ksc {
            public string guests { get; set; }
            public string total { get; set; }
            public string price1 { get; set; }
            public string packageid { get; set; }
            public string pakg { get; set; }
                    }
    

        [HttpPost]
        public IHttpActionResult Index(ManageUser mnguser, string id, string command)
        {
            string msg = string.Empty;
            mnguser.registereddate = DateTime.Now;
            mnguser.updateddate = DateTime.Now;
            if (command == "Save")
            {
                mnguser = mnguserservice.AddUser(mnguser);
                msg = "Added New User";
            }
            else if (command == "Update")
            {
                mnguser = mnguserservice.UpdateUser(mnguser, int.Parse(id));
                msg = "Updated User";
            }
            return Json(msg);
        }
        public IHttpActionResult checkemail(string email, string id)
        {
            int query = mnguserservice.checkuseremail(email, id);
            if (query == 0)
                return Json("success");
            else
                return Json("email already exits");

        }
        [HttpPost]
        public IHttpActionResult GetUserDetails(string id)
        {
            var data = mnguserservice.getuserbyid(int.Parse(id));
            return Json(data);
        }

        public IHttpActionResult customerdetails(string id)
        {
           
                var data = mnguserservice.getuserbyid(int.Parse(id));
               var customer = data;
            return Json(customer);
        }
        public IHttpActionResult mnguserdetails()
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = "Sireesh.k@xsilica.com";
            userLogin.Password = "ksc";
            userLogin = resultsPageService.GetUserLogin(userLogin);
            string uid = userLogin.UserLoginId.ToString();
            string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
            var Userlist = mnguserservice.getuser(VendorId);
           
            return Json(Userlist);
        }
        //[HttpPost]
        //public JsonResult UpdateUserDetails(ManageUser mnguser, string id)
        //{
        //    mnguser.updateddate = DateTime.Now;
        //    mnguser = mnguserservice.UpdateUser(mnguser, int.Parse(id));
        //    return Json("Sucess", JsonRequestBehavior.AllowGet);
        //}


        public class booknowinfo
        {
            public string uid { get; set; }
            public string loc { get; set; }
            public string eventtype { get; set; }
            public string guest { get; set; }
            public string date { get; set; }
            public string pid { get; set; }
            public string vid { get; set; }
            public string timeslot { get; set; }
            public string booktype { get; set; }
            public string alltprice { get; set; }
            public string alldiscounttype { get; set; }
            public string discountprice { get; set; }
            public string fpkgprice { get; set; }
        }


        [HttpPost]
        public IHttpActionResult booknow(booknowinfo booknowinfo)
        {
            int userid = Convert.ToInt32(booknowinfo.uid);
            int price;
            string totalprice = "";
            string type = "";
            string etype1 = "";
            UserAuthController home = new UserAuthController();
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //Saving Record in order Table
            //  OrderService orderService = new OrderService();
            List<string> sdate = new List<string>();
            List<string> stimeslot = new List<string>();
            List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();
            var alltprice1 = booknowinfo.alltprice.Trim(',').Split(',');
            var alldiscounttype1 = booknowinfo.alldiscounttype.Trim(',').Split(',');
            var discountprice1 = booknowinfo.discountprice.Trim(',').Split(',');
            var fpkgprice1 = booknowinfo.fpkgprice.Trim(',').Split(',');

            var pkgs = booknowinfo.pid.Split(',');
            var date1 = booknowinfo.date.Trim(',').Split(',');
            var timeslot1 = booknowinfo.timeslot.Split(',');
            OrderDetail orderDetail = new OrderDetail();

            etype1 = booknowinfo.eventtype;
            for (int i = 0; i < pkgs.Count(); i++)
            {

                var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                if (data.PackagePrice == null)
                {
                    price = Convert.ToInt16(data.price1);
                }
                else { price = Convert.ToInt16(data.PackagePrice); }
                tprice = tprice + price;
                if (type == "Photography" || type == "Decorator" || type == "Other")
                {
                    totalprice = Convert.ToString(price);
                    booknowinfo.guest = "0";
                }
                else
                {
                    totalprice = Convert.ToString(tprice * Convert.ToInt16(booknowinfo.guest));
                }
            }
            Order order = new Order();
            order.TotalPrice = Convert.ToDecimal(totalprice);
            order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
            order.UpdatedBy = long.Parse(booknowinfo.vid);
            order.OrderedBy = long.Parse(booknowinfo.vid);
            order.UpdatedDate = Convert.ToDateTime(updateddate);
            if (booknowinfo.booktype == "Quote") { order.type = "Quote"; }
            else
            {
                order.type = "Order";
            }
            order.bookingtype = "Vendor";
            order.Status = "Pending";
            order = newmanageuse.SaveOrder(order);

            //Saving Order Details
            //OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
            for (int i = 0; i < pkgs.Count(); i++)
            {

                var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                if (data.PackagePrice == null)
                {
                    price = Convert.ToInt16(data.price1);
                }
                else { price = Convert.ToInt16(data.PackagePrice); }
                tprice = tprice + price;
                if (type == "Photography" || type == "Decorator" || type == "Other")
                {
                    totalprice = Convert.ToString(price);
                    booknowinfo.guest = "0";
                }
                else
                {
                    totalprice = Convert.ToString(price * Convert.ToInt16(booknowinfo.guest));
                }

                for (int j = 0; j < date1.Count(); j++)
                {
                    if (date1[j].Split('~')[1] == data.VendorSubId.ToString())
                    {
                        var allto = alltprice1[j].Split('₹')[1].ToString();

                        if (allto.Split('~')[0].ToString() == null || allto.Split('~')[0].ToString() == "") { data.price1 = "0"; } else { data.price1 = allto.Split('~')[0].ToString(); }
                        if (alldiscounttype1[j].Split('~')[0] == null || alldiscounttype1[j].Split('~')[0] == "") { data.price2 = "0"; } else { data.price2 = alldiscounttype1[j].Split('~')[0]; }
                        if (discountprice1[j].Split('~')[0] == null || discountprice1[j].Split('~')[0] == "") { data.price3 = "0"; } else { data.price3 = discountprice1[j].Split('~')[0]; }
                        if (fpkgprice1[j].Split('~')[0] == null || fpkgprice1[j].Split('~')[0] == "") { data.price4 = "0"; } else { data.price4 = fpkgprice1[j].Split('~')[0]; }
                        data.UpdatedDate = Convert.ToDateTime(date1[j].Split('~')[0]);
                        data.timeslot = timeslot1[j].Split('~')[0];
                    }
                }

                // data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
                //data.timeslot = timeslot1[i].Split('~')[0];

                orderDetail.OrderId = order.OrderId;
                orderDetail.OrderBy = long.Parse(booknowinfo.uid);
                orderDetail.PaymentId = '1';
                orderDetail.ServiceType = type;
                //      orderDetail.ServicePrice = Convert.ToDecimal(price);
                orderDetail.attribute = data.timeslot;
                orderDetail.TotalPrice = (Convert.ToDecimal(data.price1));
                orderDetail.ServicePrice = Convert.ToDecimal(data.price4);
                orderDetail.PerunitPrice = Convert.ToDecimal(price);
                orderDetail.Quantity = Convert.ToInt32(booknowinfo.guest);
                orderDetail.OrderId = order.OrderId;
                orderDetail.VendorId = long.Parse(booknowinfo.vid);

                if (booknowinfo.booktype == "Quote") { orderDetail.type = "Quote"; }
                else
                {
                    orderDetail.type = "Order";
                }
                orderDetail.DiscountType = data.price2;
                orderDetail.DiscountPrice = Convert.ToDecimal(data.price3);
                //orderDetail.Discount = Convert.ToDecimal(data.price2);
                orderDetail.OrderType = "online";
                orderDetail.Status = "Pending";
                orderDetail.bookingtype = "Vendor";
                orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                orderDetail.UpdatedBy = userid;
                orderDetail.subid = data.VendorSubId;
                orderDetail.BookedDate = Convert.ToDateTime(data.UpdatedDate);
                //ViewBag.orderdate = orderDetail.BookedDate;
                orderDetail.EventType = etype1;
                orderDetail.ServiceType = data.VendorType;
                orderDetail.DealId = long.Parse(pkgs[i]);
                orderDetail = newmanageuse.SaveOrderDetail(orderDetail);
            }
            var userlogdetails = mnguserservice.getuserbyid(userid);
            string txtto = userlogdetails.email;
            string name = userlogdetails.firstname;
            name = home.Capitalise(name);
            string OrderId = Convert.ToString(order.OrderId);
            StringBuilder cds = new StringBuilder();
            cds.Append("<table style='border:1px black solid;'><tbody>");
            cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
            cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.TotalPrice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
            cds.Append("</tbody></table>");
            string url = Request.RequestUri.Scheme + "://" + UriPartial.Authority ;
            FileInfo File = new FileInfo(HttpContext.Current.Server.MapPath("/mailtemplate/order.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[orderid]", OrderId);
            readFile = readFile.Replace("[table]", cds.ToString());
            string txtmessage = readFile;//readFile + body;
            string subj = "Thanks for your order";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj, null);
            //emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
            string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
            emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj, null);

            var vendordetails = newmanageuse.getvendor(Convert.ToInt32(booknowinfo.vid));

            string txtto1 = vendordetails.EmailId;
            string vname = vendordetails.BusinessName;
            vname = home.Capitalise(vname);
            StringBuilder cds2 = new StringBuilder();
            cds2.Append("<table style='border:1px black solid;'><tbody>");
            cds2.Append("<tr><td>Order Id</td><td>Order Date</td><td>Customer Name</td><td>Customer Phone Number</td><td>flatno</td><td>Locality</td></tr>");
            cds2.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderDate + "</td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
            cds2.Append("</tbody></table>");
            string url1 = Request.RequestUri.Scheme + "://" + UriPartial.Authority;
            FileInfo file1 = new FileInfo(HttpContext.Current.Server.MapPath("/mailtemplate/vorder.html"));
            string readfile1 = file1.OpenText().ReadToEnd();
            readfile1 = readfile1.Replace("[ActivationLink]", url1);
            readfile1 = readfile1.Replace("[name]", name);
            readfile1 = readfile1.Replace("[vname]", vname);
            readfile1 = readfile1.Replace("[msg]", cds2.ToString());
            readfile1 = readfile1.Replace("[orderid]", OrderId);
            string txtmessage1 = readfile1;
            string subj1 = "order has been placed";
            emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1, null);
            string msg = OrderId;


            return Json(msg);
        }
        public IHttpActionResult orderdetails(string select, string packageid, string date, string timeslot)
        {

            var ksc2 = new ksc1();
            if (select != null && select != "null" && select != "")
            {
                
                UserLogin userLogin = new UserLogin();
                userLogin.UserName = "Sireesh.k@xsilica.com";
                userLogin.Password = "ksc";
                userLogin = resultsPageService.GetUserLogin(userLogin);
                string uid = userLogin.UserLoginId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                    vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                    var VendorId = vendorMaster.Id.ToString();

                    List<string> sdate = new List<string>();
                    List<string> stimeslot = new List<string>();
                    List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();

                    var select1 = select.Split(',');
                    ksc2.location = select1[0];
                    var guest = ksc2.guests = select1[1];
                    ksc2.eventtype = select1[2];
                    var pkgs = packageid.Split(',');
                    var date1 = date.Trim(',').Split(',');
                    var timeslot1 = timeslot.Split(',');
                    for (int i = 0; i < pkgs.Count(); i++)
                    {

                        var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                        int price;
                        if (data.PackagePrice == null)
                        { price = Convert.ToInt16(data.price1); }
                        else { price = Convert.ToInt16(data.PackagePrice); }
                        tprice = tprice + price;
                        for (int j = 0; j < date1.Count(); j++)
                        {
                            if (date1[j].Split('~')[1] == data.VendorSubId.ToString())
                            {
                                data.UpdatedDate = Convert.ToDateTime(date1[j].Split('~')[0]);
                                data.timeslot = timeslot1[j].Split('~')[0];
                            }
                            else
                            {
                                //  data.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                                data.timeslot = "";

                            }
                        }
                        package.Add(data);
                    }
                    var tot = tprice * Convert.ToInt32(guest);
                    var gtot = tot + (tot * 0.18);
                    ksc2.package = package.ToString();
                    ksc2.tot = tot.ToString();
                    ksc2.gtot = gtot.ToString();

                }
            
            return Json(ksc2);
        }


    public class ksc1 {
            public string guests;

            public string package { get; set; }
     public string tot { get; set; }
    public string gtot { get; set; }
            public string location { get; set; }
            public string eventtype { get; set; }
        }
        public IHttpActionResult GetParticularPackage(string pid)
        {
           
                SPGETpartpkg_Result package = newmanageuse.getpartpkgs(pid).FirstOrDefault();
                return Json(package);
          
        }
        public IHttpActionResult reseller(string VendorId)
        {
           var resellername = partnerservice.GetPartners(VendorId);

            return Json(resellername);
        }
    }
}
