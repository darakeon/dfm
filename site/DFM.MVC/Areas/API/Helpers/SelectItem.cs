using System;
using System.Collections.Generic;
using System.Linq;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API.Helpers
{
	public class SelectItem<TText, TValue>
	{
		public SelectItem(TText text, TValue value)
		{
			Text = text;
			Value = value;
		}

		public TText Text { get; set; }
		public TValue Value { get; set; }

	}

	public static class SelectItemEnum
	{
		public static SelectItem<String, Int32> SelectItem<TEnum>(this TEnum value)
			where TEnum : IConvertible
		{
			return new SelectItem<String, Int32>(
				MultiLanguage.Dictionary[value.ToString(Service.Current.Culture)],
				Convert.ToInt32(value)
			);
		}

		public static IList<SelectItem<String, Int32>> SelectItem<TEnum>()
			where TEnum : IConvertible
		{
			return Enum.GetValues(typeof(TEnum))
					.Cast<TEnum>()
					.Select(n => n.SelectItem())
					.ToList();
		}

	}
}