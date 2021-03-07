using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ContactusController : Controller
    {
        // GET: Contactus
        [Route("Contactus")]
        public ActionResult Index()
        {
            ViewBag.ContactUs = "active";
            ViewBag.Class = "white-nav";
            return View();
        }

        [Route("Contactus")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(ContactusModel contact)
        {
            var body = "<p>Hello,</p><br>" +
                "<p>{0}</p><br><br><br>" +
                "<p>Regards,</p>" +
                "<p>{1}</p>";

            var message = new MailMessage();
            message.To.Add(new MailAddress("******@gmail.com"));  // Reciever 
            message.From = new MailAddress("******@******.ac.in");  // Sender
            message.Subject = contact.FullName + "- Query";
            message.Body = string.Format(body, contact.Comments, contact.FullName);
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

                
            }
            return RedirectToAction("Index", "Contactus");
        }
    }
}