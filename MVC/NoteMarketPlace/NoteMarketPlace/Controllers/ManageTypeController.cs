using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ManageTypeController : Controller
    {
       
        readonly private NoteMarketPlaceEntities db;

        public ManageTypeController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: ManageType
        [Route("ManageType")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult Index(string search, string sortOrder, int mtPage = 1)
        {
            ViewBag.Setting = "active";
            ViewBag.MType = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.DescriptSortParm = sortOrder == "Descript" ? "Descript_desc" : "Descript";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.AddedBySortParm = sortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Active_desc" : "Active";


            var TypeList = db.NoteTypes.Select(x => x).ToList();


            if (!String.IsNullOrEmpty(search))
            {
                TypeList = TypeList.Where(x => x.Name.Contains(search) || x.Description.Contains(search) ||
                                            x.CreatedDate.ToString().Contains(search) ||
                                            (db.Users.Where(z => z.ID == x.CreatedBy).FirstOrDefault().FirstName + " " + db.Users.Where(z => z.ID == x.CreatedBy).FirstOrDefault().LastName).Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "Type_desc":
                    TypeList = TypeList.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Type":
                    TypeList = TypeList.OrderBy(s => s.Name).ToList();
                    break;
                case "Descript_desc":
                    TypeList = TypeList.OrderByDescending(s => s.Description).ToList();
                    break;
                case "Descript":
                    TypeList = TypeList.OrderBy(s => s.Description).ToList();
                    break;
                case "AddedBy_desc":
                    TypeList = TypeList.OrderByDescending(s => (db.Users.Where(x => x.ID == s.CreatedBy).FirstOrDefault().FirstName)).ToList();
                    break;
                case "AddedBy":
                    TypeList = TypeList.OrderBy(s => (db.Users.Where(x => x.ID == s.CreatedBy).FirstOrDefault().FirstName)).ToList();
                    break;
                case "Active_desc":
                    TypeList = TypeList.OrderByDescending(s => s.IsActive).ToList();
                    break;
                case "Active":
                    TypeList = TypeList.OrderBy(s => s.IsActive).ToList();
                    break;
                case "Date_desc":
                    TypeList = TypeList.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
                case "Date":
                    TypeList = TypeList.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    TypeList = TypeList.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(TypeList.Count(), mtPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = mtPage;

            ViewBag.srno = mtPage;
            ViewBag.TotalTypePage = Math.Ceiling(TypeList.Count() / 5.0);
            TypeList = TypeList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(TypeList);
        }







        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult InActiveType(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var Type = db.NoteTypes.Where(x => x.ID == id).FirstOrDefault();

            Type.IsActive = false;
            Type.ModifiedDate = DateTime.Now;
            Type.ModifiedBy = user.ID;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Index", "ManageType");
        }








        [Route("ManageType/AddType")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult AddType()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MType = "active";

            return View();
        }


        [Route("ManageType/AddType")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddType(NoteType type)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MType = "active";

            var CheckType = db.NoteTypes.Where(x => x.Name == type.Name).FirstOrDefault();
            if (CheckType != null)
            {
                ModelState.AddModelError("Name", "This Type Name Already Exist");
                return View(type);
            }

            if (ModelState.IsValid)
            {
                NoteType newType = new NoteType();
                newType.Name = type.Name;
                newType.Description = type.Description;
                newType.CreatedDate = DateTime.Now;
                newType.ModifiedDate = DateTime.Now;
                newType.CreatedBy = user.ID;
                newType.IsActive = true;
                db.NoteTypes.Add(newType);
                db.SaveChanges();

                return RedirectToAction("Index", "ManageType");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                return View(type);
            }
        }











        [Route("ManageType/EditType/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult EditType(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MType = "active";

            var type = db.NoteTypes.Where(x => x.ID == id).FirstOrDefault();

            NoteType noteType = new NoteType();
            noteType.Name = type.Name;
            noteType.Description = type.Description;

            return View(noteType);
        }



        [Route("ManageType/EditType/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditType(NoteType type)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MType = "active";

            var EditType = db.NoteTypes.Where(x => x.ID == type.ID).FirstOrDefault();

            var check = db.NoteTypes.Where(x => x.ID != type.ID && x.Name == type.Name).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError("Name", "This Type Name Already Exist");
                return View(type);
            }
            if (ModelState.IsValid)
            {
                EditType.Name = type.Name;
                EditType.Description = type.Description;
                EditType.ModifiedDate = DateTime.Now;
                EditType.ModifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return RedirectToAction("Index", "ManageType");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                return View(type);
            }
        }
    }
}