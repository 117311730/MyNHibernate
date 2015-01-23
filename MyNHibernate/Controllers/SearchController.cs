using System;
using System.Web.Mvc;
using MyNHibernate.Infrastructure;
using MyNHibernate.Models;
using NHibernate.Search;

namespace MyNHibernate.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            ViewBag.Title = "Search";
            string kw = Request.Params.Get("kw");
            int p = 0;
            if (!string.IsNullOrEmpty(Request.Params.Get("p")))
                p = Convert.ToInt32(Request.Params.Get("p"));
            //return View();

            IFullTextSession s = SearchHelper.CreateSearcher();

            return Json(s.Search<VendorProducts>(kw, 100), JsonRequestBehavior.AllowGet);
        }
    }
}
