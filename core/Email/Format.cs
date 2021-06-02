using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
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

		public static Format MoveNotification(User user)
		{
			return new(user, EmailType.MoveNotification, EmailType.MoveNotification);
		}

		public static Format SecurityAction(User user, SecurityAction securityAction)
		{
			return new(user, EmailType.SecurityAction, securityAction);
		}

		public static Format UserRemoval(User user, RemovalReason removalReason)
		{
			return new(user, EmailType.RemovalReason, removalReason);
		}

		public static Format PurgeNotice(User user, RemovalReason removalReason)
		{
			return new(user, EmailType.PurgeNotice, removalReason);
		}

		private Format(User user, EmailType type, Object layoutType)
		{
			var config = user.Config;
			var language = config.Language;
			var theme = config.Theme;

			var layoutName = layoutType.ToString();
			var replaces = getReplaces(type.ToString(), layoutName, language);

			Subject = replaces["Subject"];
			Layout = FormatEmail(theme, type, replaces);
		}

		private static Dictionary<String, String> getReplaces(
			String type, String subtype, String language
		)
		{
			var sections = new[] {type, subtype};
			var replaces = PlainText.Email[sections, language];
			return replaces.ToDictionary(p => p.Name, p => p.Text);
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
