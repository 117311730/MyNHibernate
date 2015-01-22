using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Search;

namespace MyNHibernate.Infrastructure
{
    public class SearchHelper
    {
        public static IFullTextSession CreateSearcher()
        {
            var cfg = NHibernateUtility.GetNHConfiguration();
            var indexPath = LuceneIndexHelper.GetIndexPath();
            return Search.CreateFullTextQuery(cfg, indexPath);
        }
    }
}