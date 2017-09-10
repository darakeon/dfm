using System;

namespace DFM.Generic
{
	public static class StringExtension
	{
		public static String BreakLines(this String original)
		{
			return original.Replace(Environment.NewLine, "<br />");
		}
	}
}
