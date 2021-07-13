using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

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

		public static String Format(this String format, params Object[] args)
		{
			return String.Format(format, args);
		}

		public static String ToLower(this Object obj)
		{
			return obj.ToString()?.ToLower();
		}

		public static String ReplaceRegex(
			this String text,
			[RegexPattern] String pattern,
			String replacement
		)
		{
			return Regex.Replace(text, pattern, replacement);
		}
	}
}
