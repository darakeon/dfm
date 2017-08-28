using System;
using DFM.Entities;

namespace DFM.MVC.Helpers.Extensions
{
	public static class MonthExtension
	{
		public static Int32 Url(this Month month)
		{
			return month.Year.Time*100 + month.Time;
		}
	}
}