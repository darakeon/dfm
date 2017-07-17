using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFM.Core.Entities.Bases;
using NHibernate.Linq;

namespace DFM.Core.Helpers
{
    public class Relations<T>
    {
        public static IEnumerable<String> GetDeeper(Int32 deep)
        {
            var list = new List<String>();

            for (var d = 0; d <= deep; d++)
            {
                list.AddRange(getParentEntities(d));
            }

            return list;
        }

        private static String[] getParentEntities(Int32 deep = 0, Type type = null, String prefix = null)
        {
            if (type == null)
                type = typeof(T);

            if (deep == 0)
                return getOneEntity(type, prefix);

            if (!String.IsNullOrEmpty(prefix))
                prefix += ".";

            var relations = entitiesOf(type)
                .Select(p =>
                    getParentEntities(deep - 1, p.PropertyType, prefix + p.PropertyType.Name)
                )
                .ToArray();

            var list = new List<String>();

            relations.ForEach(list.AddRange);

            return list.ToArray();
        }

        private static String[] getOneEntity(Type type, String prefix = null)
        {
            if (!String.IsNullOrEmpty(prefix))
                prefix += ".";

            return entitiesOf(type)
                .Select(p => prefix + p.PropertyType.Name)
                .ToArray();
        }

        private static IEnumerable<PropertyInfo> entitiesOf(Type type)
        {
            return type.GetProperties()
                .Where(p =>
                    typeof(IEntity).IsAssignableFrom(p.PropertyType)
                       && !NhIgnoreAttribute.HasMe(p)
                );
        }
    }
}
