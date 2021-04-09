using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ManageCountryController : Controller
    {
        readonly private NoteMarketPlaceEntities db;

        public ManageCountryController()
        {
            db = new NoteMarketPlaceEntities();
        }

        // GET: ManageCountry
        [Route("ManageCountry")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult Index(string search, string sortOrder, int mctrPage = 1)
        {
            ViewBag.Setting = "active";
            ViewBag.MCountry = "active";

            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.CountrySortParm = sortOrder == "Country" ? "Country_desc" : "Country";
            ViewBag.CCodeSortParm = sortOrder == "CCode" ? "CCode_desc" : "CCode";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.AddedBySortParm = sortOrder == "AddedBy" ? "AddedBy_desc" : "AddedBy";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Active_desc" : "Active";

            var CountryList = db.Countries.Select(x => x).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                CountryList = CountryList.Where(x => x.Name.Contains(search) || x.CountryCode.Contains(search) ||
                                            x.CreatedDate.ToString().Contains(search) ||
                                            (db.Users.Where(z => z.ID == x.CreatedBy).FirstOrDefault().FirstName + " " + db.Users.Where(z => z.ID == x.CreatedBy).FirstOrDefault().LastName).Contains(search)).ToList();
            }

            switch (sortOrder)
            {
                case "Country_desc":
                    CountryList = CountryList.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Country":
                    CountryList = CountryList.OrderBy(s => s.Name).ToList();
                    break;
                case "CCode_desc":
                    CountryList = CountryList.OrderByDescending(s => s.CountryCode).ToList();
                    break;
                case "CCode":
                    CountryList = CountryList.OrderBy(s => s.CountryCode).ToList();
                    break;
                case "AddedBy_desc":
                    CountryList = CountryList.OrderByDescending(s => (db.Users.Where(x => x.ID == s.CreatedBy).FirstOrDefault().FirstName)).ToList();
                    break;
                case "AddedBy":
                    CountryList = CountryList.OrderBy(s => (db.Users.Where(x => x.ID == s.CreatedBy).FirstOrDefault().FirstName)).ToList();
                    break;
                case "Active_desc":
                    CountryList = CountryList.OrderByDescending(s => s.IsActive).ToList();
                    break;
                case "Active":
                    CountryList = CountryList.OrderBy(s => s.IsActive).ToList();
                    break;
                case "Date_desc":
                    CountryList = CountryList.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
                case "Date":
                    CountryList = CountryList.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    CountryList = CountryList.OrderByDescending(s => s.CreatedDate).ToList();
                    break;
            }

            var pager = new ADPager(CountryList.Count(), mctrPage);
            ViewBag.currentPage = pager.CurrentPage;
            ViewBag.endPage = pager.EndPage;
            ViewBag.startpage = pager.StartPage;
            ViewBag.pageNumber = mctrPage;

            ViewBag.srno = mctrPage;
            ViewBag.TotalCountryPage = Math.Ceiling(CountryList.Count() / 5.0);
            CountryList = CountryList.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            return View(CountryList);
        }




        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult InActiveCountry(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var Country = db.Countries.Where(x => x.ID == id).FirstOrDefault();

            Country.IsActive = false;
            Country.ModifiedDate = DateTime.Now;
            Country.ModifiedBy = user.ID;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return RedirectToAction("Index", "ManageCountry");
        }









        [Route("ManageCountry/AddCountry")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult AddCountry()
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCountry = "active";

            return View();
        }




        [Route("ManageCountry/AddCountry")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCountry(Country country)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCountry = "active";

            string code = country.CountryCode.StartsWith("+") ? country.CountryCode : "+" + country.CountryCode;
            var CheckCountry = db.Countries.Where(x => x.Name == country.Name).FirstOrDefault();
            var CheckCountryCode = db.Countries.Where(x => x.CountryCode == code).FirstOrDefault();
            if (CheckCountry != null)
            {
                ModelState.AddModelError("Name", "This Country Name Already Exist");
                return View(country);
            }
            if (CheckCountryCode != null)
            {
                ModelState.AddModelError("CountryCode", "This Country Code Already Exist");
                return View(country);
            }

            if (ModelState.IsValid)
            {
                Country newContry = new Country();
                newContry.Name = country.Name;
                newContry.CountryCode = code;
                newContry.CreatedDate = DateTime.Now;
                newContry.ModifiedDate = DateTime.Now;
                newContry.CreatedBy = user.ID;
                newContry.IsActive = true;
                db.Countries.Add(newContry);
                db.SaveChanges();

                return RedirectToAction("Index", "ManageCountry");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                return View(country);
            }
        }











        [Route("ManageCountry/EditCountry/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult EditCountry(int id)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCountry = "active";


            var country = db.Countries.Where(x => x.ID == id).FirstOrDefault();

            Country ctr = new Country();
            ctr.Name = country.Name;
            ctr.CountryCode = country.CountryCode;

            return View(ctr);
        }

        [Route("ManageCountry/EditCountry/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCountry(Country country)
        {
            var user = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            ViewBag.Setting = "active";
            ViewBag.MCountry = "active";

            var Country = db.Countries.Where(x => x.ID == country.ID).FirstOrDefault();
            string code = country.CountryCode.StartsWith("+") ? country.CountryCode : "+" + country.CountryCode;

            var check = db.Countries.Where(x => x.ID != country.ID && x.Name == country.Name).FirstOrDefault();
            var checkCode = db.Countries.Where(x => x.ID != country.ID && x.CountryCode == code).FirstOrDefault();
            if (check != null)
            {
                ModelState.AddModelError("Name", "This Country Name Already Exist");
                return View(country);
            }
            if (checkCode != null)
            {
                ModelState.AddModelError("CountryCode", "This Country Code Already Exist");
                return View(country);
            }

            if (ModelState.IsValid)
            {
                Country.Name = country.Name;
                Country.CountryCode = code;
                Country.ModifiedDate = DateTime.Now;
                Country.ModifiedBy = user.ID;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return RedirectToAction("Index", "ManageCountry");
            }
            else
            {
                ViewBag.Error = "Some Error Occure! Please Try Again";
                return View(country);
            }
        }


    }
}