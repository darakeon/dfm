using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Api.Helpers
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
		public static SelectItem<String, Int32> SelectItem<TEnum>(
			this TEnum value, Translator translator, ServiceAccess service
		) where TEnum : IConvertible
		{
			return new(
				translator[value.ToString(service.Current.Culture)],
				Convert.ToInt32(value)
			);
		}

		public static IList<SelectItem<string, int>> SelectItem<TEnum>(
			Translator translator, ServiceAccess service
		) where TEnum : IConvertible
		{
			return Enum.GetValues(typeof(TEnum))
					.Cast<TEnum>()
					.Select(n => n.SelectItem(translator, service))
					.ToList();
		}

	}
}
