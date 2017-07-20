using System;
using System.Collections.Generic;

namespace DFM.Multilanguage.Helpers
{
    public class EnumHelper
    {
        public static Dictionary<TEnum, String> GetEnumNames<TEnum>(String section, String language)
        {
            var natures = new Dictionary<TEnum, String>();

            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                var key = (TEnum)item;
                var value = PlainText.Dictionary[section, language, item.ToString()];

                natures.Add(key, value);
            }
            return natures;
        }

        public static String GetEnumNamesConcat<T>(String section, String language)
        {
            var enumList = Enum.GetNames(typeof(T));
            var result = String.Empty;

            for (var e = 0; e < enumList.Length; e++)
            {
                result += PlainText.Dictionary[section, language, enumList[e]];

                switch (enumList.Length - e)
                {
                    case 1: break;
                    case 2: result += String.Format(" {0} ", PlainText.Dictionary[section, language, "and"]); break;
                    default: result += ", "; break;
                }
            }

            return result;
        }
    }
}