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

	}
}
