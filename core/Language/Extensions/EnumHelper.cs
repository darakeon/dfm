using System;
using System.Collections.Generic;

namespace DFM.Language.Extensions
{
	public class EnumHelper
	{
		public static Dictionary<TEnum, String> GetEnumNames<TEnum>(String section, String language)
		{
			var natures = new Dictionary<TEnum, String>();

			foreach (var item in Enum.GetValues(typeof(TEnum)))
			{
				var key = (TEnum)item;
				var value = PlainText.Site[
					section, language, item.ToString()
				].Text;

				natures.Add(key, value);
			}
			return natures;
		}
	}
}
