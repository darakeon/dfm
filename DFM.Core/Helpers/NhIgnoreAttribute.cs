using System;
using System.Linq;
using System.Reflection;

namespace DFM.Core.Helpers
{
    public class NhIgnoreAttribute : Attribute
    {
        public static bool HasMe(PropertyInfo propertyInfo)
        {
            return propertyInfo
                .GetCustomAttributes(false)
                .Any(
                    a => a.GetType() == typeof(NhIgnoreAttribute)
                );
        }
    }
}
