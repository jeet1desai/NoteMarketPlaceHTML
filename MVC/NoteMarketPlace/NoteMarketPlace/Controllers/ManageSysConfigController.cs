using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ManageSysConfigController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public ManageSysConfigController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: ManageSysConfig
        [Route("ManageSysConfig")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            ViewBag.Setting = "active";
            ViewBag.MSC = "active";

            SystemConfigModel model = new SystemConfigModel();
            var data = db.SystemConfigurations.Select(x => x);

            var SupportEmail = db.SystemConfigurations.Where(x => x.Name == "SupportEmailAddress").FirstOrDefault();
            var ContactNO = db.SystemConfigurations.Where(x => x.Name == "SupportContactAddress").FirstOrDefault();
            var SecondaryEmail = db.SystemConfigurations.Where(x => x.Name == "EmailAddresssesForNotify").FirstOrDefault();
            var FacebookUrl = db.SystemConfigurations.Where(x => x.Name == "FbUrl").FirstOrDefault();
            var TwitterUrl = db.SystemConfigurations.Where(x => x.Name == "TwitterUrl").FirstOrDefault();
            var LinkedInUrl = db.SystemConfigurations.Where(x => x.Name == "LinkedInUrl").FirstOrDefault();
            if (SupportEmail != null)
            {
                model.Email = SupportEmail.Value;
            }
            if (ContactNO != null)
            {
                model.Phone = ContactNO.Value;
            }
            if (SecondaryEmail != null)
            {
                model.SecondaryEmail = SecondaryEmail.Value;
            }
            if (FacebookUrl != null)
            {
                model.FacebookUrl = FacebookUrl.Value;
            }
            if (TwitterUrl != null)
            {
                model.TwitterUrl = TwitterUrl.Value;
            }
            if (LinkedInUrl != null)
            {
                model.LinkedinUrl = LinkedInUrl.Value;
            }
            return View(model);
        }

        [Route("ManageSysConfig")]
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SystemConfigModel systemConfig)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            
            ViewBag.Setting = "active";
            ViewBag.MSC = "active";

            if (ModelState.IsValid)
            {
                SystemConfiguration info = new SystemConfiguration();
                var SupportEmail = db.SystemConfigurations.Where(x => x.Name == "SupportEmailAddress").FirstOrDefault();
                var ContactNO = db.SystemConfigurations.Where(x => x.Name == "SupportContactAddress").FirstOrDefault();
                var SecondaryEmail = db.SystemConfigurations.Where(x => x.Name == "EmailAddresssesForNotify").FirstOrDefault();
                var FacebookUrl = db.SystemConfigurations.Where(x => x.Name == "FbUrl").FirstOrDefault();
                var TwitterUrl = db.SystemConfigurations.Where(x => x.Name == "TwitterUrl").FirstOrDefault();
                var LinkedInUrl = db.SystemConfigurations.Where(x => x.Name == "LinkedInUrl").FirstOrDefault();
                var NoteImage = db.SystemConfigurations.Where(x => x.Name == "DefaultNoteDisplayPicture").FirstOrDefault();
                var ProfileImage = db.SystemConfigurations.Where(x => x.Name == "DefaultMemberDisplayPicture").FirstOrDefault();

                if (SupportEmail == null)
                {
                    info = new SystemConfiguration();
                    info.Name = "SupportEmailAddress";
                    info.Value = systemConfig.Email;
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    SupportEmail.Value = systemConfig.Email;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (ContactNO == null)
                {
                    info = new SystemConfiguration();
                    info.Name = "SupportContactAddress";
                    info.Value = systemConfig.Phone;
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    ContactNO.Value = systemConfig.Phone;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (SecondaryEmail == null)
                {
                    info = new SystemConfiguration();
                    info.Name = "EmailAddresssesForNotify";
                    info.Value = systemConfig.SecondaryEmail;
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    SecondaryEmail.Value = systemConfig.SecondaryEmail;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (FacebookUrl == null)
                {
                    info = new SystemConfiguration();
                    info.Name = "FbUrl";
                    info.Value = systemConfig.FacebookUrl;
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    FacebookUrl.Value = systemConfig.FacebookUrl;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (LinkedInUrl == null)
                {
                    info = new SystemConfiguration();
                    info.Name = "LinkedInUrl";
                    info.Value = systemConfig.LinkedinUrl;
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    LinkedInUrl.Value = systemConfig.LinkedinUrl;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }


                if (TwitterUrl == null)
                {
                    info = new SystemConfiguration();
                    info.Name = "TwitterUrl";
                    info.Value = systemConfig.TwitterUrl;
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    TwitterUrl.Value = systemConfig.TwitterUrl;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

                if (NoteImage == null)
                {
                    if (systemConfig.ImageNote == null)
                    {
                        ModelState.AddModelError("ImageNote", "Image Note is Required");
                        return View();
                    }
                    info = new SystemConfiguration();
                    info.Name = "DefaultNoteDisplayPicture";
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    if (systemConfig.ImageNote != null)
                    {
                        string ImageNotefilename = Path.GetFileName(systemConfig.ImageNote.FileName);
                        string ImageNotePath = "~/Members/AdminSystemConfiguration/ImageNote/";
                        CreateDirectoryIfMissing(ImageNotePath);
                        string displaypicturefilepath = Path.Combine(Server.MapPath("~/Members/AdminSystemConfiguration/ImageNote/"), ImageNotefilename);
                        info.Value = ImageNotePath + ImageNotefilename;
                        systemConfig.ImageNote.SaveAs(displaypicturefilepath);
                    }
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    if (systemConfig.ImageNote != null)
                    {
                        string defaultFilename = Directory.GetFiles(Server.MapPath("~/Members/AdminSystemConfiguration/ImageNote/")).FirstOrDefault();
                        string finalfilename = System.IO.Path.GetFileName(defaultFilename);
                        string PathImage = Request.MapPath("~/Members/AdminSystemConfiguration/ImageNote/" + finalfilename);
                        FileInfo file = new FileInfo(PathImage);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        string ImageNotefilename = Path.GetFileName(systemConfig.ImageNote.FileName);
                        string ImageNotePath = "~/Members/AdminSystemConfiguration/ImageNote/";
                        string displaypicturefilepath = Path.Combine(Server.MapPath("~/Members/AdminSystemConfiguration/ImageNote/"), ImageNotefilename);
                        NoteImage.Value = ImageNotePath + ImageNotefilename;
                        systemConfig.ImageNote.SaveAs(displaypicturefilepath);
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();

                        var Note = db.SellerNotes.Where(x => x.DisplayPicture == "~/Members/AdminSystemConfiguration/ImageNote/" + finalfilename).ToList();
                        foreach (var item in Note)
                        {
                            item.DisplayPicture = ImageNotePath + ImageNotefilename;
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                        }
                    }
                }

                if (ProfileImage == null)
                {
                    if (systemConfig.ImageProfile == null)
                    {
                        ModelState.AddModelError("ImageProfile", "Image Profile is Required");
                        return View();
                    }
                    info = new SystemConfiguration();
                    info.Name = "DefaultMemberDisplayPicture";
                    info.CreatedDate = DateTime.Now;
                    info.CreatedBy = user.ID;
                    info.ModifiedDate = DateTime.Now;
                    info.ModifiedBy = user.ID;
                    info.IsActive = true;
                    if (systemConfig.ImageProfile != null)
                    {
                        string ImageProfilefilename = Path.GetFileName(systemConfig.ImageProfile.FileName);
                        string ImageProfilePath = "~/Members/AdminSystemConfiguration/ImageProfile/";
                        CreateDirectoryIfMissing(ImageProfilePath);
                        string displaypicturefilepath = Path.Combine(Server.MapPath("~/Members/AdminSystemConfiguration/ImageProfile/"), ImageProfilefilename);
                        info.Value = ImageProfilePath + ImageProfilefilename;
                        systemConfig.ImageProfile.SaveAs(displaypicturefilepath);
                    }
                    db.SystemConfigurations.Add(info);
                    db.SaveChanges();
                }
                else
                {
                    if (systemConfig.ImageProfile != null)
                    {
                        string defaultFilename = Directory.GetFiles(Server.MapPath("~/Members/AdminSystemConfiguration/ImageProfile/")).FirstOrDefault();
                        string finalfilename = System.IO.Path.GetFileName(defaultFilename);
                        string PathImage = Request.MapPath("~/Members/AdminSystemConfiguration/ImageProfile/" + finalfilename);
                        FileInfo file = new FileInfo(PathImage);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                        string ImageProfilefilename = Path.GetFileName(systemConfig.ImageProfile.FileName);
                        string ImageProfilePath = "~/Members/AdminSystemConfiguration/ImageProfile/";
                        string displaypicturefilepath = Path.Combine(Server.MapPath("~/Members/AdminSystemConfiguration/ImageProfile/"), ImageProfilefilename);
                        ProfileImage.Value = ImageProfilePath + ImageProfilefilename;
                        systemConfig.ImageProfile.SaveAs(displaypicturefilepath);
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();

                        var User = db.UserProfiles.Where(x => x.ProfilePicture == "~/Members/AdminSystemConfiguration/ImageProfile/" + finalfilename).ToList();
                        foreach (var item in User)
                        {
                            item.ProfilePicture = ImageProfilePath + ImageProfilefilename;
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                        }
                    }
                }

                ViewBag.sucess = "Record Updated Sucessfully";
                return View();

            }


            return View();
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