using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Core.Helpers
{
    public static class StringExtension
    {
        public static String Capitalize(this String str)
        {
            return str == null
                       ? null
                       : str.First().ToString().ToUpper()
                         + str.Substring(1).ToLower();
        }

        public static String Format(this String str, IDictionary<String, String> replaces)
        {
            return replaces.Aggregate(str, (current, replace) =>
                        current.Replace("{{" + replace.Key + "}}", replace.Value));
        }

    }
}