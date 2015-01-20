
using NHibernate.Cfg;
using NHibernate.Indexer.Engine;
namespace NHibernate.Indexer
{
    public class Indexer
    {
        public static IIndexBuilder CreateIndex(Configuration cfg, ISession session, string indexPath)
        {
            return new IndexBuilder(cfg, session, indexPath);
        }
    }
}
