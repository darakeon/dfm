using System;
using System.Globalization;
using System.Linq;
using System.Text;
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

		public static String IntoUrl(this String original)
		{
			var characters = original
				.Normalize(NormalizationForm.FormD)
				.Where(notAccent)
				.ToArray();

			return new String(characters)
				.Normalize(NormalizationForm.FormC)
				.ReplaceRegex(@"[^\w]", "_")
				.ToLower();
		}

		private static Boolean notAccent(Char c)
		{
			return CharUnicodeInfo.GetUnicodeCategory(c)
				!= UnicodeCategory.NonSpacingMark;
		}

		public static String ToBase64(this String text)
		{
			return Convert.ToBase64String(
				Encoding.UTF8.GetBytes(text)
			);
		}
	}
}
