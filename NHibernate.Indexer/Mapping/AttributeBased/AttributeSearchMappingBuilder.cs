using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Lucene.Net.Analysis;
using NHibernate.Properties;
using NHibernate.Indexer.Attributes;

namespace NHibernate.Indexer.Mapping.AttributeBased
{
    using Type = System.Type;

    public class AttributeSearchMappingBuilder
    {
        private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof(AttributeSearchMappingBuilder));

        #region BuildContext class

        public class BuildContext
        {
            public BuildContext()
            {
                this.Processed = new List<System.Type>();
            }

            public DocumentMapping Root { get; set; }
            public IList<System.Type> Processed { get; private set; }
        }

        #endregion

        public DocumentMapping Build(Type type)
        {
            var documentMapping = new DocumentMapping(type)
            {
                IndexName = GetIndexName(type)
            };

            var context = new BuildContext
            {
                Root = documentMapping
            };

            BuildClass(documentMapping, true, string.Empty, context);

            return documentMapping;
        }

        private string GetIndexName(Type type)
        {
            var name = AttributeUtil.GetAttribute<IndexedAttribute>(type);
            return string.IsNullOrEmpty(name.Index) ? type.Name : name.Index;
        }

        private void BuildClass(
            DocumentMapping documentMapping, bool isRoot,
            string path, BuildContext context
        )
        {
            IList<System.Type> hierarchy = new List<System.Type>();
            System.Type currClass = documentMapping.MappedClass;

            do
            {
                hierarchy.Add(currClass);
                currClass = currClass.BaseType;
                // NB Java stops at null we stop at object otherwise we process the class twice
                // We also need a null test for things like ISet which have no base class/interface
            } while (currClass != null && currClass != typeof(object));

            for (int index = hierarchy.Count - 1; index >= 0; index--)
            {
                currClass = hierarchy[index];
                /**
                 * Override the default analyzer for the properties if the class hold one
                 * That's the reason we go down the hierarchy
                 */

                // NB Must cast here as we want to look at the type's metadata
                var localAnalyzer = GetAnalyzer(currClass);
                var analyzer = documentMapping.Analyzer ?? localAnalyzer;

                // NB As we are walking the hierarchy only retrieve items at this level
                var propertyInfos = currClass.GetProperties(
                    BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
                );
                foreach (var propertyInfo in propertyInfos)
                {
                    BuildProperty(documentMapping, propertyInfo, analyzer, isRoot, path, context);
                }

                var fields = currClass.GetFields(
                    BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
                );
                foreach (var fieldInfo in fields)
                {
                    BuildProperty(documentMapping, fieldInfo, analyzer, isRoot, path, context);
                }
            }
        }

        private void BuildProperty(
            DocumentMapping documentMapping, MemberInfo member, Analyzer parentAnalyzer,
            bool isRoot, string path, BuildContext context
        )
        {
            var analyzer = GetAnalyzer(member) ?? parentAnalyzer;
            var boost = GetBoost(member);
            var getter = GetGetterFast(documentMapping.MappedClass, member);

            var documentIdAttribute = AttributeUtil.GetAttribute<DocumentIdAttribute>(member);
            if (documentIdAttribute != null)
            {
                string documentIdName = documentIdAttribute.Name ?? member.Name;

                if (isRoot)
                {
                    documentMapping.DocumentId = new DocumentIdMapping(
                        documentIdName, member.Name, getter
                    ) { Boost = boost };
                }
                else
                {
                    // Components should index their document id
                    documentMapping.Fields.Add(new FieldMapping(
                        GetAttributeName(member, documentIdName),
                        getter
                    )
                    {
                        Store = Attributes.Store.Yes,
                        Index = Attributes.Index.UnTokenized,
                        Boost = boost
                    });
                }
            }

            var fieldAttributes = AttributeUtil.GetFields(member);
            if (fieldAttributes.Length > 0)
            {
                foreach (var fieldAttribute in fieldAttributes)
                {
                    var fieldAnalyzer = GetAnalyzerByType(fieldAttribute.Analyzer) ?? analyzer;
                    var field = new FieldMapping(
                        GetAttributeName(member, fieldAttribute.Name),
                        getter
                    )
                    {
                        Store = fieldAttribute.Store,
                        Index = fieldAttribute.Index,
                        Analyzer = fieldAnalyzer,
                        Boost = boost
                    };

                    documentMapping.Fields.Add(field);
                }
            }
        }

        /// <summary>
        /// Get the attribute name out of the member unless overridden by name
        /// </summary>
        /// <param name="member"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetAttributeName(MemberInfo member, string name)
        {
            return !string.IsNullOrEmpty(name) ? name : member.Name;
        }

        // ashmind: this method is a bit too hardcoded, on the other hand it does not make
        // sense to ask IPropertyAccessor to find accessor by name when we already have MemberInfo        
        private IGetter GetGetterFast(Type type, MemberInfo member)
        {
            if (member is PropertyInfo)
                return new BasicPropertyAccessor.BasicGetter(type, (PropertyInfo)member, member.Name);

            if (member is FieldInfo)
                return new FieldAccessor.FieldGetter((FieldInfo)member, type, member.Name);

            throw new ArgumentException("Can not get getter for " + member.GetType() + ".", "member");
        }

        private Analyzer GetAnalyzer(MemberInfo member)
        {
            var attribute = AttributeUtil.GetAttribute<AnalyzerAttribute>(member);
            if (attribute == null)
                return null;

            if (!typeof(Analyzer).IsAssignableFrom(attribute.Type))
            {
                throw new IndexException("Lucene analyzer not implemented by " + attribute.Type.FullName);
            }

            return GetAnalyzerByType(attribute.Type);
        }

        private Analyzer GetAnalyzerByType(Type analyzerType)
        {
            if (analyzerType == null)
                return null;

            try
            {
                return (Analyzer)Activator.CreateInstance(analyzerType);
            }
            catch
            {
                // TODO: See if we can get a tigher exception trap here
                throw new IndexException("Failed to instantiate lucene analyzer with type  " + analyzerType.FullName);
            }
        }

        private static System.Type GetMemberTypeOrGenericArguments(MemberInfo member)
        {
            Type type = GetMemberType(member);
            if (type.IsGenericType)
            {
                Type[] arguments = type.GetGenericArguments();

                // if we have more than one generic arg, we assume that this is a map and return its value
                return arguments[arguments.Length - 1];
            }

            return type;
        }

        private static Type GetMemberTypeOrGenericCollectionType(MemberInfo member)
        {
            Type type = GetMemberType(member);
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        private static Type GetMemberType(MemberInfo member)
        {
            PropertyInfo info = member as PropertyInfo;
            return info != null ? info.PropertyType : ((FieldInfo)member).FieldType;
        }

        private float? GetBoost(ICustomAttributeProvider member)
        {
            if (member == null)
                return null;

            var boost = AttributeUtil.GetAttribute<BoostAttribute>(member);
            if (boost == null)
                return null;

            return boost.Value;
        }
    }
}
