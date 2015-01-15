using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MyNHibernate.Models.Mapping
{
    public class EntityClassMapping<TEntity> : ClassMapping<TEntity> where TEntity : class
    {
        public EntityClassMapping()
        {
            Id("Id", mapper => mapper.Generator(Generators.Identity));
            Table(TableName);
        }

        private string TableName
        {
            get { return typeof(TEntity).Name + "s"; }//todo
        }
    }
}