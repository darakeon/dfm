using System;

namespace DFM.Multilanguage.Helpers
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

	}
}