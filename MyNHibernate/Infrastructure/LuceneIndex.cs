using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using MyNHibernate.Models;
using MyNHibernate.Util;
using NHibernate.Indexer;
using NHibernate.Indexer.Attributes;
using NHibernate.Indexer.Mapping.AttributeBased;
namespace MyNHibernate.Infrastructure
{
    public class LuceneIndex
    {
        //private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(LuceneIndex));
        //public LuceneIndex()
        //{
        //}
        //public static void Create()
        //{
        //    //string indexPath = Context.Server.MapPath("~/IndexData");
        //    string indexPath = SystemInfo.ConvertToFullPath(ConfigurationManager.AppSettings["pathIndex"]);
        //    FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
        //    bool isUpdate = IndexReader.IndexExists(directory);
        //    Log.Info("index satatus is update ? :" + isUpdate);
        //    if (isUpdate)
        //    {
        //        if (IndexWriter.IsLocked(directory))
        //        {
        //            IndexWriter.Unlock(directory);
        //        }
        //    }
        //    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        //    IndexWriter write = new IndexWriter(directory, analyzer, !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);

        //    foreach (var item in GetProduct())
        //    {
        //        //write.DeleteDocuments(new Term("id", item.ProductId.ToString()));
        //        Document document = new Document();
        //        document.Add(new Field("id", item.ProductId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //        document.Add(new Field("pid", item.ParentId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //        document.Add(new Field("vid", item.VendorId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //        document.Add(new Field("name", item.Name, Field.Store.YES, Field.Index.ANALYZED));
        //        document.Add(new Field("price", item.Price.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //        document.Add(new Field("tags", item.Tags, Field.Store.YES, Field.Index.ANALYZED));
        //        document.Add(new Field("desc", item.Description, Field.Store.YES, Field.Index.ANALYZED));
        //        document.Add(new Field("thumbnail", item.Thumbnail, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //        document.Add(new Field("sort", item.SortOrder.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

        //        write.AddDocument(document);
        //    }

        //    write.Optimize();
        //    write.Dispose();
        //    directory.Dispose();
        //    Log.Info("product index finished.");
        //}

        //public static IEnumerable<T> Search<T>(string keyWord, int startRowIndex, int pageSize) where T : new()
        //{
        //    IEnumerable<T> results;
        //    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        //    string indexPath = SystemInfo.ConvertToFullPath(ConfigurationManager.AppSettings["pathIndex"]);
        //    FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
        //    IndexSearcher searcher = new IndexSearcher(directory);
        //    MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new string[] { "name", "tags", "desc" }, analyzer);
        //    Query query = parser.Parse(keyWord);
        //    TopScoreDocCollector collector = TopScoreDocCollector.Create(startRowIndex * pageSize, true);
        //    searcher.Search(query, collector);
        //    // int totalCount = collector.GetTotalHits();//返回总条数
        //    ScoreDoc[] docs = collector.TopDocs(startRowIndex, pageSize).ScoreDocs;//分页,下标应该从0开始吧，0是第一条记录
        //    List<T> list = new List<T>();
        //    for (int i = 0; i < docs.Length; i++)
        //    {
        //        int dosId = docs[i].Doc;
        //        Document doc = searcher.Doc(dosId);
        //        //         T result = new T() { Id = doc.Get("id"),Name = doc.Get("name")};
        //    }
        //    searcher.Dispose();
        //    return list;
        //}

        //public static IEnumerable<VendorProducts> GetProduct()
        //{
        //    using (var session = NHibernateUtility.GetSessionFactory().OpenSession())
        //    {
        //        return session.QueryOver<VendorProducts>().List();
        //    }
        //}

    }
}