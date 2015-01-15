using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MyNHibernate.Models.Mapping
{
    public class CityMapping : ClassMapping<City>
    {
        public CityMapping()
        {
            Table("City");
            Id("CityId", map => map.Generator(Generators.Identity));
            Property(city => city.CityName, map => map.Column("CityName"));
            Property(city => city.SEOName, map => map.Column("SEOName"));
            Property(city => city.CityCode, map => map.Column("CityCode"));
            Property(city => city.DisplayOrder, map => map.Column("DisplayOrder"));
        }
    }
}