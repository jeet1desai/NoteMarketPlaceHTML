using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class BuyerRequestController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public BuyerRequestController()
        {
            db = new NoteMarketPlaceEntities();
        }



        // GET: BuyerRequest
        [Route("BuyerRequest")]
        [Authorize]
        public ActionResult Index(string search, string sortOrder, int brpage = 1)
        {
            ViewBag.BuyReq = "active";
            ViewBag.Class = "white-nav";

            ViewBag.pageNumber = brpage;

            //For Sorting
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.BEmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";


            BuyerRequestModel buyerRequest = new BuyerRequestModel();

            //Auth user
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            //Buyerrequest Query
            var buyerrequestmodel = from dwl in db.Downloads
                                    join ur in db.Users on dwl.Downloader equals ur.ID
                                    join up in db.UserProfiles on dwl.Downloader equals up.UserID
                                    where dwl.Seller == user.ID && dwl.IsSellerHasAllowedDownload == false
                                    select new BuyerRequestModel { Downloadstbl = dwl, UserProfiletbl = up, Userstbl = ur };

            //If Search not Null
            if (!string.IsNullOrEmpty(search))
            {
                buyerrequestmodel = buyerrequestmodel.Where(x => x.Downloadstbl.NoteTitle.Contains(search) || x.Downloadstbl.NoteCategory.Contains(search) || x.Userstbl.FirstName.Contains(search) || x.Userstbl.LastName.Contains(search) || x.Userstbl.Email.Contains(search) || x.UserProfiletbl.PhoneNumber.Contains(search));
            }

            //Sorting
            switch (sortOrder)
            {
                case "Title_desc":
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.Downloadstbl.NoteTitle);
                    break;
                case "Title":
                    buyerrequestmodel = buyerrequestmodel.OrderBy(s => s.Downloadstbl.NoteTitle);
                    break;
                case "Category_desc":
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.Downloadstbl.NoteCategory);
                    break;
                case "Category":
                    buyerrequestmodel = buyerrequestmodel.OrderBy(s => s.Downloadstbl.NoteCategory);
                    break;
                case "Email_desc":
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.Userstbl.Email);
                    break;
                case "Email":
                    buyerrequestmodel = buyerrequestmodel.OrderBy(s => s.Userstbl.Email);
                    break;
                case "Phone_desc":
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.UserProfiletbl.PhoneNumber);
                    break;
                case "Phone":
                    buyerrequestmodel = buyerrequestmodel.OrderBy(s => s.UserProfiletbl.PhoneNumber);
                    break;
                case "Date_desc":
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.Downloadstbl.CreatedDate);
                    break;
                case "Date":
                    buyerrequestmodel = buyerrequestmodel.OrderBy(s => s.Downloadstbl.CreatedDate);
                    break;
                case "Price_desc":
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.Downloadstbl.PurchasedPrice);
                    break;
                case "Price":
                    buyerrequestmodel = buyerrequestmodel.OrderBy(s => s.Downloadstbl.PurchasedPrice);
                    break;
                default:
                    buyerrequestmodel = buyerrequestmodel.OrderByDescending(s => s.Downloadstbl.CreatedDate);
                    break;
            }

            //Pagination
            var pager = new BRPager(buyerrequestmodel.Count(), brpage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = brpage;


            ViewBag.srno = brpage;
            ViewBag.TotalBuyerRequestPage = Math.Ceiling(buyerrequestmodel.Count() / 10.0);
            //buyerrequestmodel = buyerrequestmodel.Skip((brpage - 1) * 10).Take(10);
            buyerrequestmodel = buyerrequestmodel.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            return View(buyerrequestmodel);
        }


        //Payment Recieve
        [Authorize]
        public ActionResult PaymentRecieve(int id)
        {
            //Find Entry on Download Table
            Download download = db.Downloads.Find(id);

            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //in Download Note find Seller and Downloader
            var Downloader = db.Users.Where(x => x.ID == download.Downloader).FirstOrDefault();
            var Seller = db.Users.Where(x => x.ID == download.Seller).FirstOrDefault();

            SellerNotesAttachement attachement = db.SellerNotesAttachements.Where(x => x.NoteID == download.NoteID).FirstOrDefault();

            //Make Change on Download Table
            db.Downloads.Attach(download);
            download.IsSellerHasAllowedDownload = true;
            download.AttachmentPath = attachement.FilePath;
            download.AttachmentDownloadedDate = DateTime.Now;
            download.ModifiedBy = user.ID;
            download.ModifiedDate = DateTime.Now;
            db.SaveChanges();

            //Send Email to Buyer after Payment Recieve
            var body = "<p>Hello, {0}</p><br>" +
                "<p>We would like to inform you that, {1} Allows you to download a note. Please login and see My Download tabs to download particular note.</p><br><br>" +
                "<p>Regards</p>" +
                "<p>Notes Marketplace</p>";

            var message = new MailMessage();
            message.To.Add(new MailAddress(Downloader.Email));  // Reciever 
            message.From = new MailAddress("******@******.ac.in");  // Sender
            message.Subject = Seller.FirstName + " " + Seller.LastName + " " + "Allows you to download a note";
            message.Body = string.Format(body, Downloader.FirstName+" "+Downloader.LastName, Seller.FirstName+" "+Seller.LastName);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "******@******.ac.in",
                    Password = "******"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            return RedirectToAction("Index");

        }
    }
}