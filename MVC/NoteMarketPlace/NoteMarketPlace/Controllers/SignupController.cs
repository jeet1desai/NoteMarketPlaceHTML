using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;
using System.Web.Hosting;
using System.Diagnostics;
using System.Net;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{
    public class SignupController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public SignupController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: Signup
        [Route("Signup")]
        [HttpGet]
        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [Route("Signup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "FirstName,LastName,Email,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                
                Match isPassword = Regex.Match(user.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,24}", RegexOptions.IgnorePatternWhitespace);
                Match isFirstName = Regex.Match(user.FirstName, @"^[a-zA-Z]+$", RegexOptions.IgnorePatternWhitespace);
                Match isLastName = Regex.Match(user.LastName, @"^[a-zA-Z]+$", RegexOptions.IgnorePatternWhitespace);


                //Email already Exist
                if (db.Users.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "This Email Already Exist");
                    return View(user);
                }
                
                //Password and ConfirmPassword doesn't Match
                if(!(user.Password.Equals(user.ConfirmPassword)))
                {
                    ModelState.AddModelError("ConfirmPassword", "Password and ConfirmPassword are not Same");
                    return View(user);
                }
                
                //Password is not Valid
                if (!isPassword.Success)
                {
                    ModelState.AddModelError("Password", "Password shold contain 6 long, 1 - special, digit, upper, lower char");
                    return View(user);
                }

                //FirstName And LastName is Not valid
                if (!isFirstName.Success)
                {
                    ModelState.AddModelError("FirstName", "Use Only Alphabet");
                    return View(user);
                }
                if (!isLastName.Success)
                {
                    ModelState.AddModelError("LastName", "Use Only Alphabet");
                    return View(user);
                }

                user.RoleID = 1;
                user.IsActive = true;
                user.CreatedDate = DateTime.Now;
                user.IsEmailVerified = false;
                db.Users.Add(user);
                db.SaveChanges();
                
                //Build Email Template
                BuildEmailVerifyTemplate(user.ID);

                //Save Cookie
                Session["ID"] = user.ID;
                Session["Email"] = user.Email;

                ViewBag.Status = 1;

                ModelState.Clear();

                db.Dispose();
                return View();
            }
            else
            {
                //If Fail to Signup
                ViewBag.Status = 1;
                ViewBag.Class = "danger";
                ViewBag.Msg = "Fail to Signup";
                return View(user); 
            }


        }
        

        [Route("Signup/VerifyEmail/{regID}")]
        public ActionResult VerifyEmail(int regID)
        {
            ViewBag.regID = regID;
            return View();
        }


        [Route("Signup/RegisterConfirm/{regID}")]
        public ActionResult RegisterConfirm(int regID)
        {
            Session["ID"] = regID;
            User user = db.Users.FirstOrDefault(x => x.ID == regID);
            FormsAuthentication.SetAuthCookie(user.Email, true);
            user.IsEmailVerified = true;
            user.CreatedBy = user.ID;
            user.ModifiedBy = user.ID;
            user.ModifiedDate = DateTime.Now;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Dispose();

            if(user.RoleID == 1)
            {
                return RedirectToAction("Index", "MyProfile");
            }
            else
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
        }

        
        public void BuildEmailVerifyTemplate(int RegID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailVarification/") + "EmailVerifyTemp" + ".cshtml");
            var regInfo = db.Users.FirstOrDefault(x => x.ID == RegID);
            var url = "https://localhost:44312/" + "Signup/VerifyEmail?regID=" + RegID;
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
    }
}