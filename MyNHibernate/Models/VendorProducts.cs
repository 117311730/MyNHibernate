using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNHibernate.Models
{
    public class VendorProducts
    {
        public virtual int ProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Tags { get; set; }
        public virtual string Descrtiption { get; set; }
        public virtual string Thumbnail { get; set; }
        public virtual int ParentId { get; set; }
        public virtual int VendorId { get; set; }
        public virtual int TemplateId { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string ModiifiedBy { get; set; }
    }
}