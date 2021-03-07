using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Globalization;


namespace NoteMarketPlace.Controllers
{
    public class LoginController : Controller
    {
        readonly NoteMarketPlaceEntities db;

        public LoginController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: Login
        [Route("Login")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginModel user, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //Email Not Regitered
                if (!db.Users.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This Email Not Registered");
                    return View(user);
                }

                //Wrong Password 
                var isUser = db.Users.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (isUser == null)
                {
                    ModelState.AddModelError("Password", "Wrong Passsword");
                    return View(user);
                }

                //Account is Not Active
                if (!isUser.IsActive)
                {
                    return RedirectToAction("Index");
                }

                //Email not Varified
                if (!isUser.IsEmailVerified)
                {
                    return RedirectToAction("VerifyEmail", "Signup", new { regID = isUser.ID});
                }

                if (isUser != null)
                {
                    //Remember me check box
                    FormsAuthentication.SetAuthCookie(user.Email, user.IsSelected);

                    Session["ID"] = isUser.ID;
                    Session["Email"] = isUser.Email;


                    //For Member
                    if (isUser.RoleID == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    //For Admin and SuperAdmin
                    else
                    {
                        return RedirectToAction("Index", "AdminDashboard");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }



        //Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            Session["ID"] = null;
            Session["Email"] = null;
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }

    }
}