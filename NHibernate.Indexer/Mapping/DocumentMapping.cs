using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;

namespace NHibernate.Indexer.Mapping
{
    using Type = System.Type;

    public class DocumentMapping
    {
        public DocumentMapping(Type mappedClass)
        {
            this.MappedClass = mappedClass;

            this.Fields = new List<FieldMapping>(); ;

        }

        public Type MappedClass { get; private set; }
        public string IndexName { get; set; }
        public Analyzer Analyzer { get; set; }

        public DocumentIdMapping DocumentId { get; set; }
        public IList<FieldMapping> Fields { get; private set; }
    }
}
