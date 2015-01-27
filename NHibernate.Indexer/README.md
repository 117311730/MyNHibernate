# NHibernate Lucene.net auto index

## For use
```c#
    [Indexed]
    public class VendorProducts
    {
        [DocumentId]
        public virtual int ProductId { get; set; }

        [Field(Index.Tokenized, Store = Store.Yes)]
        [Boost(2)]
        public virtual string Name { get; set; }

        [Field(Index.No, Store = Store.Yes)]
        public virtual decimal Price { get; set; }

        [Field(Index.Tokenized, Store = Store.No)]
        public virtual string Tags { get; set; }
    }
    

    using (var session = sessionFactory.OpenSession())
    {
        Indexer.CreateIndex(cfg, session, indexPath)
               .Build();
    }
```