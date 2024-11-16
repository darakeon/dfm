using System;
using System.Collections.Generic;

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

		public static String FileSize(this Int32 value)
		{
			return value.fileSize(0);
		}

		private static IList<String> dimensions =
			new List<String>{"B", "KB", "MB", "GB", "TB"};

		private static String fileSize(this Int32 value, Int32 dimension)
		{
			if (value < 1024 || dimension == dimensions.Count - 1)
				return $"{value} {dimensions[dimension]}";

			return (value/1024).fileSize(dimension+1);
		}
	}
}
