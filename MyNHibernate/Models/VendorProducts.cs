using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Indexer.Attributes;

namespace MyNHibernate.Models
{
    [Indexed(Index = "VendorProducts")]
    public class VendorProducts
    {
        [DocumentId]
        public virtual int ProductId { get; set; }
        [Field(Index.Tokenized, Store = Store.Yes)]
        public virtual string Name { get; set; }
        [Field(Index.No, Store = Store.Yes)]
        public virtual decimal Price { get; set; }
        [Field(Index.Tokenized, Store = Store.No)]
        public virtual string Tags { get; set; }
        [Field(Index.Tokenized, Store = Store.No)]
        public virtual string Description { get; set; }
         [Field(Index.No, Store = Store.Yes)]
        public virtual string Thumbnail { get; set; }
        public virtual int ParentId { get; set; }
        public virtual int VendorId { get; set; }
        public virtual int TemplateId { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
    }
}