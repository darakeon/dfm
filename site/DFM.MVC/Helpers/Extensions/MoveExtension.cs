using System;
using DFM.Entities;

namespace DFM.MVC.Helpers.Extensions
{
	public static class MoveExtension
	{
		public static Int32 ReportUrl(this Move month)
		{
			return month.Year*100 + month.Month;
		}
	}
}
