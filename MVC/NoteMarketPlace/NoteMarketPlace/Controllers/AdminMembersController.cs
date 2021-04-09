using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminMembersController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public AdminMembersController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: AdminMembers
        [Route("AdminMembers")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index(string search, string sortOrder, int amPage = 1)
        {
            ViewBag.Members = "active";

            ViewBag.FNameSortParm = sortOrder == "FName" ? "FName_desc" : "FName";
            ViewBag.LNameSortParm = sortOrder == "LName" ? "LName_desc" : "LName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.NURSortParm = sortOrder == "NUR" ? "NUR_desc" : "NUR";
            ViewBag.PnSortParm = sortOrder == "Pn" ? "Pn_desc" : "Pn";
            ViewBag.DnSortParm = sortOrder == "Dn" ? "Dn_desc" : "Dn";
            ViewBag.TExpanseSortParm = sortOrder == "TExpanse" ? "TExpanse_desc" : "TExpanse";
            ViewBag.TEarnSortParm = sortOrder == "TEarn" ? "TEarn_desc" : "TEarn";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            

            var Users = db.Users.Where(x => x.IsActive == true && x.RoleID == 1).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                Users = Users.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search) || x.Email.Contains(search) || x.CreatedDate.ToString().Contains(search)).ToList();
            }


            List<AdminMemberModel> list = new List<AdminMemberModel>();
            foreach(var item in Users)
            {
                int tex, ter;
                int nur = db.SellerNotes.Where(x => x.IsActive == true && x.SellerID == item.ID && (x.Status == 2 || x.Status == 3)).Count();
                int pn = db.SellerNotes.Where(x => x.IsActive == true && x.SellerID == item.ID && x.Status == 4).Count();
                int dn = db.Downloads.Where(x => x.Downloader == item.ID && x.IsSellerHasAllowedDownload == true).Count();
                var texList = db.Downloads.Where(x => x.Downloader == item.ID && x.IsSellerHasAllowedDownload == true).ToList();
                if (texList == null)
                {
                    tex = 0;
                }
                else
                {
                    tex = (int)texList.Sum(x=>x.PurchasedPrice);
                }
                var terList = db.Downloads.Where(x => x.Seller == item.ID && x.IsSellerHasAllowedDownload == true).ToList();
                if (terList == null)
                {
                    ter = 0;
                }
                else
                {
                    ter = (int)terList.Sum(x => x.PurchasedPrice);
                }
                AdminMemberModel adminMember = new AdminMemberModel()
                {
                    User = item,

                    NoteUnderReview = nur,
                    PublishedNote = pn,
                    DownloadedNote = dn,
                    TotalExpanses = tex,
                    TotalEarning = ter
                };
                list.Add(adminMember);
            }

            switch (sortOrder)
            {
                case "FName_desc":
                    list = list.OrderByDescending(s => s.User.FirstName).ToList();
                    break;
                case "FName":
                    list = list.OrderBy(s => s.User.FirstName).ToList();
                    break;
                case "LName_desc":
                    list = list.OrderByDescending(s => s.User.LastName).ToList();
                    break;
                case "LName":
                    list = list.OrderBy(s => s.User.LastName).ToList();
                    break;
                case "Email_desc":
                    list = list.OrderByDescending(s => s.User.Email).ToList();
                    break;
                case "Email":
                    list = list.OrderBy(s => s.User.Email).ToList();
                    break;
                case "Date_desc":
                    list = list.OrderByDescending(s => s.User.CreatedDate).ToList();
                    break;
                case "Date":
                    list = list.OrderBy(s => s.User.Email).ToList();
                    break;
                case "NUR_desc":
                    list = list.OrderByDescending(s => s.NoteUnderReview).ToList();
                    break;
                case "NUR":
                    list = list.OrderBy(s => s.NoteUnderReview).ToList();
                    break;
                case "Pn_desc":
                    list = list.OrderByDescending(s => s.PublishedNote).ToList();
                    break;
                case "Pn":
                    list = list.OrderBy(s => s.PublishedNote).ToList();
                    break;
                case "Dn_desc":
                    list = list.OrderByDescending(s => s.DownloadedNote).ToList();
                    break;
                case "Dn":
                    list = list.OrderBy(s => s.DownloadedNote).ToList();
                    break;
                case "TExpanse_desc":
                    list = list.OrderByDescending(s => s.TotalExpanses).ToList();
                    break;
                case "TExpanse":
                    list = list.OrderBy(s => s.TotalExpanses).ToList();
                    break;
                case "TEarn_desc":
                    list = list.OrderByDescending(s => s.TotalEarning).ToList();
                    break;
                case "TEarn":
                    list = list.OrderBy(s => s.TotalEarning).ToList();
                    break;
                default:
                    list = list.OrderByDescending(s => s.User.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(list.Count(), amPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = amPage;

            ViewBag.srno = amPage;
            ViewBag.TotalMemberPage = Math.Ceiling(list.Count() / 5.0);
            list = list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();


            return View(list);
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult DeActive(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();


            var Users = db.Users.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();

            Users.IsActive = false;
            Users.ModifiedBy = user.ID;
            Users.ModifiedDate = DateTime.Now;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.SellerID == Users.ID && x.Status == 4).ToList();

            foreach(var item in Notes)
            {
                item.Status = 6;
                Users.ModifiedBy = user.ID;
                Users.ModifiedDate = DateTime.Now;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "AdminMembers");
        }





        [Route("AdminMembers/MemberDetail/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult MemberDetail(int id, string sortOrder, int mdPage = 1)
        {
            ViewBag.Members = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
           

            var Users = db.Users.Where(x => x.IsActive == true && x.ID == id).FirstOrDefault();
            var UsersProfile = db.UserProfiles.Where(x => x.UserID == Users.ID).FirstOrDefault();
            int c = Convert.ToInt32(UsersProfile.Country);
            var Country = db.Countries.Where(x=>x.ID == c  && x.IsActive == true).FirstOrDefault();

            var Notes = db.SellerNotes.Where(x => x.IsActive == true && x.SellerID == Users.ID && (x.Status != 1)).ToList();

            List<MemberNotes> list = new List<MemberNotes>();
            foreach(var item in Notes)
            {
                int dn = db.Downloads.Where(x => x.NoteID == item.ID && x.IsSellerHasAllowedDownload == true && x.Seller == Users.ID).Count();
                int te;
                var teList = db.Downloads.Where(x => x.NoteID == item.ID && x.IsSellerHasAllowedDownload == true && x.Seller == Users.ID).ToList();
                if(teList == null)
                {
                    te = 0;
                }
                else
                {
                    te = (int)teList.Sum(x => x.PurchasedPrice);
                }

                MemberNotes members = new MemberNotes()
                {
                    SellerNote = item,
                    DownloadedNotes = dn,
                    TotalEarning = te
                };
                list.Add(members);
            }

            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.DnSortParm = sortOrder == "Dn" ? "Dn_desc" : "Dn";
            ViewBag.TeSortParm = sortOrder == "Te" ? "Te_desc" : "Te";
            ViewBag.ADateSortParm = sortOrder == "ADate" ? "ADate_desc" : "ADate";
            ViewBag.PDateSortParm = sortOrder == "PDate" ? "PDate_desc" : "pDate";


            switch (sortOrder)
            {
                case "Title_desc":
                    list = list.OrderByDescending(s => s.SellerNote.Title).ToList();
                    break;
                case "Title":
                    list = list.OrderBy(s => s.SellerNote.Title).ToList();
                    break;
                case "Category_desc":
                    list = list.OrderByDescending(s => s.SellerNote.NoteCategory.Name).ToList();
                    break;
                case "Category":
                    list = list.OrderBy(s => s.SellerNote.NoteCategory.Name).ToList();
                    break;
                case "Status_desc":
                    list = list.OrderByDescending(s => s.SellerNote.ReferenceData.Value).ToList();
                    break;
                case "Status":
                    list = list.OrderBy(s => s.SellerNote.ReferenceData.Value).ToList();
                    break;
                case "Dn_desc":
                    list = list.OrderByDescending(s => s.DownloadedNotes).ToList();
                    break;
                case "Dn":
                    list = list.OrderBy(s => s.DownloadedNotes).ToList();
                    break;
                case "Te_desc":
                    list = list.OrderByDescending(s => s.TotalEarning).ToList();
                    break;
                case "Te":
                    list = list.OrderBy(s => s.TotalEarning).ToList();
                    break;
                case "ADate_desc":
                    list = list.OrderByDescending(s => s.SellerNote.CreatedDate).ToList();
                    break;
                case "ADate":
                    list = list.OrderBy(s => s.SellerNote.CreatedDate).ToList();
                    break;
                case "PDate_desc":
                    list = list.OrderByDescending(s => s.SellerNote.PublishedDate).ToList();
                    break;
                case "PDate":
                    list = list.OrderBy(s => s.SellerNote.PublishedDate).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.SellerNote.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(list.Count(), mdPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = mdPage;

            ViewBag.srno = mdPage;
            ViewBag.TotalMemberDetailPage = Math.Ceiling(list.Count() / 5.0);
            list = list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();


            MemberDetailModel member = new MemberDetailModel();
            member.User = Users;
            member.UserProfile = UsersProfile;
            member.Notes = list;
            member.CountryName = Country == null ? "" :Country.Name;


            return View(member);
        }

    }
}