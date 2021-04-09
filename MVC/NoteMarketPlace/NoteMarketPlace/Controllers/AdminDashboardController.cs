using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminDashboardController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public AdminDashboardController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: AdminDashboard
        [Route("AdminDashboard")]
        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult Index(string month, string search, string sortOrder, int adpage = 1)
        {
            ViewBag.Dashboard = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            AdminDashboardModel adminDashboard = new AdminDashboardModel();

            //InReview Note
            adminDashboard.InReview = db.SellerNotes.Where(x => x.IsActive == true && (x.Status == 2 || x.Status == 3)).Count();

            //Last 7 day Download Note
            adminDashboard.Downlaod = db.Downloads.Where(x => x.ModifiedDate < DateTime.Now).ToList()
               .Where(x => x.ModifiedDate > DateTime.Now.AddDays(-7.0)).Count();

            //Last 7 day Registration
            adminDashboard.SignUp = db.Users.Where(x => x.ModifiedDate < DateTime.Now).ToList()
               .Where(x => x.ModifiedDate > DateTime.Now.AddDays(-7.0)).Count();


            DateTime dt = DateTime.Now;

            List<int> list = new List<int>();
            list.Add(dt.Month);
            for (int i = 1; i < 6; i++)
            {
                list.Add(dt.Month - i <= 0 ? (dt.Month - i) + 12 : dt.Month - i);
            }
            ViewBag.MonthList = new List<SelectListItem> { new SelectListItem { Text = list[0].ToString(), Value = "0" }, new SelectListItem { Text = list[1].ToString(), Value = "1" }, new SelectListItem { Text = list[2].ToString(), Value = "2" }, new SelectListItem { Text = list[3].ToString(), Value = "3" }, new SelectListItem { Text = list[4].ToString(), Value = "4" }, new SelectListItem { Text = list[5].ToString(), Value = "5" } };


            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.Status == 4).ToList();


            if (!String.IsNullOrEmpty(search))
            {
                Notes = Notes.Where(x => x.Title.Contains(search) || x.NoteCategory.Name.Contains(search) || (x.User1.FirstName + " " + x.User1.LastName).Contains(search) || x.User1.LastName.Contains(search) || x.User1.FirstName.Contains(search)).ToList();

            }

            if (String.IsNullOrEmpty(month) || Convert.ToInt32(month) == 0)
            {
                dt = DateTime.Now;
                var firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                Notes = Notes.Where(x => x.PublishedDate >= firstDayOfMonth && x.PublishedDate <= lastDayOfMonth).OrderByDescending(x=>x.PublishedDate).ToList();
            }
            else
            {
                DateTime PreviousMonth = DateTime.Now.AddMonths(-(Convert.ToInt32(month)));
                var firstDayOfPMonth = new DateTime(PreviousMonth.Year, PreviousMonth.Month, 1);
                var lastDayOfPMonth = new DateTime(PreviousMonth.Year, PreviousMonth.Month, DateTime.DaysInMonth(PreviousMonth.Year, PreviousMonth.Month));
                Notes = Notes.Where(x => x.PublishedDate >= firstDayOfPMonth && x.PublishedDate <= lastDayOfPMonth).OrderByDescending(x => x.PublishedDate).ToList();
            }


            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellTypeSortParm = sortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.PublisherSortParm = sortOrder == "Publisher" ? "Publisher_desc" : "Publisher";
            ViewBag.NoOfDownloadSortParm = sortOrder == "NoOfDownload" ? "NoOfDownload_desc" : "NoOfDownload";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";

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
                case "Publisher_desc":
                    Notes = Notes.OrderByDescending(s => s.User1.FirstName).ToList();
                    break;
                case "Publisher":
                    Notes = Notes.OrderBy(s => s.User1.FirstName).ToList();
                    break;
                case "NoOfDownload_desc":
                    Notes = Notes.OrderByDescending(x=>x.Downloads.Where(s => s.IsSellerHasAllowedDownload == true).Count()).ToList();
                    break;
                case "NoOfDownload":
                    Notes = Notes.OrderBy(x => x.Downloads.Where(s => s.IsSellerHasAllowedDownload == true).Count()).ToList();
                    break;
                case "Date_desc":
                    Notes = Notes.OrderByDescending(s => s.PublishedDate).ToList();
                    break;
                case "Date":
                    Notes = Notes.OrderBy(s => s.PublishedDate).ToList();
                    break;
                default:
                    Notes = Notes.OrderByDescending(s => s.PublishedDate).ToList();
                    break;
            }

            var pager = new ADPager(Notes.Count(), adpage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = adpage;


            ViewBag.srno = adpage;
            ViewBag.TotalADPage = Math.Ceiling(Notes.Count() / 5.0);
            adminDashboard.SellerNotes = Notes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            
            return View(adminDashboard);
        }









        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult DownloadNote(int id)
        {
            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Find Note
            var note = db.SellerNotes.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();

            //If not Found Note
            if (note == null)
            {
                return File("", "application/zip");
            }

            string path = Server.MapPath("~/Members/" + note.SellerID + "/" + note.ID + "/Attachements/");


            //Download Note in Zip Formate
            DirectoryInfo dir = new DirectoryInfo(path);

            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in dir.GetFiles())
                    {
                        string filepath = path + item.ToString();
                        ziparchive.CreateEntryFromFile(filepath, item.ToString());
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", note.Title + ".zip");
            }
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
                    Password = "********"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            return RedirectToAction("Index", "AdminDashboard");
        }
    }
}