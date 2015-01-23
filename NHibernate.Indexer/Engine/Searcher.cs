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
        private readonly string BasePath;
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

        public IEnumerable<TEntity> Search<TEntity>(string keyWord) where TEntity : new()
        {
            return Search<TEntity>(keyWord, 0);
        }

        public IEnumerable<TEntity> Search<TEntity>(string keyWord, int topN) where TEntity : new()
        {
            return Search<TEntity>(keyWord, topN, 0, 0);
        }

        public IEnumerable<TEntity> Search<TEntity>(string keyWord, int startRowIndex, int pageSize) where TEntity : new()
        {
            return Search<TEntity>(keyWord, 0, startRowIndex, startRowIndex);
        }

        public IEnumerable<TEntity> Search<TEntity>(string keyWord, int topN, int startRowIndex, int pageSize) where TEntity : new()
        {
            QueryString(keyWord);
            SetPager(topN, startRowIndex, pageSize);
            GetDocumentBuilder(typeof(TEntity));
            return List<TEntity>(Load(Path.Combine(BasePath, DocumentBuilder.GetIndexName)));
        }

        private void GetDocumentBuilder(System.Type type)
        {
            DocumentBuilder = MappingLoader.DocumentBuilders[type.Name];
            if (DocumentBuilder == null)
            {
                throw new SearchException("Entity not indexed :" + type);
            }
        }

        private List<Document> Load(string indexPath)
        {
            var result = new List<Document>();
            using (var provider = new FSDirectoryProvider().Initialize(indexPath))
            {
                IndexSearcher searcher = new IndexSearcher(provider.Directory);
                Query query = CreateQuery();
                if (query != null)
                {
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
                        docs = collector.TopDocs(PageSize * (StartRowIndex - 1), PageSize).ScoreDocs;
                    }
                    for (int i = 0; i < docs.Length; ++i)
                    {
                        result.Add(searcher.Doc(docs[i].Doc));
                    }
                }
                else
                {
                    int start = 0;
                    int end = 0;
                    if (TopN > 0)
                    {
                        end = TopN < searcher.MaxDoc ? TopN : searcher.MaxDoc;
                    }
                    else
                    {
                        start = PageSize * (StartRowIndex - 1);
                        end = StartRowIndex * PageSize;
                        end = end < searcher.MaxDoc ? end : searcher.MaxDoc;
                    }
                    for (; start < end; ++start)
                    {
                        result.Add(searcher.Doc(start));
                    }
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
            try
            {
                return queryParser.Parse(KeyWord);
            }
            catch (ParseException ex)
            {
                return null;
            }
        }

        private IEnumerable<TEntity> List<TEntity>(List<Document> docs) where TEntity : new()
        {
            var list = new List<TEntity>();
            foreach (var item in docs)
            {
                list.Add(BindEntity<TEntity>(item));
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
            if (string.IsNullOrEmpty(kw))
                KeyWord = string.Empty;
            else
                KeyWord = kw.Trim();
        }

        public TEntity BindEntity<TEntity>(Document doc) where TEntity : new()
        {
            // another way: build to json,then deserialize to oject
            // what's that?
            TEntity entity = new TEntity();
            foreach (var item in entity.GetType().GetProperties())
            {
                item.SetValue(entity, ChangeType(doc.Get(item.Name), item.PropertyType));
            }
            return entity;
        }

        static public object ChangeType(object value, System.Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                System.Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }
    }
}
