using System;
using System.IO;
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

				var basePath = Path.Combine(path, $"{theme}Base.htm");
				var baseContent = getContent(basePath);

				return baseContent
					.Replace("{{Body}}", mainContent)
					.Replace("{{TokenToNotHide}}", Token.New());
			}
		}

		private static string getContent(string filePath)
		{
			if (!File.Exists(filePath))
				return "";

			return File.ReadAllText(filePath);
		}
	}
}
