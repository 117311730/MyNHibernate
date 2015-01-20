using System;
using System.Collections.Generic;

using Lucene.Net.Analysis;
using NHibernate.Properties;

namespace NHibernate.Indexer.Mapping {
    public class FieldMapping : PropertyMappingBase
    {
        public FieldMapping(string name, IGetter getter) : base(getter)
        {
            this.Name = name;

            this.Store = Attributes.Store.No;
            this.Index = Attributes.Index.Tokenized;
        }

        public string Name              { get; private set; }
        public Attributes.Store Store   { get; set; }
        public Attributes.Index Index   { get; set; }
        public Analyzer Analyzer        { get; set; }
    }
}
