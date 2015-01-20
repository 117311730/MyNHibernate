using System;

namespace NHibernate.Indexer.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IndexedAttribute : Attribute
    {
        private string index = string.Empty;

        public string Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}