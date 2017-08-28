using System;

namespace DFM.Generic
{
	public static class DateTimeExtension
	{
		public static String ToShortDateString(this DateTime? dateTime)
		{
			return dateTime?.ToShortDateString();
		}
	}
}
