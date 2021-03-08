using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Generic
{
	public static class EnumX
	{
		public static T Parse<T>(String value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		public static List<T> AllValues<T>()
		{
			return Enum.GetValues(typeof(T))
				.Cast<T>()
				.ToList();
		}
	}
}
