using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class FAQController : Controller
    {
        // GET: FAQ
        [Route("FAQ")]
        public ActionResult Index()
        {
            ViewBag.FAQ = "active";
            ViewBag.Class = "white-nav";
            return View();
        }
    }
}