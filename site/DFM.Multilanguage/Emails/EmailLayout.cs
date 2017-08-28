using System;
using System.IO;
using DK.Generic.Extensions;

namespace DFM.Multilanguage.Emails
{
	public class EmailLayout
	{
		private static readonly String path = Path.Combine(PlainText.MainPath, "EmailLayouts");

		public String this[SimpleTheme simpleTheme, EmailType emailType]
		{
			get
			{
				var mainPath = Path.Combine(path, $"{emailType}.htm");
				var mainContent = getContent(mainPath);

				if (emailType < 0)
					return mainContent;

				var masterPath = Path.Combine(path, $"{simpleTheme}Master.htm");
				var masterContent = getContent(masterPath);

				return masterContent
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