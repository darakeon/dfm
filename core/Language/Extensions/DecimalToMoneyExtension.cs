using System;

namespace DFM.Language.Extensions
{
	public static class DecimalToMoneyExtension
	{
		public static String ToMoney(this Decimal number, String language, Boolean differentIfZero = false)
		{
			if (differentIfZero && number == 0)
			{
				return "---";
			}

			var format = PlainText.GetNumberFormat(language);

			return number.ToString("#,###0.00", format);
		}

		public static String ToMoney(this Decimal? number, String language, Boolean differentIfZero = false)
		{
			return number.HasValue
				? number.Value.ToMoney(language, differentIfZero)
				: "---";
		}
	}
}
