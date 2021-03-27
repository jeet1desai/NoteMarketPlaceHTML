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
    public class MyDownloadController : Controller
    {

        readonly NoteMarketPlaceEntities db;

        public MyDownloadController()
        {
            db = new NoteMarketPlaceEntities();
        }




        // GET: MyDownload
        [Route("MyDownload")]
        [HttpGet]
        [Authorize]
        public ActionResult Index(string search, string sortOrder, int mdpage = 1)
        {
            ViewBag.MyDownload = "active";
            ViewBag.Class = "white-nav";

            ViewBag.pageNumber = mdpage;


            //For Sorting
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.SellTypeSortParm = sortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";


            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.Email = user.Email;


            //MyDownload table Query
            var note = db.Downloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true).ToList();
            

            //If Search not Null 
            if(search != null)
            {
                note = note.Where(x => x.NoteTitle.Contains(search) || x.NoteCategory.Contains(search)).ToList();
            }


            //Sorting
            switch (sortOrder)
            {
                case "Title_desc":
                    note = note.OrderByDescending(x => x.NoteTitle).ToList();
                    break;
                case "Title":
                    note = note.OrderBy(x => x.NoteTitle).ToList();
                    break;
                case "Category_desc":
                    note = note.OrderByDescending(x => x.NoteCategory).ToList();
                    break;
                case "Category":
                    note = note.OrderBy(x => x.NoteCategory).ToList();
                    break;
                case "Date_desc":
                    note = note.OrderByDescending(x => x.AttachmentDownloadedDate).ToList();
                    break;
                case "Date":
                    note = note.OrderBy(x => x.AttachmentDownloadedDate).ToList();
                    break;
                case "SellType_desc":
                    note = note.OrderByDescending(x => x.IsPaid).ToList();
                    break;
                case "SellType":
                    note = note.OrderBy(x => x.IsPaid).ToList();
                    break;
                case "Price_desc":
                    note = note.OrderByDescending(x => x.PurchasedPrice).ToList();
                    break;
                case "Price":
                    note = note.OrderBy(x => x.PurchasedPrice).ToList();
                    break;
                default:
                    note = note.OrderByDescending(x => x.AttachmentDownloadedDate).ToList();
                    break;
            }


            ViewBag.srno = mdpage;
            ViewBag.TotalMyDownloadPage = Math.Ceiling(note.Count() / 10.0);
            note = note.Skip((mdpage - 1) * 10).Take(10).ToList();

            return View(note);
        }







        //Add Review
        [HttpPost]
        [Authorize]
        public ActionResult AddReview(int id)
        {
            SellerNotesReview sellerNotesReview = new SellerNotesReview();

            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Find Note in Download Table
            var download = db.Downloads.Where(x => x.ID == id).FirstOrDefault();

            int rate = Convert.ToInt32(Request.Form["rate"]);
            string comment = Request.Form["cmt"];

            if (rate == 0)
            {
                return Content("Please give Rating");
            }

            if (string.IsNullOrEmpty(comment))
            {
                return Content("Please Enter Comment");
            }

            //Save Info on SellerNoteReview Table
            sellerNotesReview.NoteID = download.NoteID;
            sellerNotesReview.ReviewedByID = user.ID;
            sellerNotesReview.AgainstDownloadsID = download.ID;
            sellerNotesReview.Ratings = rate;
            sellerNotesReview.Comments = comment;
            sellerNotesReview.CreatedDate = DateTime.Now;
            sellerNotesReview.CreatedBy = user.ID;
            sellerNotesReview.ModifiedDate = DateTime.Now;
            sellerNotesReview.ModifiedBy = user.ID;
            sellerNotesReview.IsActive = true;

            db.SellerNotesReviews.Add(sellerNotesReview);
            db.SaveChanges();

            return RedirectToAction("Index", "MyDownload");
        }





        //Report as Inappropriate
        [HttpPost]
        [Authorize]
        public ActionResult Spam(int id)
        {
            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Find Note on Download Table
            var download = db.Downloads.Where(x => x.ID == id).FirstOrDefault();


            //Note Seller
            var seller = db.Users.FirstOrDefault(x => x.ID == download.Seller);


            SellerNotesReportedIssue reportedIssue = new SellerNotesReportedIssue();
            string remark = Request.Form["Remark"];

            var checkAlreadyReport = db.SellerNotesReportedIssues.Where(x => x.NoteID == download.NoteID && x.ReportedByID == user.ID && x.AgainstDownloadID == download.ID).FirstOrDefault();
            if(checkAlreadyReport != null)
            {
                return RedirectToAction("Index", "MyDownload");
            }

            if (string.IsNullOrEmpty(remark))
            {
                return Content("Please enter Remark...");
            }


            //Save info in SellerNotesReportedIssue Table
            reportedIssue.NoteID = download.NoteID;
            reportedIssue.ReportedByID = user.ID;
            reportedIssue.AgainstDownloadID = download.ID;
            reportedIssue.Remarks = remark;
            reportedIssue.CreatedDate = DateTime.Now;
            reportedIssue.CreatedBy = user.ID;
            reportedIssue.ModifiedDate = DateTime.Now;
            reportedIssue.ModifiedBy = user.ID;

            db.SellerNotesReportedIssues.Add(reportedIssue);
            db.SaveChanges();


            //Send Mail to Admin to Check Note
            var body = "<p>Hello Admins,</p><br>" +
                "<p>We want to inform you that, {0} Reported an issue for {1}’s Note with title {2}. Please look at the notes and take required actions. </p><br><br>" +
                "<p>Regards,</p>" +
                "<p>Note MarketPlace</p>";

            var message = new MailMessage();
            message.To.Add(new MailAddress("******@gmail.com"));  // Reciever 
            message.From = new MailAddress("******@******.ac.in");  // Sender
            message.Subject = user.FirstName + " " + user.LastName + " " + "Reported an issue for " + download.NoteTitle;
            message.Body = string.Format(body, user.FirstName + " " + user.LastName, seller.FirstName + " " + seller.LastName, download.NoteTitle);
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

            return RedirectToAction("Index", "MyDownload");
        }

    }
}