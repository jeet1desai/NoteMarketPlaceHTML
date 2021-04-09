using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;

namespace NoteMarketPlace.Controllers
{

    public class SearchNoteController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public SearchNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: SearchNote
        [Route("SearchNote")]
        public ActionResult Index(string search, string type, string category, string university, string course, string country, string ratings, int page = 1)
        {
            ViewBag.Class = "white-nav";
            ViewBag.SN = "active";


            //For Sorting DropDown
            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.Type = type;
            ViewBag.University = university;
            ViewBag.Course = course;
            ViewBag.Country = country;
            ViewBag.Rating = ratings;


            //DropDown List
            ViewBag.CategoryList = db.NoteCategories.Where(x => x.IsActive == true).ToList();
            ViewBag.TypeList = db.NoteTypes.Where(x => x.IsActive == true).ToList();
            ViewBag.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();
            ViewBag.UniversityList = db.SellerNotes.Where(x => x.IsActive == true && x.UniversityName != null && x.Status == 4).Select(x => x.UniversityName).Distinct();
            ViewBag.CourseList = db.SellerNotes.Where(x => x.IsActive == true && x.Course != null && x.Status == 4).Select(x => x.Course).Distinct();
            ViewBag.RatingList = new List<SelectListItem> { new SelectListItem { Text = "1+", Value = "1" }, new SelectListItem { Text = "2+", Value = "2" }, new SelectListItem { Text = "3+", Value = "3" }, new SelectListItem { Text = "4+", Value = "4" }, new SelectListItem { Text = "5", Value = "5" } };


            //Pubblished NOte Query
            var note = db.SellerNotes.Where(x => x.Status == 4).ToList();


            //Search, Type, Category, University, Course, Couuntry, Rating is not Null
            if (!String.IsNullOrEmpty(search))
            {
                note = note.Where(x => x.Title.ToLower().Contains(search.ToLower()) ||
                                                 x.NoteCategory.Name.ToLower().Contains(search.ToLower())
                                            ).ToList();
            }

            if (!String.IsNullOrEmpty(type))
            {
                note = note.Where(x => x.NoteType.ToString().ToLower().Contains(type.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(category))
            {
                note = note.Where(x => x.Category.ToString().ToLower().Contains(category.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(university))
            {
                note = note.Where(x => x.UniversityName.ToLower().Contains(university.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(course))
            {
                note = note.Where(x => x.Course.ToLower().Contains(course.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(country))
            {
                note = note.Where(x => x.Country.ToString().ToLower().Contains(country.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(ratings))
            {
                List<SellerNote> searchNoteList = new List<SellerNote>();
                foreach (var item in note)
                {
                    var review = db.SellerNotesReviews.Where(x => x.NoteID == item.ID).Select(x=>x.Ratings);
                    var totalreview = review.Count();
                    var avgreview = totalreview > 0 ? Math.Ceiling(review.Average()) : 0;
                    if (avgreview >= Convert.ToInt32(ratings))
                    {
                        searchNoteList.Add(item);
                    }
                }
                note = searchNoteList;
            }

            //Order Note
            note = note.OrderByDescending(x => x.PublishedDate).ToList();

            ViewBag.TotalNote = note.Count();
            var pager = new Pager(note.Count(), page);

            var searchNote = new SearchNoteModel
            {
                Notes = note.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return View(searchNote);
        }







        //NoteDetail
        [OutputCache(Duration = 0)]
        [Route("SearchNote/NoteDetail/{id}")]
        [AllowAnonymous]
        public ActionResult NoteDetail(int id)
        {
            ViewBag.Class = "white-nav";

            if (TempData["Requested"] != null)
            {
                ViewBag.Requested = "Requested";
                Session["Req"] = "Requested";
            }

            //Auth User as Buyer
            var buyer = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            //Note
            var NoteDetail = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();

            //Note File for Download
            var NoteFileDetail = db.SellerNotesAttachements.Where(x => x.NoteID == NoteDetail.ID).FirstOrDefault();

            //Rating, Spam, Totol Review 
            var rating = db.SellerNotesReviews.Where(x => x.NoteID == id && x.IsActive == true).Average(x => (int?)x.Ratings) ?? 0;
            var totalreview = db.SellerNotesReviews.Where(x => x.NoteID == id && x.IsActive == true).Count();
            var spam = db.SellerNotesReportedIssues.Where(x => x.NoteID == id).Count();

            //Note Review Query
            IEnumerable<ReviewsModel> reviews = from review in db.SellerNotesReviews
                                                    join users in db.Users on review.ReviewedByID equals users.ID
                                                    join userprofile in db.UserProfiles on review.ReviewedByID equals userprofile.UserID
                                                    where review.NoteID == id && review.IsActive == true orderby review.Ratings descending, review.CreatedDate descending
                                                    select new ReviewsModel { SellerNotesReviews = review, Users = users, UserProfiles = userprofile };

            //If Note is not UnAvailable
            if (NoteDetail == null)
            {
                return Content("This Note Is UnAvailable...");
            }

            
            NoteDetailsModel note = new NoteDetailsModel();
            
            
            //If Buyer already Bought this Note the disable Download Button
            if (buyer != null)
            {
                //if User is Admin then Review Delete Button Show
                if (buyer.RoleID != 1)
                {
                    note.DisableBtn = false;
                    note.isAdmin = true;
                }
                else
                {
                    var disable = db.Downloads.Where(x => x.Downloader == buyer.ID && x.Seller == NoteDetail.SellerID && x.NoteID == NoteDetail.ID).FirstOrDefault();
                    if (disable != null)
                    {
                        if (disable.IsPaid)
                        {
                            note.DisableBtn = true;
                        }
                    }
                }
            }
            note.sellerNote = NoteDetail;
            note.sellerNotesAttachement = NoteFileDetail;
            note.Spam = spam != 0 ? spam : 0;
            note.AvgRating = (int)rating;
            note.TotalReview = totalreview;
            note.notesreview = reviews;


            

            return View(note);
        }





        [Authorize]
        public ActionResult DownloadNote(int noteId)
        {
            //Auth User as Buyer
            var buyer = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            //If Buyer Didnot Enter UserProfile the Redirect First on MyProfile Page
            var  Profile = db.UserProfiles.Where(x=>x.UserID == buyer.ID).FirstOrDefault();
            if(Profile == null)
            {
                return RedirectToAction("Index", "MyProfile");
            }


            //Note which User want to download
            var note = db.SellerNotes.Find(noteId);
            //Note File Attachment
            var fileAttachNote = db.SellerNotesAttachements.Where(x => x.NoteID == note.ID).ToList();
            //Note Seller Info
            var seller = db.Users.Where(x => x.ID == note.SellerID).FirstOrDefault();


            if (buyer.RoleID != 1)
            {
                string Apath = Server.MapPath("~/Members/" + seller.ID + "/" + note.ID + "/Attachements/");
                DirectoryInfo Adir = new DirectoryInfo(Apath);
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in Adir.GetFiles())
                        {
                            string filepath = Apath + item.ToString();
                            ziparchive.CreateEntryFromFile(filepath, item.ToString());
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", note.Title + ".zip");
                }
            }


            //if User download First Time free Note
            var firstTime = db.Downloads.Where(x => x.NoteID == note.ID && x.Seller == seller.ID && x.Downloader == buyer.ID && x.IsSellerHasAllowedDownload == true).FirstOrDefault();


            //if Note is Paid then Store Info in Download Table
            Download download = new Download
            {
                NoteID = note.ID,
                Seller = note.SellerID,
                Downloader = buyer.ID,
                IsSellerHasAllowedDownload = note.IsPaid ? false : true,
                AttachmentPath = note.IsPaid ? null : fileAttachNote[0].FilePath,
                IsAttachmentDownloaded = note.IsPaid ? false : true,
                AttachmentDownloadedDate = note.IsPaid ? (DateTime?)null : DateTime.Now,
                IsPaid = note.IsPaid,
                PurchasedPrice = note.SellingPrice,
                NoteTitle = note.Title,
                NoteCategory = note.NoteCategory.Name,
                CreatedDate = DateTime.Now,
                CreatedBy = buyer.ID,
                ModifiedDate = DateTime.Now,
                ModifiedBy = buyer.ID
            };


            //Downlaod Note if User Note is Free and User Is not First Time Downlaod
            if (firstTime != null && firstTime.IsSellerHasAllowedDownload == true)
            {
                firstTime.ModifiedDate = DateTime.Now;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                string fpath = Server.MapPath("~/Members/" + seller.ID + "/" + note.ID + "/Attachements/");
                DirectoryInfo fdir = new DirectoryInfo(fpath);
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in fdir.GetFiles())
                        {
                            string filepath = fpath + item.ToString();
                            ziparchive.CreateEntryFromFile(filepath, item.ToString());
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", note.Title +".zip");
                }
            }
            db.Downloads.Add(download);
            db.SaveChanges();


            //If Note is Paid then Send Mail To Seller 
            if (note.IsPaid)
            {
                var body = "<p>Hello, {0}</p><br>" +
                "<p>We would like to inform you that, {1} wants to purchase your notes. Please see Buyer Requests tab and allow download access to Buyer if you have received the payment from him.</p><br><br><br>" +
                "<p>Regards,</p>" +
                "<p>NoteMarketPlace</p>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(seller.Email));  // Reciever 
                message.From = new MailAddress("170200107021@gecrajkot.ac.in");  // Sender
                message.Subject = buyer.FirstName + " " + buyer.LastName + " wants to purchase your notes";
                message.Body = string.Format(body, seller.FirstName + " " + seller.LastName, buyer.FirstName + " " + buyer.LastName);
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
                TempData["Requested"] = "Requested";
                return RedirectToAction("NoteDetail", "SearchNote", new { id = noteId });
            }

            //Download Note iF its Free and FirstTime Downlaod Note
            string path = Server.MapPath("~/Members/" + seller.ID + "/" + note.ID + "/Attachements/");
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



        public ActionResult DeleteReview(int id)
        {
            //Auth User as Buyer
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if(user.RoleID == 1)
            {
                return Content("You'r not Allowed to Delete this Review of this Book");
            }

            var Review = db.SellerNotesReviews.Where(x => x.ID == id && x.IsActive == true).FirstOrDefault();
            var Note = db.SellerNotes.Where(x => x.ID == Review.NoteID).FirstOrDefault();

            if (Review == null)
            {
                return Content("This Review is not Available....");
            }
            db.SellerNotesReviews.Remove(Review);
            db.SaveChanges();

            return RedirectToAction("NoteDetail", "SearchNote", new { id = Note.ID});
        }

    }
}
