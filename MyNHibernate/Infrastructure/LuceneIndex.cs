using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using MyNHibernate.Models;
using MyNHibernate.Util;
namespace MyNHibernate.Infrastructure
{
    public class LuceneIndex
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LuceneIndex));
        public LuceneIndex()
        {
        }
        public static void Create()
        {
            string indexPath = SystemInfo.ConvertToFullPath(ConfigurationManager.AppSettings["pathIndex"]);
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isUpdate = IndexReader.IndexExists(directory);
            Log.Info("index satatus is update ? :" + isUpdate);
            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter write = new IndexWriter(directory, analyzer, !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (var item in GetProduct())
            {
                //write.DeleteDocuments(new Term("id", item.ProductId.ToString()));
                Document document = new Document();
                document.Add(new Field("id", item.ProductId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("pid", item.ParentId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("vid", item.VendorId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("name", item.Name, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("price", item.Price.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("tags", item.Tags, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("desc", item.Description, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("thumbnail", item.Thumbnail, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("sort", item.SortOrder.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                write.AddDocument(document);
            }

            write.Dispose();
            directory.Dispose();
            Log.Info("product index finished.");
        }

        public static IEnumerable<VendorProducts> GetProduct()
        {
            using (var session = NHibernateUtility.GetSessionFactory().OpenSession())
            {
                return session.QueryOver<VendorProducts>().List();
            }
        }
    }
}