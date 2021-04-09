using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminDownloadedNoteController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public AdminDownloadedNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: AdminDownloadedNote
        [Route("AdminDownloadedNote")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(string search, string sortOrder, string seller, string buyer, string note, string noteId, string Bid, int adlPage = 1)
        {
            ViewBag.Notes = "active";
            ViewBag.ADN = "active";

            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellerSortParm = sortOrder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.BuyerSortParm = sortOrder == "Buyer" ? "Buyer_desc" : "Buyer";
            ViewBag.SellTypeSortParm = sortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
           

            var Notes = db.Downloads.Where(x=>x.IsSellerHasAllowedDownload == true).ToList();

            if (!String.IsNullOrEmpty(Bid))
            {
                Notes = Notes.Where(x => x.Downloader == Convert.ToInt32(Bid)).ToList();
            }

            if (!String.IsNullOrEmpty(noteId))
            {
                Notes = Notes.Where(x => x.NoteID == Convert.ToInt32(noteId)).ToList();
            }

            ViewBag.Seller = Notes.Select(x => (x.User1.FirstName + " " + x.User1.LastName)).OrderBy(x => x).Distinct().ToList();
            ViewBag.Buyer = Notes.Select(x => (x.User.FirstName + " " + x.User.LastName)).OrderBy(x => x).Distinct().ToList();
            ViewBag.Note = Notes.Select(x => x.NoteTitle).OrderBy(x => x).Distinct().ToList();

            if (seller != null)
            {
                Notes = Notes.Where(x => (x.User1.FirstName + " " + x.User1.LastName).Contains(seller)).ToList();
            }
            if (buyer != null)
            {
                Notes = Notes.Where(x => (x.User.FirstName + " " + x.User.LastName).Contains(buyer)).ToList();
            }
            if (note != null)
            {
                Notes = Notes.Where(x => x.NoteTitle.Contains(note)).ToList();
            }

            if (search != null)
            {
                Notes = Notes.Where(x => x.NoteTitle.Contains(search) ||
                                        x.NoteCategory.Contains(search) ||
                                        x.User1.FirstName.Contains(search) || x.User1.LastName.Contains(search) || (x.User1.FirstName + " " + x.User1.LastName).Contains(search) ||
                                        x.User.FirstName.Contains(search) || x.User.LastName.Contains(search) || (x.User.FirstName + " " + x.User.LastName).Contains(search) ||
                                        x.PurchasedPrice.ToString().Contains(search) ||
                                        x.IsPaid.ToString().Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "Title_desc":
                    Notes = Notes.OrderByDescending(s => s.NoteTitle).ToList();
                    break;
                case "Title":
                    Notes = Notes.OrderBy(s => s.NoteTitle).ToList();
                    break;
                case "Category_desc":
                    Notes = Notes.OrderByDescending(s => s.NoteCategory).ToList();
                    break;
                case "Category":
                    Notes = Notes.OrderBy(s => s.NoteCategory).ToList();
                    break;
                case "SellType_desc":
                    Notes = Notes.OrderByDescending(s => s.IsPaid).ToList();
                    break;
                case "SellType":
                    Notes = Notes.OrderBy(s => s.IsPaid).ToList();
                    break;
                case "Price_desc":
                    Notes = Notes.OrderByDescending(s => s.PurchasedPrice).ToList();
                    break;
                case "Price":
                    Notes = Notes.OrderBy(s => s.PurchasedPrice).ToList();
                    break;
                case "Seller_desc":
                    Notes = Notes.OrderByDescending(s => s.User1.FirstName).ToList();
                    break;
                case "Seller":
                    Notes = Notes.OrderBy(s => s.User1.FirstName).ToList();
                    break;
                case "Buyer_desc":
                    Notes = Notes.OrderByDescending(s => s.User.FirstName).ToList();
                    break;
                case "Buyer":
                    Notes = Notes.OrderBy(s => s.User.FirstName).ToList();
                    break;
                case "Date_desc":
                    Notes = Notes.OrderByDescending(s => s.AttachmentDownloadedDate).ToList();
                    break;
                case "Date":
                    Notes = Notes.OrderBy(s => s.AttachmentDownloadedDate).ToList();
                    break;
                default:
                    Notes = Notes.OrderByDescending(s => s.AttachmentDownloadedDate).ToList();
                    break;
            }




            var pager = new ADPager(Notes.Count(), adlPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = adlPage;

            ViewBag.srno = adlPage;
            ViewBag.TotalAdminDLPage = Math.Ceiling(Notes.Count() / 5.0);
            Notes = Notes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Notes);
        }
    }
}