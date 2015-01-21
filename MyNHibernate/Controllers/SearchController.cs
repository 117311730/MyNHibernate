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
using MyNHibernate.Models;
using MyNHibernate.Util;

namespace MyNHibernate.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            ViewBag.Title = "Search";
            string kw = Request.Params["kw"].ToString();
           // return View();

            return Json(Search<VendorProducts>(kw, 1, 5), JsonRequestBehavior.AllowGet);
        }

        public static IEnumerable<int> Search<T>(string keyWord, int startRowIndex, int pageSize) where T : new()
        {
            Type myType = typeof(T);
            string indexPath = SystemInfo.ConvertToFullPath(ConfigurationManager.AppSettings["pathIndex"]);
            indexPath = Path.Combine(indexPath, myType.Name);
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            IndexSearcher searcher = new IndexSearcher(directory);
            MultiFieldQueryParser queryParser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new string[] { "Name", "Tags", "Description" }, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));

            Query query = queryParser.Parse(keyWord);
            TopScoreDocCollector collector = TopScoreDocCollector.Create(startRowIndex * pageSize, true);
            searcher.Search(query, collector);
            // int totalCount = collector.GetTotalHits();//返回总条数
            ScoreDoc[] docs = collector.TopDocs(startRowIndex, pageSize).ScoreDocs;//分页,下标应该从0开始吧，0是第一条记录
            List<int> list = new List<int>();
            for (int i = 0; i < docs.Length; i++)
            {
                int dosId = docs[i].Doc;
                list.Add(Convert.ToInt32(searcher.Doc(dosId).Get("ProductId")));
            }
            searcher.Dispose();
            return list;
        }
    }
}
