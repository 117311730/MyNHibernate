using System.Collections.Generic;
using NHibernate.Cfg;

namespace NHibernate.Indexer.Mapping
{

    public interface IIndexMapping
    {
        ICollection<DocumentMapping> Build(Configuration cfg);
    }
}
