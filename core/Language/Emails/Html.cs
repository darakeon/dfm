using System;
using System.IO;
using DFM.Entities;
using DFM.Generic;
using Keon.Util.Extensions;

namespace DFM.Language.Emails
{
	public class Html
	{
		private static readonly String path = Path.Combine(PlainText.CurrentPath, "Email");

		public String this[Theme theme, EmailType type, String subtype, Misc misc]
		{
			get
			{
				var mainPath = Path.Combine(path, $"{type}.htm");
				var mainContent = getContent(mainPath);

				if (type < 0)
					return mainContent;

				var basePath = Path.Combine(path, "Base.htm");
				var baseContent = getContent(basePath);

				var miscPath = Path.Combine(path, "Misc.htm");
				var miscContent = misc != null
					? getContent(miscPath)
					: "";

				if (type.ToString() != subtype)
				{
					var subtypePath = Path.Combine(path, $"{subtype}.htm");
					if (File.Exists(subtypePath))
					{
						mainContent += getContent(subtypePath);
					}
				}

				var html = baseContent
					.Replace("{{Body}}", mainContent)
					.Replace("{{Misc}}", miscContent)
					.Replace("{{TokenToNotHide}}", Token.New());

				if (html.Contains("{{SecurityAction}}"))
				{
					var securityActionPath = Path.Combine(path, "SecurityAction.htm");
					var securityActionContent = getContent(securityActionPath);

					html = html.Replace("{{SecurityAction}}", securityActionContent);
				}

				var themePath = Path.Combine(path, "theme.json");
				var themeJson = getContent(themePath);

				foreach (var (name, value) in ThemeColorize.Get(themeJson, theme))
				{
					html = html.Replace($"{{{{{name}}}}}", $"{value}");
				}

				return html;
			}
		}

		private static String getContent(String filePath)
		{
			return File.ReadAllText(filePath);
		}
	}
}
