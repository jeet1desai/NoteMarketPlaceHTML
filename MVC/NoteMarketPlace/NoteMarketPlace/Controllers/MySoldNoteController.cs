using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class MySoldNoteController : Controller
    {

        readonly NoteMarketPlaceEntities db;

        public MySoldNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }




        // GET: MySoldNote
        [Route("MySoldNote")]
        [Authorize]
        public ActionResult Index(string search, string sortOrder, int snpage = 1)
        {
            ViewBag.MySoldNote = "active";
            ViewBag.Class = "white-nav";


            //For Sorting
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.SellTypeSortParm = sortOrder == "SellType" ? "SellType_desc" : "SellType";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";

            ViewBag.pageNumber = snpage;


            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);


            //SoldNote Query 
            var SoldNotes = from dwl in db.Downloads join up in db.Users on dwl.Downloader equals up.ID
                            where dwl.Seller == user.ID && dwl.IsSellerHasAllowedDownload == true
                            select new SoldNoteModel { MySoldNote = dwl, Users = up};



            //Search is not Null 
            if (!string.IsNullOrEmpty(search))
            {
                SoldNotes = SoldNotes.Where(x => x.MySoldNote.NoteTitle.Contains(search) || x.Users.Email.Contains(search) || x.MySoldNote.NoteCategory.Contains(search));
            }

            //Sorting
            switch (sortOrder)
            {
                case "Title_desc":
                    SoldNotes = SoldNotes.OrderByDescending(s => s.MySoldNote.NoteTitle);
                    break;
                case "Title":
                    SoldNotes = SoldNotes.OrderBy(s => s.MySoldNote.NoteTitle);
                    break;
                case "Category_desc":
                    SoldNotes = SoldNotes.OrderByDescending(s => s.MySoldNote.NoteCategory);
                    break;
                case "Category":
                    SoldNotes = SoldNotes.OrderBy(s => s.MySoldNote.NoteCategory);
                    break;
                case "Email_desc":
                    SoldNotes = SoldNotes.OrderByDescending(s => s.Users.Email);
                    break;
                case "Email":
                    SoldNotes = SoldNotes.OrderBy(s => s.Users.Email);
                    break;
                case "SellType_desc":
                    SoldNotes = SoldNotes.OrderByDescending(s => s.MySoldNote.IsPaid);
                    break;
                case "SellType":
                    SoldNotes = SoldNotes.OrderBy(s => s.MySoldNote.IsPaid);
                    break;
                case "Price_desc":
                    SoldNotes = SoldNotes.OrderByDescending(s => s.MySoldNote.PurchasedPrice);
                    break;
                case "Price":
                    SoldNotes = SoldNotes.OrderBy(s => s.MySoldNote.PurchasedPrice);
                    break;
                case "Date_desc":
                    SoldNotes = SoldNotes.OrderByDescending(s => s.MySoldNote.AttachmentDownloadedDate);
                    break;
                case "Date":
                    SoldNotes = SoldNotes.OrderBy(s => s.MySoldNote.AttachmentDownloadedDate);
                    break;
                default:
                    SoldNotes = SoldNotes.OrderByDescending(s => s.MySoldNote.AttachmentDownloadedDate);
                    break;
            }


            //Pagination
            var pager = new BRPager(SoldNotes.Count(), snpage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = snpage;

            ViewBag.srno = snpage;
            ViewBag.TotalSoldNotePage = Math.Ceiling(SoldNotes.Count() / 10.0);
            SoldNotes = SoldNotes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);
            //SoldNotes = SoldNotes.Skip((snpage - 1) * 10).Take(10);

            return View(SoldNotes);
        }





        //In SoldNote Download Note in Zip Formation
        [Authorize]
        public FileResult DownloadNote(int id)
        {
            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);


            //Find Note in SellerNote Table
            var note = db.SellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true && x.ID == id).FirstOrDefault();

            if (note == null)
            {
                return File("", "application/zip");
            }

            string path = Server.MapPath("~/Members/" + user.ID + "/" + note.ID + "/Attachements/");

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
    }
}