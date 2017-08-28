using System;
using System.IO;

namespace DFM.Multilanguage.Emails
{
	public class EmailLayout
	{
		private static readonly String path = Path.Combine(PlainText.MainPath, "EmailLayouts");

		public String this[SimpleTheme simpleTheme, EmailType emailType]
		{
			get
			{
				var masterPath = Path.Combine(path, $"{simpleTheme}Master.htm");
				var mainPath = Path.Combine(path, $"{emailType}.htm");

				var masterContent = getContent(masterPath);
				var mainContent = getContent(mainPath);

				return masterContent.Replace("{{Body}}", mainContent);
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