using System;

namespace DFM.Generic
{
	public static class StringExtension
	{
		public static String BreakLines(this String original)
		{
			return original
				.Replace(Environment.NewLine, "<br />")
				.Replace("\n", "<br />");
		}

		public static String Format(this String format, params object[] args)
		{
			return String.Format(format, args);
		}
	}
}
