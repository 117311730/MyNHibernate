using System.Collections.Generic;
namespace NHibernate.Search
{
    public interface IFullTextSession
    {
        IFullTextSession SetSession(ISession session);
        int ResultSize { get; }
        IEnumerable<TEntity> Search<TEntity>(string keyWord) where TEntity : new();
    }
}