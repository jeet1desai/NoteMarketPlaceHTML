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
    public class AdminPublishedNoteController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public AdminPublishedNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: AdminPublishedNote
        [Route("AdminPublishedNote")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(string search, string seller, string sortOrder, string Sid, int apnPage = 1)
        {
            ViewBag.Notes = "active";
            ViewBag.APN = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            

            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellTypeSortParm = sortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.SellerSortParm = sortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.ApproveBySortParm = sortOrder == "ApproveBy" ? "ApproveBy_desc" : "ApproveBy";
            ViewBag.NoOfDownloadSortParm = sortOrder == "NoOfDownload" ? "NoOfDownload_desc" : "NoOfDownload";


            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.Status == 4).ToList();

            if (!String.IsNullOrEmpty(Sid))
            {
                Notes = Notes.Where(x => x.SellerID == Convert.ToInt32(Sid)).ToList();
            }

            ViewBag.Seller = Notes.Select(x => x.User1.FirstName).OrderBy(x => x).Distinct().ToList();

            if (!String.IsNullOrEmpty(seller))
            {
                Notes = Notes.Where(x => x.User1.FirstName.Contains(seller)).ToList();
            }

            if (!String.IsNullOrEmpty(search))
            {
                Notes = Notes.Where(x => x.User1.FirstName.Contains(search) || x.User1.LastName.Contains(search) || x.Title.Contains(search) || x.NoteCategory.Name.Contains(search) || x.ReferenceData.Value.Contains(search) || (x.User1.FirstName + " " + x.User1.LastName).Contains(search) || x.User.FirstName.Contains(search) || x.User.LastName.Contains(search) || (x.User.FirstName + " " + x.User.LastName).Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "Title_desc":
                    Notes = Notes.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Title":
                    Notes = Notes.OrderBy(s => s.Title).ToList();
                    break;
                case "Category_desc":
                    Notes = Notes.OrderByDescending(s => s.NoteCategory.Name).ToList();
                    break;
                case "Category":
                    Notes = Notes.OrderBy(s => s.NoteCategory.Name).ToList();
                    break;
                case "SellType_desc":
                    Notes = Notes.OrderByDescending(s => s.IsPaid).ToList();
                    break;
                case "SellType":
                    Notes = Notes.OrderBy(s => s.IsPaid).ToList();
                    break;
                case "Price_desc":
                    Notes = Notes.OrderByDescending(s => s.SellingPrice).ToList();
                    break;
                case "Price":
                    Notes = Notes.OrderBy(s => s.SellingPrice).ToList();
                    break;
                case "Seller_desc":
                    Notes = Notes.OrderByDescending(s => s.User1.FirstName).ToList();
                    break;
                case "Seller":
                    Notes = Notes.OrderBy(s => s.User1.FirstName).ToList();
                    break;
                case "Date_desc":
                    Notes = Notes.OrderByDescending(s => s.PublishedDate).ToList();
                    break;
                case "Date":
                    Notes = Notes.OrderBy(s => s.PublishedDate).ToList();
                    break;
                case "ApproveBy_desc":
                    Notes = Notes.OrderByDescending(s => s.User.FirstName).ToList();
                    break;
                case "ApproveBy":
                    Notes = Notes.OrderBy(s => s.User.FirstName).ToList();
                    break;
                case "NoOfDownload_desc":
                    Notes = Notes.OrderByDescending(x => x.Downloads.Where(s => s.IsSellerHasAllowedDownload == true).Count()).ToList();
                    break;
                case "NoOfDownload":
                    Notes = Notes.OrderBy(x => x.Downloads.Where(s => s.IsSellerHasAllowedDownload == true).Count()).ToList();
                    break;
                default:
                    Notes = Notes.OrderByDescending(s => s.PublishedDate).ToList();
                    break;
            }


            var pager = new ADPager(Notes.Count(), apnPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = apnPage;

            ViewBag.srno = apnPage;
            ViewBag.TotalAdminPNPage = Math.Ceiling(Notes.Count() / 5.0);
            Notes = Notes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Notes);
        }



        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult UnPublishNote(int id)
        {
            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
           

            string Remark = Request.Form["Remark"];
            if (String.IsNullOrEmpty(Remark))
            {
                return Content("Please Enter Remark");
            }

            var Note = db.SellerNotes.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();

            var Seller = db.Users.Where(x => x.ID == Note.SellerID).FirstOrDefault();

            Note.ModifiedDate = DateTime.Now;
            Note.ModifiedBy = user.ID;
            Note.ActionedBy = user.ID;
            Note.AdminRemarks = Remark;
            Note.Status = 6;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();


            var body = "<p>Hello, {0}</p><br>" +
                "<p>We want to inform you that, your note {1} has been removed from the portal.</p>" +
                "<p>Please find our remarks as below -</p>" +
                "<p>{2}</p><br>" +
                "<p>Regards</p>" +
                "<p>Notes MarketPlace</p>";

            var message = new MailMessage();
            message.To.Add(new MailAddress(Seller.Email));  // Reciever 
            message.From = new MailAddress("170200107021@gecrajkot.ac.in");  // Sender
            message.Subject = "Sorry! We need to remove your notes from our portal. ";
            message.Body = string.Format(body, Seller.FirstName + " " + Seller.LastName, Note.Title, Note.AdminRemarks);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "170200107021@gecrajkot.ac.in",
                    Password = "*******"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            return RedirectToAction("Index", "AdminPublishedNote");
        }
    }
}