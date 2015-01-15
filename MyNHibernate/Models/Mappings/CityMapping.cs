using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
namespace MyNHibernate.Models.Mappings
{
    public class CityMapping : ClassMapping<City>
    {
        public CityMapping()
        {
            Lazy(true);
            Id(e => e.CityId, map => map.Generator(Generators.Identity));
            Property(e => e.CityName, map => map.NotNullable(true));
            Property(e => e.SEOName);
            Property(e => e.CityCode);
            Property(e => e.DisplayOrder, map => map.NotNullable(true));
            Bag(x => x.District, colmap => { colmap.Key(x => x.Column("CityID")); colmap.Inverse(true); }, map => { map.OneToMany(); });
            Set(x => x.LocalVendor, colmap => { colmap.Key(x => x.Column("CityId")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}