using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNHibernate.Models
{
    public class City
    {
        public virtual int CityId { get; set; }
        public virtual string CityName { get; set; }
        public virtual string SEOName { get; set; }
        public virtual string CityCode { get; set; }
        public virtual int DisplayOrder {get;set;}

        public virtual IList<District> District { get; set; }
        public virtual ISet<LocalVendor> LocalVendor { get; set; }
    }
}