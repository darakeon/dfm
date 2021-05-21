using System;
using DFM.Generic.Datetime;

namespace DFM.Exchange
{
	static class PrimitiveConverters
	{
		public static String ToCsv(this DateTime value)
		{
			return value.Universal();
		}

		public static String ToCsv(this Decimal value)
		{
			return value == 0 ? "" : value.ToString("F2");
		}
	}
}
