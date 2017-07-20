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

    }
}