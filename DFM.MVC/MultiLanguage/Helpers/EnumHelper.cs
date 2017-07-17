using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class EnumHelper
    {
        public static Dictionary<TEnum, String> GetEnumNames<TEnum>()
        {
            var natures = new Dictionary<TEnum, String>();

            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                var key = (TEnum)item;
                var value = PlainText.Dictionary[item];

                natures.Add(key, value);
            }
            return natures;
        }

        public static String GetEnumNamesConcat<T>()
        {
            var enumList = Enum.GetNames(typeof(T));
            var result = String.Empty;

            for (var e = 0; e < enumList.Length; e++)
            {
                result += PlainText.Dictionary[enumList[e]];

                switch (enumList.Length - e)
                {
                    case 1: break;
                    case 2: result += String.Format(" {0} ", PlainText.Dictionary["and"]); break;
                    default: result += ", "; break;
                }
            }

            return result;
        }
    }
}