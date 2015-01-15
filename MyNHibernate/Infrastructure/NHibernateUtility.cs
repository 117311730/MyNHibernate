using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace MyNHibernate.Infrastructure
{
    public class NHibernateUtility
    {
        private static Configuration _Configure;
        private static ISessionFactory _SessionFactory;

        public void Initialize()
        {
            Configure();
            _SessionFactory = _Configure.BuildSessionFactory();
        }

        public static ISessionFactory GetSessionFactory()
        {
            return _SessionFactory;
        }

        private void Configure()
        {
            _Configure = new Configuration().Configure();
            _Configure.AddMapping(GetMappings());
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();
            //mapper.AddMappings(Assembly.GetAssembly(typeof(CityMapping)).GetExportedTypes());
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == "MyNHibernate.Models.Mappings"
                    select t;
            mapper.AddMappings(q);
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }
}