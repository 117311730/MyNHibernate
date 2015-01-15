using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNHibernate.Models
{
    public class VendorCategory
    {
        public virtual int CategoryId { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual string ThumbnailPath { get; set; }
        public virtual int? DisplayOrder { get; set; }
        public virtual string Description { get; set; }
        public virtual string SeoName { get; set; }
        public virtual int? ParentId { get; set; }
        public virtual string CategoryCode { get; set; }

        public virtual IList<LocalVendor> LocalVendor { get; set; }
    }
}