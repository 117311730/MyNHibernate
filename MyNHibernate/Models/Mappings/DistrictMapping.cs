using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MyNHibernate.Models.Mappings
{
    public class DistrictMapping : ClassMapping<District>
    {
        public DistrictMapping()
        {
            Lazy(true);
            Id(x => x.DistrictId, map => map.Generator(Generators.Identity));
            Property(x => x.DistrictName, map => map.NotNullable(true));
            Property(x => x.DisplayOrder, map => map.NotNullable(true));
            ManyToOne(x => x.City, map =>
            {
                map.Column("CityID");
                map.Cascade(Cascade.None);
            });

            Bag(x => x.LocalVendor, colmap => { colmap.Key(x => x.Column("DistrictId")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}