using System;
using System.Collections.Generic;
using System.Globalization;
using Lucene.Net.Documents;
using NHibernate.Indexer.Mapping;
using NHibernate.Util;

namespace NHibernate.Indexer.Engine
{
    using Lucene.Net.Index;
    using NHibernate.Indexer.Attributes;
    using Type = System.Type;

    public class DocumentBuilder
    {
        private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof(DocumentBuilder));

        private IList<Type> mappedSubclasses = new List<Type>();
        private readonly DocumentMapping rootClassMapping;

        #region Constructors

        public DocumentBuilder(DocumentMapping classMapping)
        {
            if (classMapping == null) throw new AssertionFailure("Unable to build a DocumemntBuilder with a null class");

            rootClassMapping = classMapping;
            if (rootClassMapping.DocumentId == null)
                throw new IndexException("No document id for: " + classMapping.MappedClass.Name);
        }

        #endregion

        #region Property methods

        public IList<System.Type> MappedSubclasses
        {
            get { return mappedSubclasses; }
        }

        public DocumentIdMapping DocumentId
        {
            get { return rootClassMapping.DocumentId; }
        }

        public string GetIndexName
        {
            get { return rootClassMapping.IndexName; }
        }

        public Type GetEntryClass
        {
            get { return rootClassMapping.MappedClass; }
        }

        #endregion

        public Document GetDocument(object instance)
        {
            Document doc = new Document();
            BuildDocumentFields(instance, doc, rootClassMapping, string.Empty);
            return doc;
        }

        public Term GetTerm(object instance)
        {
            return new Term(DocumentId.Name, DocumentId.Getter.Get(instance).ToString());
        }

        public static System.Type GetDocumentClass(Document document, string fieldName)
        {
            string className = document.Get(fieldName);
            try
            {
                return ReflectHelper.ClassForName(className);
            }
            catch (Exception e)
            {
                throw new IndexException("Unable to load indexed class: " + className, e);
            }
        }

        private void BuildDocumentFields(Object instance, Document doc, DocumentMapping classMapping, string prefix)
        {
            if (instance == null)
            {
                return;
            }

            BuildDocumentId(instance, doc);

            foreach (var field in classMapping.Fields)
            {
                BuildDocumentField(field, instance, doc, prefix);
            }
        }

        private void BuildDocumentId(Object instance, Document doc)
        {
            doc.Add(new Field(DocumentId.Name, DocumentId.Getter.Get(instance).ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        }

        private void BuildDocumentField(FieldMapping fieldMapping, object unproxiedInstance, Document doc, string prefix)
        {
            var value = fieldMapping.Getter.Get(unproxiedInstance);
            var fieldName = prefix + fieldMapping.Name;
            Field field = null;
            try
            {
                field = new Field(fieldName.ToString(), value.ToString(), GetStore(fieldMapping.Store), GetIndex(fieldMapping.Index));
                if (fieldMapping.Boost != null)
                    field.Boost = fieldMapping.Boost.Value;
                doc.Add(field);
            }
            catch (Exception e)
            {
                logger.Error(
                    string.Format(CultureInfo.InvariantCulture, "Error processing field for {0}.{1}",
                                  unproxiedInstance.GetType().FullName, fieldName), e);
            }
        }

        private static int GetFieldPosition(string[] fields, string fieldName)
        {
            int fieldNbr = fields.GetUpperBound(0);
            for (int index = 0; index < fieldNbr; index++)
            {
                if (fieldName.Equals(fields[index]))
                {
                    return index;
                }
            }

            return -1;
        }

        private static Field.Index GetIndex(Index index)
        {
            switch (index)
            {
                case Index.No:
                    return Field.Index.NO;
                case Index.NoNorms:
                    return Field.Index.NOT_ANALYZED_NO_NORMS;
                case Index.Tokenized:
                    return Field.Index.ANALYZED;
                case Index.UnTokenized:
                    return Field.Index.NOT_ANALYZED;
                default:
                    throw new AssertionFailure("Unexpected Index: " + index);
            }
        }

        private static Field.Store GetStore(Attributes.Store store)
        {
            switch (store)
            {
                case Attributes.Store.No:
                    return Field.Store.NO;
                case Attributes.Store.Yes:
                    return Field.Store.YES;
                default:
                    throw new AssertionFailure("Unexpected Store: " + store);
            }
        }

        public string LuceneTypeName(System.Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

    }
}