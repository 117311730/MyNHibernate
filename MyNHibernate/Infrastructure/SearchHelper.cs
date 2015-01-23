using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Search;

namespace MyNHibernate.Infrastructure
{
    public class SearchHelper
    {
        private static IFullTextSession FullTextSession;
        public static IFullTextSession CreateSearcher()
        {
            var cfg = NHibernateUtility.GetNHConfiguration();
            var indexPath = LuceneIndexHelper.GetIndexPath();
            if (FullTextSession == null)
                return Search.CreateFullTextQuery(cfg, indexPath);
            else
                return FullTextSession;
        }
    }
}