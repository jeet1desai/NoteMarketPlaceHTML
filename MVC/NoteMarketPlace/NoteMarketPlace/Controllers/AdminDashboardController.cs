using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        [Route("AdminDashboard")]
        public ActionResult Index()
        {
            return View();
        }
    }
}