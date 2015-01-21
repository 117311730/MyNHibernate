using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using NHibernate.Cfg;
using NHibernate.Indexer.Mapping;
using NHibernate.Indexer.Mapping.AttributeBased;
using NHibernate.Util;
using Lucene.Net.Index;
using Lucene.Net.Documents;

namespace NHibernate.Indexer.Engine
{
   public class IndexBuilder : IIndexBuilder
    {
        private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof(IndexBuilder));
        private readonly ISession session;
        private string basePath;

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
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(Path.Combine(basePath, doc.GetIndexName)), new NativeFSLockFactory());
            bool isUpdate = IndexReader.IndexExists(directory);
            logger.InfoFormat(@"{0}""index satatus is update ? : {1}", doc.GetIndexName, isUpdate);
            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexWriter write = new IndexWriter(directory, analyzer, !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);
            var list = session.CreateCriteria(doc.GetEntryClass.Name).List();
            foreach (var item in list)
            {
                Document document = doc.GetDocument(item);
                write.DeleteDocuments(doc.GetTerm(item));
                write.AddDocument(document);
            }

            write.Optimize();
            write.Commit();
            write.Dispose();
            directory.Dispose();
            logger.InfoFormat("{0} index finished.", doc.GetIndexName);
        }
    }
}
