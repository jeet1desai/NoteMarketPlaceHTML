using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminRejectedNoteController : Controller
    {

        readonly private NoteMarketPlaceEntities db;

        public AdminRejectedNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: AdminRejectedNote
        [Route("AdminRejectedNote")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(string seller, string search, string sortOrder, int arjPage = 1)
        {
            ViewBag.Notes = "active";
            ViewBag.ARN = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParm = sortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.RejectBySortParm = sortOrder == "RejectBy" ? "RejectBy_desc" : "RejectBy";
            ViewBag.RemarkSortParm = sortOrder == "Remark" ? "Remark_desc" : "Remark";


            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.Status == 5).ToList();

            ViewBag.Seller = Notes.Select(x => x.User1.FirstName).OrderBy(x => x).Distinct().ToList();


            if (!String.IsNullOrEmpty(seller))
            {
                Notes = Notes.Where(x => (x.User1.FirstName + " " + x.User1.LastName).Contains(seller)).ToList();
            }

            if (!String.IsNullOrEmpty(search))
            {
                Notes = Notes.Where(x => x.User1.FirstName.Contains(search) || x.User1.LastName.Contains(search) || x.Title.Contains(search) || x.NoteCategory.Name.Contains(search) || (x.User1.FirstName + " " + x.User1.LastName).Contains(search) || x.User.FirstName.Contains(search) || x.User.LastName.Contains(search) || (x.User.FirstName + " " + x.User.LastName).Contains(search) || x.AdminRemarks.Contains(search)).ToList();
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
                case "Date_desc":
                    Notes = Notes.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
                case "Date":
                    Notes = Notes.OrderBy(s => s.ModifiedDate).ToList();
                    break;
                case "RejectBy_desc":
                    Notes = Notes.OrderByDescending(s => s.User.FirstName).ToList();
                    break;
                case "RejectBy":
                    Notes = Notes.OrderBy(s => s.User.FirstName).ToList();
                    break;
                case "Remark_desc":
                    Notes = Notes.OrderByDescending(s => s.AdminRemarks).ToList();
                    break;
                case "Remark":
                    Notes = Notes.OrderBy(s => s.AdminRemarks).ToList();
                    break;
                default:
                    Notes = Notes.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
            }

            var pager = new ADPager(Notes.Count(), arjPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = arjPage;

            ViewBag.srno = arjPage;
            ViewBag.TotalAdminRJPage = Math.Ceiling(Notes.Count() / 5.0);
            Notes = Notes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Notes);
        }




        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Approve(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();
            Notes.Status = 4;
            Notes.AdminRemarks = null;
            Notes.ActionedBy = user.ID;
            Notes.PublishedDate = DateTime.Now;
            Notes.ModifiedBy = user.ID;
            Notes.ModifiedDate = DateTime.Now;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminRejectedNote");
        }
    }
}