using System.Collections.Generic;
namespace NHibernate.Search
{
    public interface IFullTextSession
    {
        int ResultSize { get; }
        IEnumerable<TEntity> Search<TEntity>(string keyWord) where TEntity : new();
        IEnumerable<TEntity> Search<TEntity>(string keyWord, int topN) where TEntity : new();
        IEnumerable<TEntity> Search<TEntity>(string keyWord, int startRowIndex, int pageSize) where TEntity : new();
        IEnumerable<TEntity> Search<TEntity>(string keyWord, int topN, int startRowIndex, int pageSize) where TEntity : new();
    }
}