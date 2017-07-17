using System;
using System.Linq;
using System.Reflection;

namespace DFM.Core.Helpers
{
    internal class NhIgnoreAttribute : Attribute
    {
        internal static bool HasMe(PropertyInfo propertyInfo)
        {
            return propertyInfo
                .GetCustomAttributes(false)
                .Any(
                    a => a.GetType() == typeof(NhIgnoreAttribute)
                );
        }
    }
}
