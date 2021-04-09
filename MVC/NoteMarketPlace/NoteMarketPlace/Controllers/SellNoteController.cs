using NoteMarketPlace.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web;

namespace NoteMarketPlace.Controllers
{
    [Authorize]
    public class SellNoteController : Controller
    {
        readonly NoteMarketPlaceEntities db;

        public SellNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }





        // Searching, Sorting and Paging
        [Route("SellNote")]
        [Authorize]
        [HttpGet]
        public ActionResult Index(string search, string search2, string sortOrder, string sortOrderP, int inprogresspage = 1, int publishpage = 1)
        {
            ViewBag.Class = "white-nav";
            ViewBag.SellNote = "active";

            //For Sorting InProgress
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";


            //For Sorting Publish
            ViewBag.TitleSortParm1 = sortOrderP == "Title" ? "title_desc" : "Title";
            ViewBag.DateSortParm1 = sortOrderP == "Date" ? "date_desc" : "Date";
            ViewBag.CategorySortParm1 = sortOrderP == "Category" ? "category_desc" : "Category";
            ViewBag.STypeSortParm1 = sortOrderP == "SellType" ? "sellType_desc" : "SellType";
            ViewBag.PriceSortParm1 = sortOrderP == "Price" ? "price_desc" : "Price";

            ViewBag.PageNumber = inprogresspage;
            ViewBag.PageNumberPublished = publishpage;

            SellNoteDashboardModel dashboardviewmodel = new SellNoteDashboardModel();

            //Auth User
            var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Buyer Request
            var BuyerReqCount = db.Downloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == false).Count();
            dashboardviewmodel.BuyerRequest = BuyerReqCount;

            //My Download
            var MyDownloadCount = db.Downloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true).Count();
            dashboardviewmodel.MyDownload = MyDownloadCount;

            //My Rejected Note
            var RejectNote = db.SellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true && x.Status == 5).Count();
            dashboardviewmodel.MyRejectedNote = RejectNote;

            //Note Sold and Earn Money
            var SoldNote = db.Downloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == true).Count();
            dashboardviewmodel.NumberOfSoldNote = SoldNote;
            var EarnMoney = db.Downloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == true).Sum(x => x.PurchasedPrice);
            dashboardviewmodel.MoneyEarned = EarnMoney == null ? 0 : (int)EarnMoney;

            //InProgrss Query
            var IpNote = db.SellerNotes.Where(x => x.SellerID == user.ID && (x.Status == 1 || x.Status == 2 || x.Status == 3));
            //Publish Query
            var PNote = db.SellerNotes.Where(x => x.SellerID == user.ID && x.Status == 4);

            //Is InProgrss and Publish Search not Null
            if (search != null)
            {
                IpNote = IpNote.Where(x => x.Title.Contains(search) || x.NoteCategory.Name.Contains(search) || x.ReferenceData.Value.Contains(search));
            }
            if (search2 != null)
            {
                PNote = PNote.Where(x => x.Title.Contains(search2) || x.NoteCategory.Name.Contains(search2) || x.ReferenceData.Value.Contains(search2));
            }

            //Inprogrss Sorting
            switch (sortOrder)
            {
                case "title_desc":
                    dashboardviewmodel.InProgressNote = IpNote.OrderByDescending(s => s.Title);
                    break;
                case "Title":
                    dashboardviewmodel.InProgressNote = IpNote.OrderBy(s => s.Title);
                    break;
                case "category_desc":
                    dashboardviewmodel.InProgressNote = IpNote.OrderByDescending(s => s.NoteCategory.Name);
                    break;
                case "Category":
                    dashboardviewmodel.InProgressNote = IpNote.OrderBy(s => s.NoteCategory.Name);
                    break;
                case "status_desc":
                    dashboardviewmodel.InProgressNote = IpNote.OrderByDescending(s => s.Status);
                    break;
                case "Status":
                    dashboardviewmodel.InProgressNote = IpNote.OrderBy(s => s.Status);
                    break;
                case "date_desc":
                    dashboardviewmodel.InProgressNote = IpNote.OrderByDescending(s => s.ModifiedDate);
                    break;
                case "Date":
                    dashboardviewmodel.InProgressNote = IpNote.OrderBy(s => s.ModifiedDate);
                    break;
                default:
                    dashboardviewmodel.InProgressNote = IpNote.OrderByDescending(s => s.ModifiedDate);
                    break;
            }

            //Publish Sorting
            switch (sortOrderP)
            {
                case "title_desc":
                    dashboardviewmodel.PublishedNote = PNote.OrderByDescending(s => s.Title);
                    break;
                case "Title":
                    dashboardviewmodel.PublishedNote = PNote.OrderBy(s => s.Title);
                    break;
                case "category_desc":
                    dashboardviewmodel.PublishedNote = PNote.OrderByDescending(s => s.NoteCategory.Name);
                    break;
                case "Category":
                    dashboardviewmodel.PublishedNote = PNote.OrderBy(s => s.NoteCategory.Name);
                    break;
                case "sellType_desc":
                    dashboardviewmodel.PublishedNote = PNote.OrderByDescending(s => s.IsPaid);
                    break;
                case "SellType":
                    dashboardviewmodel.PublishedNote = PNote.OrderBy(s => s.IsPaid);
                    break;
                case "date_desc":
                    dashboardviewmodel.PublishedNote = PNote.OrderByDescending(s => s.ModifiedDate);
                    break;
                case "Date":
                    dashboardviewmodel.PublishedNote = PNote.OrderBy(s => s.ModifiedDate);
                    break;
                case "price_desc":
                    dashboardviewmodel.PublishedNote = PNote.OrderByDescending(s => s.SellingPrice);
                    break;
                case "Price":
                    dashboardviewmodel.PublishedNote = PNote.OrderBy(s => s.SellingPrice);
                    break;
                default:
                    dashboardviewmodel.PublishedNote = PNote.OrderByDescending(s => s.ModifiedDate);
                    break;
            }

            //Pagination
            ViewBag.TotalPagesInProgress = Math.Ceiling(dashboardviewmodel.InProgressNote.Count() / 5.0);
            ViewBag.TotalPagesInPublished = Math.Ceiling(dashboardviewmodel.PublishedNote.Count() / 5.0);
            dashboardviewmodel.InProgressNote = dashboardviewmodel.InProgressNote.Skip((inprogresspage - 1) * 5).Take(5);
            dashboardviewmodel.PublishedNote = dashboardviewmodel.PublishedNote.Skip((publishpage - 1) * 5).Take(5);

            return View(dashboardviewmodel);
        }







        //Delete Seller Note if Its in Draft
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Auth USer
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            //Check Note is Store in Draft Formate
            var note = db.SellerNotes.Where(x => x.ID == id && x.SellerID == user.ID && x.Status == 1).FirstOrDefault();

            if (note == null)
            {
                return Content("This Note is not Available....");
            }
            if (note.SellerID != user.ID)
            {
                return Content("You Can't Delete This Note, This Note doesn't Belong to You");
            }
            var noteFile = db.SellerNotesAttachements.Where(x => x.NoteID == note.ID).ToList();

            //Delete in Database Table
            foreach (var item in noteFile)
            {
                db.SellerNotesAttachements.Remove(item);
            }

            db.SaveChanges();

            db.SellerNotes.Remove(note);
            db.SaveChanges();

            //Delete in server Folder(Members)
            string PathPreview = Request.MapPath("~/Members/" + note.SellerID + "/" + note.ID);

            if (Directory.Exists(PathPreview))
            {
                DeleteDirectory(PathPreview);
            }

            return RedirectToAction("Index");
        }

        public void DeleteDirectory(string path)
        {
            foreach (string filename in Directory.GetFiles(path))
            {
                FileInfo file = new FileInfo(filename);
                file.Delete();
            }
            // Check all child Directories and delete files  
            foreach (string subfolder in Directory.GetDirectories(path))
            {
                DeleteDirectory(subfolder);
            }
            Directory.Delete(path);
        }










        //Add Note
        [Route("SellNote/AddNote")]
        [Authorize]
        [HttpGet]
        public ActionResult AddNote()
        {
            ViewBag.Class = "white-nav";

            var cat = db.NoteCategories.Where(x => x.IsActive == true).ToList();
            var typ = db.NoteTypes.Where(x => x.IsActive == true).ToList();
            var cnt = db.Countries.Where(x => x.IsActive == true).ToList();

            AddNoteModel viewModel = new AddNoteModel
            {
                NoteCategoryList = cat,
                NoteTypeList = typ,
                CountryList = cnt
            };
            return View(viewModel);

        }






        //Add Note
        [HttpPost]
        [Route("SellNote/AddNote")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddNote(AddNoteModel note, string Command)
        {
            ViewBag.Class = "white-nav";
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (user != null && ModelState.IsValid)
            {
                SellerNote sellernotes = new SellerNote();

                // Save note info in Database
                sellernotes.SellerID = user.ID;
                sellernotes.Title = note.Title;
                sellernotes.Status = Command == "Save" ? 1 : 2;
                sellernotes.Category = note.Category;
                sellernotes.NoteType = note.NoteType;
                sellernotes.NumberofPages = note.NumberofPages;
                sellernotes.Description = note.Description;
                sellernotes.UniversityName = note.UniversityName;
                sellernotes.Country = note.Country;
                sellernotes.Course = note.Course;
                sellernotes.CourseCode = note.CourseCode;
                sellernotes.Professor = note.Professor;
                sellernotes.IsPaid = note.IsPaid;
                sellernotes.SellingPrice = sellernotes.IsPaid == false ? 0 : note.SellingPrice;
                sellernotes.CreatedDate = DateTime.Now;
                sellernotes.ModifiedDate = DateTime.Now;
                sellernotes.CreatedBy = user.ID;
                sellernotes.ModifiedBy = user.ID;
                sellernotes.IsActive = true;

                // If seller add unvalid price
                if (note.IsPaid)
                {
                    if (note.SellingPrice == null || note.SellingPrice < 1)
                    {
                        ModelState.AddModelError("SellingPrice", "Enter valid Selling price");

                        AddNoteModel viewModel = GetDD();

                        return View(viewModel);
                    }
                }
                if (note.UploadNotes[0] == null)
                {
                    ModelState.AddModelError("UploadNotes", "Enter Note");

                    AddNoteModel viewModel = GetDD();

                    return View(viewModel);
                }

                db.SellerNotes.Add(sellernotes);
                db.SaveChanges();


                sellernotes = db.SellerNotes.Find(sellernotes.ID);

                //save note picture if User add
                if (note.DisplayPicture != null)
                {
                    string displaypicturefilename = Path.GetFileName(note.DisplayPicture.FileName);
                    string displaypicturepath = "~/Members/" + user.ID + "/" + sellernotes.ID + "/";
                    CreateDirectoryIfMissing(displaypicturepath);
                    string displaypicturefilepath = Path.Combine(Server.MapPath("~/Members/" + user.ID + "/" + sellernotes.ID + "/"), displaypicturefilename);
                    sellernotes.DisplayPicture = displaypicturepath + displaypicturefilename;
                    note.DisplayPicture.SaveAs(displaypicturefilepath);
                }
                else
                {
                    var filepath = db.SystemConfigurations.Where(x => x.Name == "DefaultNoteDisplayPicture").FirstOrDefault();
                    
                    sellernotes.DisplayPicture = filepath.Value;

                }


                //save note preview
                if (note.NotesPreview != null)
                {
                    string notespreviewfilename = System.IO.Path.GetFileName(note.NotesPreview.FileName);
                    string notespreviewpath = "~/Members/" + user.ID + "/" + sellernotes.ID + "/";
                    CreateDirectoryIfMissing(notespreviewpath);
                    string notespreviewfilepath = Path.Combine(Server.MapPath(notespreviewpath), notespreviewfilename);
                    sellernotes.NotesPreview = notespreviewpath + notespreviewfilename;
                    note.NotesPreview.SaveAs(notespreviewfilepath);
                }

                db.SellerNotes.Attach(sellernotes);
                db.Entry(sellernotes).Property(x => x.DisplayPicture).IsModified = true;
                db.Entry(sellernotes).Property(x => x.NotesPreview).IsModified = true;
                db.SaveChanges();

                //save note file
                if (note.UploadNotes[0] != null)
                {
                    foreach (HttpPostedFileBase file in note.UploadNotes)
                    {
                        if (file != null)
                        {
                            string notesattachementfilename = System.IO.Path.GetFileName(file.FileName);
                            string notesattachementpath = "~/Members/" + user.ID + "/" + sellernotes.ID + "/Attachements/";
                            CreateDirectoryIfMissing(notesattachementpath);
                            string notesattachementfilepath = Path.Combine(Server.MapPath(notesattachementpath), notesattachementfilename);

                            note.NotesPreview.SaveAs(notesattachementfilepath);


                            //save note file into SellerNotesAttachement table
                            SellerNotesAttachement notesattachements = new SellerNotesAttachement
                            {
                                NoteID = sellernotes.ID,
                                FileName = notesattachementfilename,
                                FilePath = notesattachementpath + notesattachementfilename,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user.ID,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = user.ID,
                                IsActive = true
                            };

                            db.SellerNotesAttachements.Add(notesattachements);

                            db.SaveChanges();
                        }
                    }
                }
                db.Dispose();

                return RedirectToAction("Index", "SellNote");
            }
            else
            {
                AddNoteModel viewModel = GetDD();
                return View(viewModel);
            }
        }







        //Edit Note
        [Route("SellNote/EditNote/{id}")]
        [Authorize]
        [HttpGet]
        public ActionResult EditNote(int id)
        {
            ViewBag.Class = "white-nav";

            //DropDown
            var cat = db.NoteCategories.Where(x => x.IsActive == true).ToList();
            var typ = db.NoteTypes.Where(x => x.IsActive == true).ToList();
            var cnt = db.Countries.Where(x => x.IsActive == true).ToList();

            //Auth USer
            var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Note Detail
            var note = db.SellerNotes.Where(x => x.ID == id && x.SellerID == user.ID && x.Status == 1).FirstOrDefault();

            var FileNote = db.SellerNotesAttachements.Where(x => x.NoteID == note.ID).ToList();
            string path = "";
            foreach (var item in FileNote)
            {
                path = path + item.FileName + ", ";
            }

            if (note == null)
            {
                return Content("You can't have access to this file");
            }

            EditNoteModel viewModel = new EditNoteModel();


            viewModel.NoteCategoryList = cat;
            viewModel.NoteTypeList = typ;
            viewModel.CountryList = cnt;

            viewModel.Title = note.Title;
            viewModel.Category = note.Category;
            viewModel.NoteType = note.NoteType;
            viewModel.NumberofPages = note.NumberofPages;
            viewModel.Description = note.Description;
            viewModel.Country = note.Country;
            viewModel.UniversityName = note.UniversityName;
            viewModel.Course = note.Course;
            viewModel.CourseCode = note.CourseCode;
            viewModel.Professor = note.Professor;
            viewModel.IsPaid = note.IsPaid;
            viewModel.SellingPrice = note.SellingPrice;

            viewModel.DpPathName = note.DisplayPicture;
            viewModel.NpPathName = note.NotesPreview;

            ViewBag.ImageName = Path.GetFileName(note.DisplayPicture);
            ViewBag.PreviewName = Path.GetFileName(note.NotesPreview);
            ViewBag.FileName = path;

            return View(viewModel);
        }







        [HttpPost]
        [Route("SellNote/EditNote/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditNote(EditNoteModel viewModel, string Command)
        {
            ViewBag.Class = "white-nav";
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            var note = db.SellerNotes.Where(x => x.ID == viewModel.ID && x.Status == 1 && x.SellerID == user.ID).FirstOrDefault();

            var AttachFile = db.SellerNotesAttachements.Where(x => x.NoteID == note.ID).ToList();

            if (user != null && ModelState.IsValid)
            {
                note.Title = viewModel.Title;
                note.Status = Command == "Save" ? 1 : 2;
                note.Category = viewModel.Category;
                note.NoteType = viewModel.NoteType;
                note.NumberofPages = viewModel.NumberofPages;
                note.Description = viewModel.Description;
                note.UniversityName = viewModel.UniversityName;
                note.Country = viewModel.Country;
                note.Course = viewModel.Course;
                note.CourseCode = viewModel.CourseCode;
                note.Professor = viewModel.Professor;
                note.IsPaid = viewModel.IsPaid;
                note.SellingPrice = viewModel.SellingPrice;
                note.ModifiedDate = DateTime.Now;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();


                if (viewModel.DisplayPicture != null)
                {
                    string FileNameDelete = System.IO.Path.GetFileName(note.DisplayPicture);
                    string PathImage = Request.MapPath("~/Members/" + note.SellerID + "/" + note.ID + "/" + FileNameDelete);
                    FileInfo file = new FileInfo(PathImage);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    string displaypicturefilename = Path.GetFileName(viewModel.DisplayPicture.FileName);
                    string displaypicturepath = "~/Members/" + note.SellerID + "/" + note.ID + "/";
                    string displaypicturefilepath = Path.Combine(Server.MapPath(displaypicturepath), displaypicturefilename);
                    note.DisplayPicture = displaypicturepath + displaypicturefilename;
                    viewModel.DisplayPicture.SaveAs(displaypicturefilepath);

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (viewModel.NotesPreview != null)
                {
                    string FileNameDelete = System.IO.Path.GetFileName(note.NotesPreview);
                    string PathPreview = Request.MapPath("~/Members/" + note.SellerID + "/" + note.ID + "/" + FileNameDelete);
                    FileInfo file = new FileInfo(PathPreview);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    string notespreviewfilename = Path.GetFileName(viewModel.NotesPreview.FileName);
                    string notespreviewpath = "~/Members/" + note.SellerID + "/" + note.ID + "/";
                    string notespreviewfilepath = Path.Combine(Server.MapPath(notespreviewpath), notespreviewfilename);
                    note.NotesPreview = notespreviewpath + notespreviewfilename;
                    viewModel.NotesPreview.SaveAs(notespreviewfilepath);

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                if (viewModel.UploadNotes != null)
                {
                    string deletepath = Request.MapPath("~/Members/" + note.SellerID + "/" + note.ID + "/Attachements/");
                    foreach (string filename in Directory.GetFiles(deletepath))
                    {
                        FileInfo file = new FileInfo(filename);
                        file.Delete();
                    }
                    foreach (var item in AttachFile)
                    {
                        db.SellerNotesAttachements.Remove(item);
                        db.SaveChanges();
                    }
                    foreach (HttpPostedFileBase file in viewModel.UploadNotes)
                    {
                        if (file != null)
                        {
                            string notesattachementfilename = System.IO.Path.GetFileName(file.FileName);
                            string notesattachementpath = "~/Members/" + note.SellerID + "/" + note.ID + "/Attachements/";
                            string notesattachementfilepath = Path.Combine(Server.MapPath(notesattachementpath), notesattachementfilename);

                            file.SaveAs(notesattachementfilepath);

                            SellerNotesAttachement notesattachements = new SellerNotesAttachement
                            {
                                NoteID = viewModel.ID,
                                FileName = notesattachementfilename,
                                FilePath = notesattachementpath + notesattachementfilename,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user.ID,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = user.ID,
                                IsActive = true
                            };

                            db.SellerNotesAttachements.Add(notesattachements);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "SellNote");
            }
            return View();
        }






        public AddNoteModel GetDD()
        {
            var cat = db.NoteCategories.Where(x => x.IsActive == true).ToList();
            var typ = db.NoteTypes.Where(x => x.IsActive == true).ToList();
            var cnt = db.Countries.Where(x => x.IsActive == true).ToList();

            AddNoteModel viewModel = new AddNoteModel
            {
                NoteCategoryList = cat,
                NoteTypeList = typ,
                CountryList = cnt
            };

            return viewModel;
        }



        private void CreateDirectoryIfMissing(string path)
        {
            bool folderExists = Directory.Exists(Server.MapPath(path));
            if (!folderExists)
            {
                Directory.CreateDirectory(Server.MapPath(path));
            }

        }
    }
}