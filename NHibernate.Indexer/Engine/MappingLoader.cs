using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using NHibernate.Cfg;
using NHibernate.Indexer.Mapping.AttributeBased;
using NHibernate.Util;

namespace NHibernate.Indexer.Engine
{
    public class MappingLoader
    {
        private static readonly Dictionary<string, DocumentBuilder> documentBuilders = new Dictionary<string, DocumentBuilder>();

        public static void Init(Configuration cfg)
        {
            if (documentBuilders == null || documentBuilders.Count <= 0)
            {
                Analyzer analyzer = InitAnalyzer(cfg);
                InitDocumentBuilders(cfg, analyzer);
            }
        }

        public static IReadOnlyDictionary<string, DocumentBuilder> DocumentBuilders
        {
            get { return documentBuilders; }
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

        private static void InitDocumentBuilders(Configuration cfg, Analyzer analyzer)
        {
            var classMappings = new AttributeSearchMapping().Build(cfg);
            foreach (var classMapping in classMappings)
            {
                classMapping.Analyzer = analyzer;
                DocumentBuilder documentBuilder = new DocumentBuilder(classMapping);
                documentBuilders[classMapping.MappedClass.Name] = documentBuilder;
            }
        }
    }
}
