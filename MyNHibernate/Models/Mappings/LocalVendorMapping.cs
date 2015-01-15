using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MyNHibernate.Models.Mappings
{
    public class LocalVendorMapping : ClassMapping<LocalVendor>
    {
        public LocalVendorMapping()
        {
            Lazy(true);
            Id(x => x.Vendorid, map => map.Generator(Generators.Identity));
            Property(x => x.Vendorname, map => map.NotNullable(true));
            Property(x => x.Vendortype, map => map.NotNullable(true));
            Property(x => x.Logopath);
            Property(x => x.Thumbnailpath);
            Property(x => x.Telephonenumber);
            Property(x => x.Telephone1);
            Property(x => x.Telephone2);
            Property(x => x.Telephone3);
            Property(x => x.Contact);
            Property(x => x.Address);
            Property(x => x.Email);
            Property(x => x.Qqnumber);
            Property(x => x.Msn);
            Property(x => x.Website);
            Property(x => x.Introduction);
            Property(x => x.Productintroduction);
            Property(x => x.Reviewcount);
            Property(x => x.Overallprice);
            Property(x => x.Overallrating);
            Property(x => x.Isrecommend);
            Property(x => x.Popularityindex);
            Property(x => x.Createddate, map => map.NotNullable(true));
            Property(x => x.Createdby);
            Property(x => x.Updateddate);
            Property(x => x.Updatedby);
            Property(x => x.Isactive);
            Property(x => x.Overallpricevalue);
            Property(x => x.Uploadfolder);
            Property(x => x.Recommendindex);
            Property(x => x.Selective);
            Property(x => x.Productintroduction2);
            Property(x => x.Productintroduction3);
            Property(x => x.Productintroduction4);
            Property(x => x.Seoname);
            Property(x => x.Vendorcode, map => map.Unique(true));
            Property(x => x.Microblog);
            Property(x => x.Blog);
            Property(x => x.Webshop);
            Property(x => x.Keywords);
            Property(x => x.Dealerid);
            Property(x => x.Introductiontitle);
            Property(x => x.Introductionimg);
            Property(x => x.Largephoto);
            ManyToOne(x => x.Vendorcategory, map =>
            {
                map.Column("CategoryId");
                map.Cascade(Cascade.None);
            });

            ManyToOne(x => x.City, map =>
            {
                map.Column("CityId");
                map.Cascade(Cascade.None);
            });

            ManyToOne(x => x.District, map =>
            {
                map.Column("DistrictId");
                map.NotNullable(true);
                map.Cascade(Cascade.None);
            });
        }
    }
}