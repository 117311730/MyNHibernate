using NHibernate.Properties;

namespace NHibernate.Indexer.Mapping
{
    public class PropertyMappingBase
    {
        protected PropertyMappingBase(IGetter getter)
        {
            this.Getter = getter;
        }

        public IGetter Getter { get; private set; }
    }
}
