using System.Configuration;
using MyNHibernate.Util;
namespace MyNHibernate.Infrastructure
{
    public class LuceneIndexHelper
    {
        public static string GetIndexPath()
        {
            //string indexPath = Context.Server.MapPath("~/IndexData");
            return SystemInfo.ConvertToFullPath(ConfigurationManager.AppSettings["pathIndex"]);
        }
    }
}