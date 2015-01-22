using NHibernate.Cfg;
using Lucene.Net.Documents;
using System.IO;

namespace NHibernate.Indexer.Engine
{
    public class IndexBuilder : IIndexBuilder
    {
        private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof(IndexBuilder));
        private readonly ISession session;
        private readonly string basePath;

        public IndexBuilder(Configuration cfg, ISession sen, string indexPath)
        {
            session = sen;
            basePath = indexPath;
            MappingLoader.Init(cfg);
        }

        public void Build()
        {
            foreach (var item in MappingLoader.DocumentBuilders)
            {
                create(item.Value);
            }
        }

        public void create(DocumentBuilder doc)
        {
            string indexPath = Path.Combine(basePath,doc.GetIndexName);
            using (var provider = new FSDirectoryProvider().Initialize(indexPath))
            {
                logger.InfoFormat("{0} index satatus is update ? : {1}", doc.GetIndexName, provider.IsUpdate);
                var list = session.CreateCriteria(doc.GetEntryClass.Name).List();
                foreach (var item in list)
                {
                    Document document = doc.GetDocument(item);
                    provider.GetIndexWriter.DeleteDocuments(doc.GetTerm(item));
                    provider.GetIndexWriter.AddDocument(document);
                }
                logger.InfoFormat("{0} index finished.", doc.GetIndexName);
            }
        }
    }
}
