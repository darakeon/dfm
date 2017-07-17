using System;
using System.Linq;

namespace DFM.MVC.Helpers
{
    public static class StringExtension
    {
        public static String Capitalize(this String @string)
        {
            return @string == null
                       ? null
                       : @string.First().ToString().ToUpper()
                         + @string.Substring(1).ToLower();
        }
    }
}