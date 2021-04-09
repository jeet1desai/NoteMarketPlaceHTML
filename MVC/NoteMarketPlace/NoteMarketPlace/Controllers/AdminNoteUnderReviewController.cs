using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminNoteUnderReviewController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public AdminNoteUnderReviewController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: AdminNoteUnderReview
        [Route("AdminNoteUnderReview")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(string search, string seller, string sortOrder, string Sid, int anurPage = 1)
        {
            ViewBag.Notes = "active";
            ViewBag.ANUR = "active";

            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParm = sortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "Status_desc" : "Status";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && (x.Status == 2 || x.Status == 3)).ToList();


            ViewBag.Seller = Notes.Select(x => x.User1.FirstName).OrderBy(x => x).Distinct().ToList();

            if (!String.IsNullOrEmpty(Sid))
            {
                Notes = Notes.Where(x => x.SellerID == Convert.ToInt32(Sid)).ToList();
            }

            if (!String.IsNullOrEmpty(seller))
            {
                Notes = Notes.Where(x => x.User1.FirstName.Contains(seller)).ToList();
            }

            if (!String.IsNullOrEmpty(search))
            {
                Notes = Notes.Where(x => x.User1.FirstName.Contains(search) || (x.User1.FirstName + " " + x.User1.LastName).Contains(search) || x.User1.LastName.Contains(search) || x.Title.Contains(search) || x.NoteCategory.Name.Contains(search) || x.ReferenceData.Value.Contains(search)).ToList();
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
                case "Seller_desc":
                    Notes = Notes.OrderByDescending(s => s.User1.FirstName).ToList();
                    break;
                case "Seller":
                    Notes = Notes.OrderBy(s => s.User1.FirstName).ToList();
                    break;
                case "Status_desc":
                    Notes = Notes.OrderByDescending(s => s.ReferenceData.Value).ToList();
                    break;
                case "Status":
                    Notes = Notes.OrderBy(s => s.ReferenceData.Value).ToList();
                    break;
                case "Date_desc":
                    Notes = Notes.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
                case "Date":
                    Notes = Notes.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    Notes = Notes.OrderBy(s => s.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(Notes.Count(), anurPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = anurPage;

            ViewBag.srno = anurPage;
            ViewBag.TotalAdminNURPage = Math.Ceiling(Notes.Count() / 5.0);
            Notes = Notes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Notes);
        }












        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult InReview(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();
            Notes.Status = 3;
            Notes.ActionedBy = user.ID;
            Notes.ModifiedBy = user.ID;
            Notes.ModifiedDate = DateTime.Now;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminNoteUnderReview");
        }













        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Approve(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();
            Notes.Status = 4;
            Notes.ActionedBy = user.ID;
            Notes.PublishedDate = DateTime.Now;
            Notes.ModifiedBy = user.ID;
            Notes.ModifiedDate = DateTime.Now;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminNoteUnderReview");
        }









        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Reject(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            

            string Remark = Request.Form["remark"];
            if (String.IsNullOrEmpty(Remark))
            {
                return Content("Please Enter Remark");
            }

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();
            Notes.Status = 5;
            Notes.ActionedBy = user.ID;
            Notes.AdminRemarks = Remark;
            Notes.ModifiedBy = user.ID;
            Notes.ModifiedDate = DateTime.Now;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminNoteUnderReview");
        }
    }
}