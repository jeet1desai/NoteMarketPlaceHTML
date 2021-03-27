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
    public class MyRejectNoteController : Controller
    {
        readonly NoteMarketPlaceEntities db;

        public MyRejectNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }




        // GET: MyRejectNote
        [Route("MyRejectNote")]
        [Authorize]
        public ActionResult Index(string search, string sortOrder, int rjpage = 1)
        {

            ViewBag.MyRejectNote = "active";
            ViewBag.Class = "white-nav";

            //For Sorting
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.RemarkSortParm = sortOrder == "Remark" ? "Remark_desc" : "Remark";

            ViewBag.pageNumber = rjpage;


            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);


            //Rjected Note Table Query
            var rejectednotes = from slr in db.SellerNotes join cat in db.NoteCategories on slr.Category equals cat.ID
                                where slr.SellerID == user.ID && slr.Status == 5 && slr.IsActive == true
                                select new RejectedNoteModel { MyRejectedNote = slr, NoteCategory = cat };


            //If Search is Not Null
            if (!string.IsNullOrEmpty(search))
            {
                rejectednotes = rejectednotes.Where(x => x.MyRejectedNote.Title.Contains(search) || x.MyRejectedNote.AdminRemarks.Contains(search) || x.NoteCategory.Name.Contains(search));
            }


            //Sorting
            switch (sortOrder)
            {
                case "Title_desc":
                    rejectednotes = rejectednotes.OrderByDescending(s => s.MyRejectedNote.Title);
                    break;
                case "Title":
                    rejectednotes = rejectednotes.OrderBy(s => s.MyRejectedNote.Title);
                    break;
                case "Category_desc":
                    rejectednotes = rejectednotes.OrderByDescending(s => s.NoteCategory.Name);
                    break;
                case "Category":
                    rejectednotes = rejectednotes.OrderBy(s => s.NoteCategory.Name);
                    break;
                case "Remark_desc":
                    rejectednotes = rejectednotes.OrderByDescending(s => s.MyRejectedNote.AdminRemarks);
                    break;
                case "Remark":
                    rejectednotes = rejectednotes.OrderBy(s => s.MyRejectedNote.AdminRemarks);
                    break;
                default:
                    rejectednotes = rejectednotes.OrderByDescending(s => s.MyRejectedNote.ModifiedDate);
                    break;
            }

            ViewBag.srno = rjpage;


            //Pagination
            var pager = new RJPager(rejectednotes.Count(), rjpage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = rjpage;


            ViewBag.TotalRejectNotePage = Math.Ceiling(rejectednotes.Count() / 10.0);
            //rejectednotes = rejectednotes.Skip((rjpage - 1) * 10).Take(10);
            rejectednotes = rejectednotes.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            return View(rejectednotes);
        }




        //On RejectNote page Download Note
        [Authorize]
        public FileResult DownloadNote(int id)
        {
            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Find Note
            var note = db.SellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true && x.ID == id).FirstOrDefault();

            //If not Found Note
            if (note == null)
            {
                return File("", "application/zip");
            }

            string path = Server.MapPath("~/Members/" + user.ID + "/" + note.ID + "/Attachements/");


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




        //Clone Note
        [Authorize]
        public ActionResult CloneNote(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var rejectednote = db.SellerNotes.Find(id);

            SellerNote clonenote = new SellerNote();

            clonenote.SellerID = rejectednote.SellerID;
            clonenote.Status = 1;
            clonenote.Title = rejectednote.Title;
            clonenote.Category = rejectednote.Category;
            clonenote.NoteType = rejectednote.NoteType;
            clonenote.NumberofPages = rejectednote.NumberofPages;
            clonenote.Description = rejectednote.Description;
            clonenote.UniversityName = rejectednote.UniversityName;
            clonenote.Country = rejectednote.Country;
            clonenote.Course = rejectednote.Course;
            clonenote.CourseCode = rejectednote.CourseCode;
            clonenote.Professor = rejectednote.Professor;
            clonenote.IsPaid = rejectednote.IsPaid;
            clonenote.SellingPrice = rejectednote.SellingPrice;
            clonenote.CreatedBy = user.ID;
            clonenote.CreatedDate = DateTime.Now;
            clonenote.ModifiedBy = user.ID;
            clonenote.ModifiedDate = DateTime.Now;
            clonenote.IsActive = true;

            db.SellerNotes.Add(clonenote);
            db.SaveChanges();

            clonenote = db.SellerNotes.Find(clonenote.ID);

            if (rejectednote.DisplayPicture != null)
            {
                var rejectednotefilepath = Server.MapPath(rejectednote.DisplayPicture);
                var clonenotefilepath = "~/Members/" + user.ID + "/" + clonenote.ID + "/";

                var filepath = Path.Combine(Server.MapPath(clonenotefilepath));

                FileInfo file = new FileInfo(rejectednotefilepath);

                Directory.CreateDirectory(filepath);
                if (file.Exists)
                {
                    System.IO.File.Copy(rejectednotefilepath, Path.Combine(filepath, Path.GetFileName(rejectednotefilepath)));
                }

                clonenote.DisplayPicture = Path.Combine(clonenotefilepath, Path.GetFileName(rejectednotefilepath));
                db.SaveChanges();
            }

            if (rejectednote.NotesPreview != null)
            {
                var rejectednotefilepath = Server.MapPath(rejectednote.NotesPreview);
                var clonenotefilepath = "~/Members/" + user.ID + "/" + clonenote.ID + "/";

                var filepath = Path.Combine(Server.MapPath(clonenotefilepath));

                FileInfo file = new FileInfo(rejectednotefilepath);

                Directory.CreateDirectory(filepath);

                if (file.Exists)
                {
                    System.IO.File.Copy(rejectednotefilepath, Path.Combine(filepath, Path.GetFileName(rejectednotefilepath)));
                }

                clonenote.NotesPreview = Path.Combine(clonenotefilepath, Path.GetFileName(rejectednotefilepath));
                db.SaveChanges();
            }

            var rejectednoteattachement = Server.MapPath("~/Members/" + user.ID + "/" + rejectednote.ID + "/Attachements/");
            var clonenoteattachement = "~/Members/" + user.ID + "/" + clonenote.ID + "/Attachements/";

            var attachementfilepath = Path.Combine(Server.MapPath(clonenoteattachement));

            Directory.CreateDirectory(attachementfilepath);

            foreach (var files in Directory.GetFiles(rejectednoteattachement))
            {
                FileInfo file = new FileInfo(files);

                if (file.Exists)
                {
                    System.IO.File.Copy(file.ToString(), Path.Combine(attachementfilepath, Path.GetFileName(file.ToString())));
                }

                SellerNotesAttachement attachement = new SellerNotesAttachement();
                attachement.NoteID = clonenote.ID;
                attachement.FileName = Path.GetFileName(file.ToString());
                attachement.FilePath = clonenoteattachement;
                attachement.CreatedDate = DateTime.Now;
                attachement.CreatedBy = user.ID;
                attachement.ModifiedDate = DateTime.Now;
                attachement.ModifiedBy = user.ID;
                attachement.IsActive = true;

                db.SellerNotesAttachements.Add(attachement);
                db.SaveChanges();
            }


            return RedirectToAction("Index", "SellNote");
        }

    }
}