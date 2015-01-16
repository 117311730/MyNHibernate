using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MyNHibernate.Models.Mappings
{
    public class VendorProductsMapping : ClassMapping<VendorProducts>
    {
        public VendorProductsMapping()
        {
            Schema("dbo");
            Lazy(true);
            Id(e => e.ProductId, map => map.Generator(Generators.Identity));
            Property(e => e.Name, map => map.NotNullable(true));
            Property(e => e.Price, map => map.NotNullable(true));
            Property(e => e.Tags, map => map.NotNullable(true));
            Property(e => e.Description, map => map.NotNullable(true));
            Property(e => e.Thumbnail, map => map.NotNullable(true));
            Property(e => e.ProductId, map => map.NotNullable(true));
            Property(e => e.VendorId, map => map.NotNullable(true));
            Property(e => e.TemplateId, map => map.NotNullable(true));
            Property(e => e.SortOrder, map => map.NotNullable(true));
            Property(e => e.CreatedDate, map => map.NotNullable(true));
            Property(e => e.CreatedBy, map => map.NotNullable(true));
            Property(e => e.ModifiedDate);
            Property(e => e.ModifiedBy);
        }
    }
}