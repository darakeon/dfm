using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Language;
using DFM.Language.Emails;
using DFM.Entities.Enums;
using DFM.Generic;
using Keon.Util.Extensions;

namespace DFM.Email
{
	public class Format
	{
		public String Subject { get; set; }
		public String Layout { get; set; }

		public static Format MoveNotification(String language, Theme theme)
		{
			return new Format(language, theme, EmailType.MoveNotification, EmailType.MoveNotification);
		}

		public static Format SecurityAction(String language, Theme theme, SecurityAction securityAction)
		{
			return new Format(language, theme, EmailType.SecurityAction, securityAction);
		}

		private Format(String language, Theme theme, EmailType type, object layoutType)
		{
			var layoutName = layoutType.ToString();

			Subject = PlainText.Email[layoutName, language, "Subject"];

			var replaces = PlainText.Email[layoutName, language]
				.ToDictionary(p => p.Name, p => p.Text);

			Layout = FormatEmail(theme, type, replaces);
		}

		public static String FormatEmail<T>(Theme theme, EmailType type, IDictionary<String, T> replaces)
		{
			return PlainText.Html[theme, type]
				.Format(
					replaces.ToDictionary(
						p => p.Key,
						p => p.Value?.ToString()
					)
				);
		}
	}
}
