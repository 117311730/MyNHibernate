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
        private readonly IIndexMapping mapping;
        private readonly Dictionary<string, DocumentBuilder> documentBuilders = new Dictionary<string, DocumentBuilder>();
        private readonly ISession session;
        private string basePath;

        public IndexBuilder(Configuration cfg, ISession sen, string indexPath)
        {
            mapping = new AttributeSearchMapping();
            session = sen;
            basePath = indexPath;

            Analyzer analyzer = InitAnalyzer(cfg);
            InitDocumentBuilders(cfg, analyzer);
        }

        public void Build()
        {
            foreach (var item in documentBuilders)
            {
                create(item.Value);
            }
        }

        private static Analyzer InitAnalyzer(Configuration cfg)
        {
            System.Type analyzerClass;

            String analyzerClassName = cfg.GetProperty(Environment.AnalyzerClass);
            if (analyzerClassName != null)
                try
                {
                    analyzerClass = ReflectHelper.ClassForName(analyzerClassName);
                }
                catch (Exception e)
                {
                    throw new IndexException(
                        string.Format("Lucene analyzer class '{0}' defined in property '{1}' could not be found.",
                                      analyzerClassName, Environment.AnalyzerClass), e);
                }
            else
                analyzerClass = typeof(StandardAnalyzer);
            // Initialize analyzer
            Analyzer defaultAnalyzer;
            try
            {
                object param = null;
                if (analyzerClass == typeof(StandardAnalyzer))
                    param = Lucene.Net.Util.Version.LUCENE_30;
                defaultAnalyzer = (Analyzer)Activator.CreateInstance(analyzerClass, param);
            }
            catch (InvalidCastException)
            {
                throw new IndexException(
                    string.Format("Lucene analyzer does not implement {0}: {1}", typeof(Analyzer).FullName,
                                  analyzerClassName)
                    );
            }
            catch (Exception)
            {
                throw new IndexException("Failed to instantiate lucene analyzer with type " + analyzerClassName);
            }
            return defaultAnalyzer;
        }

        private void InitDocumentBuilders(Configuration cfg, Analyzer analyzer)
        {
            var classMappings = this.mapping.Build(cfg);
            foreach (var classMapping in classMappings)
            {
                classMapping.Analyzer = analyzer;
                DocumentBuilder documentBuilder = new DocumentBuilder(classMapping);
                documentBuilders[classMapping.IndexName] = documentBuilder;
            }
        }

        private FSDirectory InitDir(string indexName)
        {
            return FSDirectory.Open(new DirectoryInfo(Path.Combine(basePath, indexName)), new NativeFSLockFactory());
        }

        public void create(DocumentBuilder doc)
        {
            FSDirectory directory = InitDir(doc.GetIndexName);
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
