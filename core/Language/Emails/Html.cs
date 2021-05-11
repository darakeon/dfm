using System;
using System.IO;
using DFM.Generic;
using Keon.Util.Extensions;

namespace DFM.Language.Emails
{
	public class Html
	{
		private static readonly String path = Path.Combine(PlainText.CurrentPath, "Email");

		public String this[Theme theme, EmailType emailType]
		{
			get
			{
				var mainPath = Path.Combine(path, $"{emailType}.htm");
				var mainContent = getContent(mainPath);

				if (emailType < 0)
					return mainContent;

				var basePath = Path.Combine(path, "Base.htm");
				var baseContent = getContent(basePath);

				var html = baseContent
					.Replace("{{Body}}", mainContent)
					.Replace("{{TokenToNotHide}}", Token.New());

				var themePath = Path.Combine(path, "theme.json");
				var themeJson = getContent(themePath);

				foreach (var (name, hex) in ThemeColorize.Get(themeJson, theme))
				{
					html = html.Replace($"{{{{{name}}}}}", $"#{hex}");
				}

				return html;
			}
		}

		private static String getContent(String filePath)
		{
			if (!File.Exists(filePath))
				return "";

			return File.ReadAllText(filePath);
		}
	}
}
