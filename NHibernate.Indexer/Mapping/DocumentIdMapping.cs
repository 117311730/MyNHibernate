using System;
using System.Collections.Generic;
using NHibernate.Mapping;
using NHibernate.Properties;

namespace NHibernate.Indexer.Mapping
{
    public class DocumentIdMapping : PropertyMappingBase
    {
        public const string DefaultIndexedName = "id";

        public DocumentIdMapping(IGetter getter)
            : this(DefaultIndexedName, RootClass.DefaultIdentifierColumnName, getter)
        {
        }

        public DocumentIdMapping(string name, IGetter getter)
            : this(name, RootClass.DefaultIdentifierColumnName, getter)
        {
        }

        public DocumentIdMapping(string name, string propertyName, IGetter getter)
            : base(getter)
        {
            this.Name = name;
            this.PropertyName = propertyName;
        }

        public string Name { get; private set; }
        public string PropertyName { get; private set; }
        public float? Boost { get; set; }
    }
}
