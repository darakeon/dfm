using System;

namespace DFM.MVC.Helpers.Extensions
{
	public static class IntExtension
	{
		public static Int16 ForceBetween(this Int16 number, Int16 min, Int16 max)
		{
			return number > max ? max
				: number < min ? min
				: number;
		}
	}
}
