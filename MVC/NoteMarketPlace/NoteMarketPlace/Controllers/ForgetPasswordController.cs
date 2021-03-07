using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{
    public class ForgetPasswordController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public ForgetPasswordController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: ForgetPassword
        [Route("ForgetPassword")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("ForgetPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            var isUser = db.Users.Where(a => a.Email == user.Email).FirstOrDefault();

            if (!db.Users.Any(x => x.Email == user.Email))
            {
                ViewBag.Status = 1;
                return View(user);
            }

            //Generate 8 character Random Password
            string password = Membership.GeneratePassword(8, 0);

            if(isUser != null)
            {
                //Save Password in Database
                isUser.Password = password;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }

            //Send Mail with Password to User
            var body = "<p>Hello,</p><br>" +
                "<p>We have generated a new password for you</p>" +
                "<p>Password : {0}</p><br><br><br><br><br>" +
                "<p>Regards,</p>" +
                "<p>Notes Marketplace</p>";

            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));  // Reciever 
            message.From = new MailAddress("******@******.ac.in");  // Sender
            message.Subject = " New Temporary Password has been created for you";
            message.Body = string.Format(body, password);
            message.IsBodyHtml = true;

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
                smtp.Send(message);

                return RedirectToAction("Index", "Login");
            }
        }
    }
}