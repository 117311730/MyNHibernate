using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MyNHibernate.Models.Mappings
{
    public class VendorCategoryMapping : ClassMapping<VendorCategory>
    {
        public VendorCategoryMapping()
        {
            Lazy(true);
            Id(x => x.CategoryId, map => map.Generator(Generators.Identity));
            Property(x => x.CategoryName, map => map.NotNullable(true));
            Property(x => x.ThumbnailPath);
            Property(x => x.DisplayOrder);
            Property(x => x.Description);
            Property(x => x.SeoName);
            Property(x => x.ParentId);
            Property(x => x.CategoryCode, map => map.Unique(true));
            Bag(x => x.LocalVendor, colmap => { colmap.Key(x => x.Column("CategoryId")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            
        }
    }
}