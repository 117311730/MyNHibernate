using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyNHibernate.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            ViewBag.Title = "Search";
            return View();
        }

    }
}
