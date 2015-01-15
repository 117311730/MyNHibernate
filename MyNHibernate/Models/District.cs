using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNHibernate.Models
{
    public class District
    {
        public virtual int DistrictId { get; set; }
        public virtual City City { get; set; }
        public virtual string DistrictName { get; set; }
        public virtual int DisplayOrder { get; set; }

        public virtual IList<LocalVendor> LocalVendor { get; set; }
    }
}