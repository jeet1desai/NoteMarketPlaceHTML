using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminUpdateProfileController : Controller
    {
        
        readonly private NoteMarketPlaceEntities db;

        public AdminUpdateProfileController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: AdminUpdateProfile
        [Route("AdminUpdateProfile")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index()
        {
            ViewBag.Profile = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var userProfile = db.UserProfiles.FirstOrDefault(x => x.UserID == user.ID);

            AdminProfileModel admin = new AdminProfileModel();

            admin.FirstName = user.FirstName;
            admin.LastName = user.LastName;
            admin.Email = user.Email;


            admin.PhoneCode = userProfile.PhoneNumberCountryCode;
            admin.Phone = userProfile.PhoneNumber;

            if (userProfile.SecondaryEmailAddress != null)
            {
                admin.SecondEmail = userProfile.SecondaryEmailAddress;
            }

            admin.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();

            return View(admin);
        }

        [Route("AdminUpdateProfile")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdminProfileModel admin)
        {
            ViewBag.Profile = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var userProfile = db.UserProfiles.FirstOrDefault(x => x.UserID == user.ID);

            if (ModelState.IsValid)
            {
                user.FirstName = admin.FirstName;
                user.LastName = admin.LastName;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                userProfile.PhoneNumberCountryCode = admin.PhoneCode;
                userProfile.PhoneNumber = admin.Phone;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                if(admin.SecondEmail != null)
                {
                    userProfile.SecondaryEmailAddress = admin.SecondEmail;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }


                if (admin.ProfilePicture != null)
                {
                    string userprofilename = System.IO.Path.GetFileName(admin.ProfilePicture.FileName);
                    string userprofileext = System.IO.Path.GetExtension(admin.ProfilePicture.FileName);

                    string storeprofilepath = "~/Members/" + user.ID + "/";

                    CreateDirectoryIfMissing(storeprofilepath);

                    userprofilename = "DP_" + DateTime.Now.ToString("ddMMyyyy") + userprofileext;

                    string userprofilepath = Path.Combine(Server.MapPath(storeprofilepath), userprofilename);
                    userProfile.ProfilePicture = storeprofilepath + userprofilename;

                    admin.ProfilePicture.SaveAs(userprofilepath);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "AdminDashboard");

            }
            else
            {
                AdminProfileModel admin1 = new AdminProfileModel();
                admin1.CountryList = db.Countries.Where(x => x.IsActive == true).ToList();

                return View(admin1);
            }
        }


        //folder structure create if missing
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