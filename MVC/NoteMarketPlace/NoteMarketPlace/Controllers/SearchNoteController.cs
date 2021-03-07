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

    public class SearchNoteController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public SearchNoteController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: SearchNote
        [Route("SearchNote")]
        public ActionResult Index()
        {
            ViewBag.Class = "white-nav";
            ViewBag.SN = "active";

            var note = db.SellerNotes.Where(x => x.Status == 4).ToList();

            return View(note);
        }





        //NoteDetail
        [Route("SearchNote/NoteDetail/{id}")]
        [AllowAnonymous]
        public ActionResult NoteDetail(int id)
        {
            ViewBag.Class = "white-nav";
            var buyer = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var NoteDetail = db.SellerNotes.Where(x => x.ID == id).FirstOrDefault();

            if (NoteDetail == null)
            {
                return Content("This Note Is UnAvailable...");
            }

            var NoteFileDetail = db.SellerNotesAttachements.Where(x => x.NoteID == NoteDetail.ID).FirstOrDefault();
            var disable = db.Downloads.Where(x => x.Downloader == buyer.ID && x.Seller == NoteDetail.SellerID && x.NoteID == NoteDetail.ID).FirstOrDefault();

            NoteDetailsModel note = new NoteDetailsModel();
            if(disable != null)
            {
                if (disable.IsPaid)
                {
                    note.DisableBtn = true;
                }
            }
            note.sellerNote = NoteDetail;
            note.sellerNotesAttachement = NoteFileDetail;




            return View(note);
        }


        public ActionResult DownloadNote(int noteId)
        {
            var buyer = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var note = db.SellerNotes.Find(noteId);
            var fileAttachNote = db.SellerNotesAttachements.Where(x => x.NoteID == note.ID).FirstOrDefault();

            var seller = db.Users.Where(x => x.ID == note.SellerID).FirstOrDefault();
           

            Download download = new Download
            {
                NoteID = note.ID,
                Seller = note.SellerID,
                Downloader = buyer.ID,
                IsSellerHasAllowedDownload = note.IsPaid ? false : true,
                AttachmentPath = note.IsPaid ? null : fileAttachNote.FilePath,
                IsAttachmentDownloaded = note.IsPaid ? false : true,
                AttachmentDownloadedDate = note.IsPaid ? (DateTime?)null : DateTime.Now,
                IsPaid = note.IsPaid,
                PurchasedPrice = note.SellingPrice,
                NoteTitle = note.Title,
                NoteCategory = note.NoteCategory.Name,
                CreatedDate = DateTime.Now,
                CreatedBy = buyer.ID,
                ModifiedDate = DateTime.Now,
                ModifiedBy = buyer.ID
            };
            db.Downloads.Add(download);
            db.SaveChanges();

            if (note.IsPaid)
            {
                var body = "<p>Hello, {0}</p><br>" +
                "<p>We would like to inform you that, {1} wants to purchase your notes. Please see Buyer Requests tab and allow download access to Buyer if you have received the payment from him.</p><br><br><br>" +
                "<p>Regards,</p>" +
                "<p>NoteMarketPlace</p>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(seller.Email));  // Reciever 
                message.From = new MailAddress("******@******.ac.in");  // Sender
                message.Subject = buyer.FirstName + " " + buyer.LastName + " wants to purchase your notes";
                message.Body = string.Format(body, seller.FirstName + " " + seller.LastName, buyer.FirstName + " " + buyer.LastName);
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

                return RedirectToAction("NoteDetail", "SearchNote", new { id = noteId});
            }
            return File(download.AttachmentPath, "application/pdf", fileAttachNote.FileName);


        }





    }
}
