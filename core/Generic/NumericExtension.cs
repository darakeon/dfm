using System;

namespace DFM.Generic
{
	public static class NumericExtension
	{
		public static Int32 ToCents(this Decimal value)
		{
			return (Int32) Math.Round(value * 100, 0);
		}

		public static Decimal ToVisual(this Int32 value)
		{
			return value / 100m;
		}

		public static Int32? ToCents(this Decimal? value)
		{
			return value?.ToCents();
		}

		public static Decimal? ToVisual(this Int32? value)
		{
			return value?.ToVisual();
		}
	}
}
