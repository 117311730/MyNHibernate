using System;
using System.Reflection;
using NHibernate.Indexer.Attributes;

namespace NHibernate.Indexer.Mapping.AttributeBased
{
    public class AttributeUtil
    {
        //private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof(AttributeUtil));

        public static T GetAttribute<T>(ICustomAttributeProvider member) where T : Attribute
        {
            object[] objects = member.GetCustomAttributes(typeof(T), true);
            if (objects.Length == 0)
            {
                return null;
            }

            return (T)objects[0];
        }

        public static bool HasAttribute<T>(ICustomAttributeProvider member) where T : Attribute
        {
            return member.IsDefined(typeof(T), true);
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider member)
            where T : class
        {
            return GetAttributes<T>(member, true);
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider member, bool inherit)
            where T : class
        {
            return (T[])member.GetCustomAttributes(typeof(T), inherit);
        }

        public static FieldAttribute GetField(MemberInfo member)
        {
            FieldAttribute attribute = GetAttribute<FieldAttribute>(member);
            if (attribute == null)
            {
                return null;
            }

            attribute.Name = attribute.Name ?? member.Name;
            return attribute;
        }

        public static FieldAttribute[] GetFields(MemberInfo member)
        {
            FieldAttribute[] attribs = GetAttributes<FieldAttribute>(member);
            if (attribs != null)
            {
                foreach (FieldAttribute attribute in attribs)
                {
                    attribute.Name = attribute.Name ?? member.Name;
                }
            }

            return attribs;
        }

    }
}
