using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;

namespace NHibernate.Indexer.Mapping
{
    using Type = System.Type;

    public interface IIndexMapping
    {
        ICollection<DocumentMapping> Build(Configuration cfg);
    }
}
