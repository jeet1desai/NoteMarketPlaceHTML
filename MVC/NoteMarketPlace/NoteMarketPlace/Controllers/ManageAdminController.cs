using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ManageAdminController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public ManageAdminController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: ManageAdmin
        [Route("ManageAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public ActionResult Index(string search, string sortOrder, int maPage = 1)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            ViewBag.Setting = "active";
            ViewBag.MAdmin = "active";

            ViewBag.FNameSortParm = sortOrder == "FName" ? "FName_desc" : "FName";
            ViewBag.LNameSortParm = sortOrder == "LName" ? "LName_desc" : "LName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Active_desc" : "Active";

            var Admin = db.Users.Where(x => x.RoleID == 2).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                Admin = Admin.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search) ||
                                    x.Email.Contains(search) || x.CreatedDate.ToString().Contains(search) ||
                                    db.UserProfiles.Where(s => s.UserID == x.ID).FirstOrDefault().PhoneNumber.Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "FName_desc":
                    Admin = Admin.OrderByDescending(s => s.FirstName).ToList();
                    break;
                case "FName":
                    Admin = Admin.OrderBy(s => s.FirstName).ToList();
                    break;
                case "LName_desc":
                    Admin = Admin.OrderByDescending(s => s.LastName).ToList();
                    break;
                case "LName":
                    Admin = Admin.OrderBy(s => s.LastName).ToList();
                    break;
                case "Email_desc":
                    Admin = Admin.OrderByDescending(s => s.Email).ToList();
                    break;
                case "Email":
                    Admin = Admin.OrderBy(s => s.Email).ToList();
                    break;
                case "Phone_desc":
                    Admin = Admin.OrderByDescending(x => (db.UserProfiles.Where(s => s.UserID == x.ID).FirstOrDefault().PhoneNumber)).ToList();
                    break;
                case "Phone":
                    Admin = Admin.OrderBy(x => (db.UserProfiles.Where(s => s.UserID == x.ID).FirstOrDefault().PhoneNumber)).ToList();
                    break;
                case "Active_desc":
                    Admin = Admin.OrderByDescending(s => s.IsActive).ToList();
                    break;
                case "Active":
                    Admin = Admin.OrderBy(s => s.IsActive).ToList();
                    break;
                case "Date_desc":
                    Admin = Admin.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
                case "Date":
                    Admin = Admin.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    Admin = Admin.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(Admin.Count(), maPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = maPage;

            ViewBag.srno = maPage;
            ViewBag.TotalAdminPage = Math.Ceiling(Admin.Count() / 5.0);
            Admin = Admin.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Admin);
        }




        [Route("ManageAdmin/AddAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public ActionResult AddAdmin()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
           
            ViewBag.Setting = "active";
            ViewBag.MAdmin = "active";

            ManageAdminModel Admin = new ManageAdminModel();
            Admin.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();

            return View(Admin);
        }


        [Route("ManageAdmin/AddAdmin")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(ManageAdminModel admin)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            ViewBag.Setting = "active";
            ViewBag.MAdmin = "active";

            var CheckEmail = db.Users.Where(x => x.Email == admin.Email).FirstOrDefault();
            if (CheckEmail != null)
            {
                ModelState.AddModelError("Email", "This Email Already Exist");
                ManageAdminModel AdminModel = new ManageAdminModel();
                AdminModel.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();
                return View(AdminModel);
            }

            if (ModelState.IsValid)
            {
                User newAdmin = new User();
                newAdmin.RoleID = 2;
                newAdmin.FirstName = admin.FirstName;
                newAdmin.LastName = admin.LastName;
                newAdmin.Email = admin.Email;
                newAdmin.Password = "Admin@123";
                newAdmin.IsEmailVerified = true;
                newAdmin.CreatedDate = DateTime.Now;
                newAdmin.CreatedBy = user.ID;
                newAdmin.ModifiedDate = DateTime.Now;
                newAdmin.ModifiedBy = user.ID;
                newAdmin.IsActive = true;
                db.Users.Add(newAdmin);
                db.SaveChanges();

                var UserIdProfile = db.Users.Where(x => x.RoleID == 2 && x.Email == admin.Email).FirstOrDefault();

                UserProfile newAdminProfile = new UserProfile();
                newAdminProfile.UserID = UserIdProfile.ID;
                newAdminProfile.PhoneNumberCountryCode = admin.CountryPhoneCode;
                newAdminProfile.PhoneNumber = admin.Phone;

                var filepath = db.SystemConfigurations.Where(x => x.Name == "DefaultMemberDisplayPicture").FirstOrDefault();
                newAdminProfile.ProfilePicture = filepath.Value;

                newAdminProfile.AddressLine1 = "Null";
                newAdminProfile.AddressLine2 = "Null";
                newAdminProfile.City = "Null";
                newAdminProfile.State = "Null";
                newAdminProfile.ZipCode = "Null";
                newAdminProfile.Country = "Null";
                newAdminProfile.CreatedDate = DateTime.Now;
                newAdminProfile.CreatedBy = user.ID;
                newAdminProfile.ModifiedDate = DateTime.Now;
                newAdminProfile.ModifiedBy = user.ID;
                
                db.UserProfiles.Add(newAdminProfile);
                db.SaveChanges();

                return RedirectToAction("Index", "ManageAdmin");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                ManageAdminModel Admin = new ManageAdminModel();
                Admin.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();
                return View(Admin);
            }


        }



        [Route("ManageAdmin/EditAdmin/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            ViewBag.Setting = "active";
            ViewBag.MAdmin = "active";

            var UserDetail = db.Users.Where(x => x.RoleID == 2 && x.ID == id).FirstOrDefault();

            ManageAdminModel admin = new ManageAdminModel();
            admin.Email = UserDetail.Email;
            admin.FirstName = UserDetail.FirstName;
            admin.LastName = UserDetail.LastName;
            admin.CountryPhoneCode = UserDetail.UserProfiles.Where(x => x.UserID == UserDetail.ID).FirstOrDefault().PhoneNumberCountryCode;
            admin.Phone = UserDetail.UserProfiles.Where(x => x.UserID == UserDetail.ID).FirstOrDefault().PhoneNumber;

            admin.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();
            
            return View(admin);
        }



        [Route("ManageAdmin/EditAdmin/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(ManageAdminModel editAdmin)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            ViewBag.Setting = "active";
            ViewBag.MAdmin = "active";
            

            var Users = db.Users.Where(x => x.ID == editAdmin.ID && x.RoleID == 2).FirstOrDefault();
            var UserProfile = db.UserProfiles.Where(x => x.UserID == Users.ID).FirstOrDefault();

            var check = db.Users.Where(x => x.ID != Users.ID && x.Email == editAdmin.Email).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError("Email", "This Email Already Exist");
                ManageAdminModel AdminModel = new ManageAdminModel();
                AdminModel.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();
                return View(AdminModel);
            }

            if (ModelState.IsValid)
            {
                Users.FirstName = editAdmin.FirstName;
                Users.LastName = editAdmin.LastName;
                Users.Email = editAdmin.Email;
                Users.ModifiedDate = DateTime.Now;
                Users.ModifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                UserProfile.PhoneNumberCountryCode = editAdmin.CountryPhoneCode;
                UserProfile.PhoneNumber = editAdmin.Phone;
                UserProfile.ModifiedDate = DateTime.Now;
                UserProfile.ModifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return RedirectToAction("Index", "ManageAdmin");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                ManageAdminModel AdminModel = new ManageAdminModel();
                AdminModel.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();
                return View(AdminModel);
            }
            
        }


        [Authorize(Roles = "SuperAdmin")]
        public ActionResult InActiveAdmin(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            var Users = db.Users.Where(x=> x.ID == id).FirstOrDefault();

            Users.IsActive = false;
            Users.ModifiedDate = DateTime.Now;
            Users.ModifiedBy = user.ID;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Index", "ManageAdmin");
        }
    }
}