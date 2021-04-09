using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class SpamReportController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public SpamReportController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: SpamReport
        [Authorize(Roles = "Admin, SuperAdmin")]
        [Route("SpamReport")]
        public ActionResult Index(string search, string sortOrder, int srPage = 1)
        {
            ViewBag.Reports = "active";
            ViewBag.SpamReport = "active";

            ViewBag.RBySortParm = sortOrder == "RBy" ? "RBy_desc" : "RBy";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.RemarkSortParm = sortOrder == "Remark" ? "Remark_desc" : "Remark";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();


            var Spam = db.SellerNotesReportedIssues.Where(x=>x.SellerNote.Status == 4).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                Spam = Spam.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(search) ||
                                        x.SellerNote.Title.Contains(search) || 
                                        x.SellerNote.NoteCategory.Name.Contains(search) || 
                                        x.CreatedDate.ToString().Contains(search) || 
                                        x.Remarks.Contains(search)).ToList();
            }


            switch (sortOrder)
            {
                case "Title_desc":
                    Spam = Spam.OrderByDescending(x => x.SellerNote.Title).ToList();
                    break;
                case "Title":
                    Spam = Spam.OrderBy(x => x.SellerNote.Title).ToList();
                    break;
                case "Category_desc":
                    Spam = Spam.OrderByDescending(x => x.SellerNote.NoteCategory.Name).ToList();
                    break;
                case "Category":
                    Spam = Spam.OrderBy(x => x.SellerNote.NoteCategory.Name).ToList();
                    break;
                case "Date_desc":
                    Spam = Spam.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
                case "Date":
                    Spam = Spam.OrderBy(x => x.CreatedDate).ToList();
                    break;
                case "RBy_desc":
                    Spam = Spam.OrderByDescending(x => x.User.FirstName).ToList();
                    break;
                case "RBy":
                    Spam = Spam.OrderBy(x => x.User.FirstName).ToList();
                    break;
                case "Remark_desc":
                    Spam = Spam.OrderByDescending(x => x.Remarks).ToList();
                    break;
                case "Remark":
                    Spam = Spam.OrderBy(x => x.Remarks).ToList();
                    break;
                default:
                    Spam = Spam.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(Spam.Count(), srPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = srPage;


            ViewBag.srno = srPage;
            ViewBag.TotalSpamReportPage = Math.Ceiling(Spam.Count() / 5.0);
            Spam = Spam.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Spam);
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult DeleteSpam(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            var Spam = db.SellerNotesReportedIssues.Where(x => x.ID == id).FirstOrDefault();

            db.SellerNotesReportedIssues.Remove(Spam);
            db.SaveChanges();

            return RedirectToAction("Index", "SpamReport");
        }
    }
}