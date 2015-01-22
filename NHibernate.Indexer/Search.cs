using NHibernate.Cfg;
using NHibernate.Search.Engine;

namespace NHibernate.Search
{
    public class Search
    {
        public static IFullTextSession CreateFullTextQuery(Configuration cfg, string indexPath)
        {
            return new Searcher(cfg, indexPath);
        }
    }
}
