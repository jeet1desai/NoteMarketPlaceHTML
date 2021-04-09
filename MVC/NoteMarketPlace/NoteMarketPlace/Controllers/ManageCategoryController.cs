using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ManageCategoryController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public ManageCategoryController()
        {
            db = new NoteMarketPlaceEntities();
        }


        // GET: ManageCategory
        [Route("ManageCategory")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult Index(string search, string sortOrder, int mcPage = 1)
        {
            ViewBag.Setting = "active";
            ViewBag.MCategory = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.DescriptSortParm = sortOrder == "Descript" ? "Descript_desc" : "Descript";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.AddedBySortParm = sortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Active_desc" : "Active";

            var Category = db.NoteCategories.Select(x => x).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                Category = Category.Where(x => x.Name.Contains(search) || x.Description.Contains(search) ||
                                            x.CreatedDate.ToString().Contains(search)||
                                            (db.Users.Where(z=>z.ID == x.CreatedBy).FirstOrDefault().FirstName + " " + db.Users.Where(z => z.ID == x.CreatedBy).FirstOrDefault().LastName).Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "Category_desc":
                    Category = Category.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Category":
                    Category = Category.OrderBy(s => s.Name).ToList();
                    break;
                case "Descript_desc":
                    Category = Category.OrderByDescending(s => s.Description).ToList();
                    break;
                case "Descript":
                    Category = Category.OrderBy(s => s.Description).ToList();
                    break;
                case "AddedBy_desc":
                    Category = Category.OrderByDescending(s => (db.Users.Where(x => x.ID == s.CreatedBy).FirstOrDefault().FirstName)).ToList();
                    break;
                case "AddedBy":
                    Category = Category.OrderBy(s => (db.Users.Where(x => x.ID == s.CreatedBy).FirstOrDefault().FirstName)).ToList();
                    break;
                case "Active_desc":
                    Category = Category.OrderByDescending(s => s.IsActive).ToList();
                    break;
                case "Active":
                    Category = Category.OrderBy(s => s.IsActive).ToList();
                    break;
                case "Date_desc":
                    Category = Category.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
                case "Date":
                    Category = Category.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    Category = Category.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(Category.Count(), mcPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = mcPage;

            ViewBag.srno = mcPage;
            ViewBag.TotalCategoryPage = Math.Ceiling(Category.Count() / 5.0);
            Category = Category.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(Category);
        }







        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult InActiveCategory(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var Category = db.NoteCategories.Where(x => x.ID == id).FirstOrDefault();

            Category.IsActive = false;
            Category.ModifiedDate = DateTime.Now;
            Category.ModifiedBy = user.ID;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Index", "ManageCategory");
        }







        [Route("ManageCategory/AddCategory")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult AddCategory()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCategory = "active";

            return View();
        }




        [Route("ManageCategory/AddCategory")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(NoteCategory category)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCategory = "active";

            var CheckCat = db.NoteCategories.Where(x => x.Name == category.Name).FirstOrDefault();
            if(CheckCat != null)
            {
                ModelState.AddModelError("Name", "This Category Name Already Exist");
                return View(category);
            }

            if (ModelState.IsValid)
            {
                NoteCategory cat = new NoteCategory();
                cat.Name = category.Name;
                cat.Description = category.Description;
                cat.CreatedDate = DateTime.Now;
                cat.ModifiedDate = DateTime.Now;
                cat.CreatedBy = user.ID;
                cat.IsActive = true;
                db.NoteCategories.Add(cat);
                db.SaveChanges();

                return RedirectToAction("Index", "ManageCategory");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                return View(category);
            }
        }









        [Route("ManageCategory/EditCategory/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCategory = "active";


            var catagory = db.NoteCategories.Where(x => x.ID == id).FirstOrDefault();

            NoteCategory noteCategory = new NoteCategory();
            noteCategory.Name = catagory.Name;
            noteCategory.Description = catagory.Description;

            return View(noteCategory);
        }

        [Route("ManageCategory/EditCategory/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(NoteCategory category)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCategory = "active";

            var Cat = db.NoteCategories.Where(x => x.ID == category.ID).FirstOrDefault();

            var check = db.NoteCategories.Where(x => x.ID != category.ID && x.Name == category.Name).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError("Name", "This Category Name Already Exist");
                return View(category);
            }

            if (ModelState.IsValid)
            {
                Cat.Name = category.Name;
                Cat.Description = category.Description;
                Cat.ModifiedDate = DateTime.Now;
                Cat.ModifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return RedirectToAction("Index", "ManageCategory");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                return View(category);
            }
        }

    }
}