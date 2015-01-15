using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyNHibernate.Models
{
    public class LocalVendor
    {
        public virtual long Vendorid { get; set; }
        public virtual VendorCategory Vendorcategory { get; set; }
        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual string Vendorname { get; set; }
        public virtual int Vendortype { get; set; }
        public virtual string Logopath { get; set; }
        public virtual string Thumbnailpath { get; set; }
        public virtual string Telephonenumber { get; set; }
        public virtual string Telephone1 { get; set; }
        public virtual string Telephone2 { get; set; }
        public virtual string Telephone3 { get; set; }
        public virtual string Contact { get; set; }
        public virtual string Address { get; set; }
        public virtual string Email { get; set; }
        public virtual string Qqnumber { get; set; }
        public virtual string Msn { get; set; }
        public virtual string Website { get; set; }
        public virtual string Introduction { get; set; }
        public virtual string Productintroduction { get; set; }
        public virtual int? Reviewcount { get; set; }
        public virtual int? Overallprice { get; set; }
        public virtual int? Overallrating { get; set; }
        public virtual bool? Isrecommend { get; set; }
        public virtual int? Popularityindex { get; set; }
        public virtual DateTime Createddate { get; set; }
        public virtual string Createdby { get; set; }
        public virtual DateTime? Updateddate { get; set; }
        public virtual string Updatedby { get; set; }
        public virtual bool? Isactive { get; set; }
        public virtual int? Overallpricevalue { get; set; }
        public virtual string Uploadfolder { get; set; }
        public virtual int? Recommendindex { get; set; }
        public virtual int? Selective { get; set; }
        public virtual string Productintroduction2 { get; set; }
        public virtual string Productintroduction3 { get; set; }
        public virtual string Productintroduction4 { get; set; }
        public virtual string Seoname { get; set; }
        public virtual string Vendorcode { get; set; }
        public virtual string Microblog { get; set; }
        public virtual string Blog { get; set; }
        public virtual string Webshop { get; set; }
        public virtual string Keywords { get; set; }
        public virtual long? Dealerid { get; set; }
        public virtual string Introductiontitle { get; set; }
        public virtual string Introductionimg { get; set; }
        public virtual string Largephoto { get; set; }
    }
}