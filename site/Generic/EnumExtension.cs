using System;
using System.Collections;
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
			return values<T>().ToList();
		}

		public static List<T> AllExcept<T>(params T[] remove)
		{
			return values<T>()
				.Where(e => !remove.Contains(e))
				.ToList();
		}

		private static IEnumerable<T> values<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}
