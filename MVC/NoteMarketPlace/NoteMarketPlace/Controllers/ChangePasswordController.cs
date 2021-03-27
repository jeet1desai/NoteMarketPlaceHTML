using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{
    public class ChangePasswordController : Controller
    {


        readonly NoteMarketPlaceEntities db;

        public ChangePasswordController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: ChangePassword
        [Route("ChangePassword")]
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Route("ChangePassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Index(ChangePasswordModel changePassword)
        {

            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            //Enter Password and Db Password Not Match
            if (user.Password != changePassword.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Enter Valid Old Password");
                return View(changePassword);
            }

            //New Pass and Confirm Pass not Match
            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                ModelState.AddModelError("confirmpassword", "Password and ConfirmPassword are not Same");
                return View(changePassword);
            }

            if (user.IsActive == true && ModelState.IsValid)
            {
                user.Password = changePassword.NewPassword;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                Session["ID"] = null;
                Session["Email"] = null;
                Session.RemoveAll();

                db.Dispose();
                return RedirectToAction("Index", "Login");
            }

            return View(changePassword);
        }

    }
}