using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class MyProfileController : Controller
    {

        readonly NoteMarketPlaceEntities db;

        public MyProfileController()
        {
            db = new NoteMarketPlaceEntities();
        }


        [Route("MyProfile")]
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.MyProfile = "active";
            ViewBag.Class = "white-nav";

            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Check in UserProfile Table if User Already Enter Detail
            var CheckUserProfile = db.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();

            var countryList = db.Countries.Where(x => x.IsActive == true).ToList();
            var genderList = db.ReferenceDatas.Where(x => x.IsActive == true && x.RefCategory == "Gender").ToList();

            MyProfileModel myProfileModel = new MyProfileModel();


            //if User Came on this Page Second Time then AutoPopulate Info
            myProfileModel.CountryList = countryList;
            myProfileModel.GenderList = genderList;

            myProfileModel.FirstName = user.FirstName;
            myProfileModel.LastName = user.LastName;
            myProfileModel.Email = user.Email;

            if (CheckUserProfile != null)
            {
                myProfileModel.DOB = CheckUserProfile.DOB;
                myProfileModel.Gender = CheckUserProfile.Gender;
                myProfileModel.CountryPhoneCode = CheckUserProfile.PhoneNumberCountryCode;
                myProfileModel.PhoneNumber = CheckUserProfile.PhoneNumber;
                myProfileModel.AddressOne = CheckUserProfile.AddressLine1;
                myProfileModel.AddressTwo = CheckUserProfile.AddressLine2;
                myProfileModel.City = CheckUserProfile.City;
                myProfileModel.ZipCode = CheckUserProfile.ZipCode;
                myProfileModel.Country = CheckUserProfile.Country;
                myProfileModel.State = CheckUserProfile.State;
                myProfileModel.University = CheckUserProfile.University;
                myProfileModel.College = CheckUserProfile.College;
                myProfileModel.PpPath = CheckUserProfile.ProfilePicture;
                ViewBag.ProfilePicture = Path.GetFileName(CheckUserProfile.ProfilePicture);
            }
            return View(myProfileModel);
        }


        [Route("MyProfile")]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MyProfileModel profile)
        {
            //Auth User
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Check if user already Enter Profile Detail or Not
            var CheckUserProfile = db.UserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
            if (user != null && ModelState.IsValid)
            {
                //if User Came on this Page First Time and want To Update Profile Info
                if (CheckUserProfile == null)
                {
                    UserProfile userProfile = new UserProfile();

                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    user.ModifiedDate = DateTime.Now;
                    user.ModifiedBy = user.ID;

                    db.Configuration.ValidateOnSaveEnabled = false;


                    userProfile.UserID = user.ID;
                    userProfile.DOB = profile.DOB;
                    userProfile.Gender = profile.Gender;
                    userProfile.PhoneNumberCountryCode = profile.CountryPhoneCode;
                    userProfile.PhoneNumber = profile.PhoneNumber;
                    userProfile.AddressLine1 = profile.AddressOne;
                    userProfile.AddressLine2 = profile.AddressTwo;
                    userProfile.City = profile.City;
                    userProfile.State = profile.State;
                    userProfile.ZipCode = profile.ZipCode;
                    userProfile.Country = profile.Country;
                    userProfile.University = profile.University;
                    userProfile.College = profile.College;
                    userProfile.CreatedDate = DateTime.Now;
                    userProfile.CreatedBy = user.ID;
                    userProfile.ModifiedDate = DateTime.Now;
                    userProfile.ModifiedBy = user.ID;

                    if (profile.ProfilePicture != null)
                    {
                        string userprofilename = System.IO.Path.GetFileName(profile.ProfilePicture.FileName);
                        string userprofileext = System.IO.Path.GetExtension(profile.ProfilePicture.FileName);

                        string storeprofilepath = "~/Members/" + user.ID + "/";

                        userprofilename = "DP_" + DateTime.Now.ToString("ddMMyyyy") + userprofileext;

                        CreateDirectoryIfMissing(storeprofilepath);

                        string userprofilepath = Path.Combine(Server.MapPath(storeprofilepath), userprofilename);
                        userProfile.ProfilePicture = storeprofilepath + userprofilename;

                        profile.ProfilePicture.SaveAs(userprofilepath);

                    }
                    else
                    {
                        //Admin Config Defualt Pic Use 
                    }

                    db.UserProfiles.Add(userProfile);
                    db.SaveChanges();

                    return RedirectToAction("Index", "SearchNote");
                }
                //if User Came on this Page Second Time and want To Update Profile Info
                else
                {
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    user.ModifiedDate = DateTime.Now;
                    user.ModifiedBy = user.ID;

                    db.Configuration.ValidateOnSaveEnabled = false;

                    UserProfile ExistUser = db.UserProfiles.FirstOrDefault(x => x.UserID == user.ID);

                    ExistUser.DOB = profile.DOB;
                    ExistUser.Gender = profile.Gender;
                    ExistUser.PhoneNumberCountryCode = profile.CountryPhoneCode;
                    ExistUser.PhoneNumber = profile.PhoneNumber;
                    ExistUser.AddressLine1 = profile.AddressOne;
                    ExistUser.AddressLine2 = profile.AddressTwo;
                    ExistUser.City = profile.City;
                    ExistUser.State = profile.State;
                    ExistUser.ZipCode = profile.ZipCode;
                    ExistUser.Country = profile.Country;
                    ExistUser.University = profile.University;
                    ExistUser.College = profile.College;
                    ExistUser.ModifiedDate = DateTime.Now;
                    ExistUser.ModifiedBy = user.ID;

                    if (profile.ProfilePicture != null)
                    {
                        string PathImage = "~/Members/" + user.ID + "/" + Path.GetFileName(ExistUser.ProfilePicture);
                        FileInfo file = new FileInfo(PathImage);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        string userprofilename = System.IO.Path.GetFileName(profile.ProfilePicture.FileName);
                        string userprofileext = System.IO.Path.GetExtension(profile.ProfilePicture.FileName);

                        string storeprofilepath = "~/Members/" + user.ID + "/";

                        userprofilename = "DP_" + DateTime.Now.ToString("ddMMyyyy") + userprofileext;

                        string userprofilepath = Path.Combine(Server.MapPath(storeprofilepath), userprofilename);
                        ExistUser.ProfilePicture = storeprofilepath + userprofilename;

                        profile.ProfilePicture.SaveAs(userprofilepath);

                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    return RedirectToAction("Index", "SearchNote");
                }

            }
            var countryList = db.Countries.Where(x => x.IsActive == true).ToList();
            var genderList = db.ReferenceDatas.Where(x => x.IsActive == true && x.RefCategory == "Gender").ToList();

            MyProfileModel myProfileModel = new MyProfileModel();

            myProfileModel.CountryList = countryList;
            myProfileModel.GenderList = genderList;

            myProfileModel.FirstName = user.FirstName;
            myProfileModel.LastName = user.LastName;
            myProfileModel.Email = user.Email;

            return View(myProfileModel);

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