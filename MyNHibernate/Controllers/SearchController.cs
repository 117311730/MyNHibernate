using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using MyNHibernate.Infrastructure;
using MyNHibernate.Models;
using MyNHibernate.Util;
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
            //return View();

            IFullTextSession s = SearchHelper.CreateSearcher();
            s.SetSession(MvcApplication.GetCurrentSession());

            return Json(s.Search<VendorProducts>(kw), JsonRequestBehavior.AllowGet);
        }
    }
}
