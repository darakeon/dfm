using System;

namespace DFM.Generic
{
	public static class NumericExtension
	{
		public static Int32 ToCents(this Double value)
		{
			return (Int32)(value * 100);
		}

		public static Double ToVisual(this Int32 value)
		{
			return value / 100.0;
		}

		public static Int32? ToCents(this Double? value)
		{
			return value == null ? null : (Int32?)(value * 100);
		}

		public static Double? ToVisual(this Int32? value)
		{
			return value == null ? null : value / 100.0;
		}

	}
}
