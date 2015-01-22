using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using NHibernate.Cfg;
using NHibernate.Indexer.Engine;

namespace NHibernate.Search.Engine
{
    class Searcher : IFullTextSession
    {
        private ISession Session;
        private readonly string BasePath;
        private string IndexPath;

        private int TopN;
        private int StartRowIndex;
        private int PageSize;

        public DocumentBuilder DocumentBuilder { get; private set; }

        public int ResultSize { get; private set; }

        public string KeyWord { get; private set; }

        public Searcher(Configuration cfg, string indexPath)
        {
            BasePath = indexPath;
            MappingLoader.Init(cfg);
        }

        public IFullTextSession SetSession(ISession session)
        {
            Session = session;
            return this;
        }

        public IEnumerable<TEntity> Search<TEntity>(string keyWord) where TEntity : new()
        {
            return Search<TEntity>(keyWord, 0);
        }

        public IEnumerable<TEntity> Search<TEntity>(string keyWord, int topN) where TEntity : new()
        {
            return Search<TEntity>(keyWord, topN, 0, 0);
        }

        public IEnumerable<TEntity> Search<TEntity>(string keyWord, int topN, int startRowIndex, int pageSize) where TEntity : new()
        {
            QueryString(keyWord);
            SetPager(topN, startRowIndex, pageSize);
            GetDocumentBuilder(typeof(TEntity));
            IndexPath = Path.Combine(BasePath, DocumentBuilder.GetIndexName);
            return Load<TEntity>(List());
        }

        private void GetDocumentBuilder(System.Type type)
        {
            DocumentBuilder = MappingLoader.DocumentBuilders[type.Name];
            if (DocumentBuilder == null)
            {
                throw new SearchException("Entity not indexed :" + type);
            }
        }

        private List<Document> List()
        {
            var result = new List<Document>();
            using (var provider = new FSDirectoryProvider().Initialize(IndexPath))
            {
                IndexSearcher searcher = new IndexSearcher(provider.Directory);
                Query query = CreateQuery();
                ScoreDoc[] docs = null;
                if (TopN > 0)
                {
                    TopDocs res = searcher.Search(query, TopN);
                    ResultSize = res.TotalHits;
                    docs = res.ScoreDocs;
                }
                else
                {
                    TopScoreDocCollector collector = TopScoreDocCollector.Create(StartRowIndex * PageSize, true);
                    searcher.Search(query, collector);
                    ResultSize = collector.TotalHits;
                    docs = collector.TopDocs(StartRowIndex, PageSize).ScoreDocs;
                }

                for (int i = 0; i < docs.Length; ++i)
                {
                    result.Add(searcher.Doc(docs[i].Doc));
                }
                searcher.Dispose();
            }
            return result;
        }

        private Query CreateQuery()
        {
            string[] fields = DocumentBuilder.ClassMapping.Fields
                .Where(i => i.Index == Indexer.Attributes.Index.Tokenized)
                .Select(i => i.Name)
                .ToArray<string>();
            MultiFieldQueryParser queryParser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));

            return queryParser.Parse(KeyWord);
        }

        private IEnumerable<TEntity> Load<TEntity>(List<Document> docs)
        {
            if (Session == null)
            {
                throw new SearchException("Session is null .");
            }

            var list = new List<TEntity>();
            foreach (var item in docs)
            {
                System.Type type = DocumentBuilder.DocumentId.Getter.ReturnType;
                string idStr = item.Get(DocumentBuilder.DocumentId.Name);
                object id = ConvertValue(type, idStr);
                list.Add(Session.Load<TEntity>(id));
            }
            return list;
        }

        private void SetPager(int topN = 0, int startRowIndex = 1, int pageSize = 20)
        {
            TopN = topN <= 0 ? 0 : topN;
            StartRowIndex = startRowIndex <= 1 ? 1 : startRowIndex;
            PageSize = pageSize <= 20 ? 20 : pageSize;
        }

        private void QueryString(string kw)
        {
            KeyWord = kw.Trim();
        }

        public static object ConvertValue(System.Type t, string value)
        {
            if (t.Name == typeof(int).Name)
                return int.Parse(value);
            return value;
        }
    }
}
