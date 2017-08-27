using System;

namespace DFM.Generic
{
	public static class NumericExtension
	{
		public static Int32 ToCents(this Double value)
		{
            return (Int32)(Math.Round(value * 100, 0));
		}

		public static Double ToVisual(this Int32 value)
		{
			return value / 100.0;
		}

		public static Int32? ToCents(this Double? value)
		{
		    return value.HasValue 
                ? value.Value.ToCents()
                : default(Int32?);
		}

		public static Double? ToVisual(this Int32? value)
		{
			return value.HasValue
                ? value.Value.ToVisual()
                : default(Double?);
		}

	}
}
