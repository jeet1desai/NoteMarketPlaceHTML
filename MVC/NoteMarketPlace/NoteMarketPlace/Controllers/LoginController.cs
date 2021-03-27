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
using System.Web.Hosting;
using System.Net.Mail;
using System.Text;
using System.Net;

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
                    ModelState.AddModelError("Email", "Please verify Email");
                    //Build Email Template
                    BuildEmailVerifyTemplate(isUser.ID);
                    return View(user);
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
                        var UserProfile = db.UserProfiles.Where(x => x.UserID == isUser.ID).FirstOrDefault();
                        
                        if(UserProfile == null)
                        {
                            return RedirectToAction("Index", "MyProfile");
                        }

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


        [Route("Login/VerifyEmail/{regID}")]
        public ActionResult VerifyEmail(int regID)
        {
            ViewBag.regID = regID;
            return View();
        }

        public void BuildEmailVerifyTemplate(int RegID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailVarification/") + "EmailVerifyTemp" + ".cshtml");
            var regInfo = db.Users.FirstOrDefault(x => x.ID == RegID);
            var url = "https://localhost:44312/" + "Login/VerifyEmail?regID=" + RegID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.Replace("@ViewBag.FirstName", regInfo.FirstName);
            body = body.ToString();
            BuildEmailVerifyTemplate(body, regInfo.Email);
        }

        public static void BuildEmailVerifyTemplate(string bodyText, string sendTo)
        {

            var message = new MailMessage();
            message.To.Add(new MailAddress(sendTo.Trim()));  // Reciever 
            message.From = new MailAddress("******@******.ac.in");  // Sender
            message.Subject = "Note Marketplace - Email Verification";

            string body;

            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();

            message.Body = string.Format(body);
            message.IsBodyHtml = true;
            SendEmail(message);
        }

        //Send Mail
        public static void SendEmail(MailMessage mail)
        {
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "******@******.ac.in",
                    Password = "******"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        [Route("Login/EmailConfirm/{regID}")]
        public ActionResult EmailConfirm(int regID)
        {
            User user = db.Users.FirstOrDefault(x => x.ID == regID);
            Session["ID"] = user.ID;
            Session["Email"] = user.Email;
            FormsAuthentication.SetAuthCookie(user.Email, true);
            user.IsEmailVerified = true;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Dispose();

            if (user.RoleID == 1)
            {
                return RedirectToAction("Index", "MyProfile");
            }
            else
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
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

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return RedirectToAction("Index", "Login");
        }

    }
}