using System;
using System.IO;

namespace DFM.Multilanguage.Helpers
{
	public class EmailLayout
	{
		private static readonly String path = Path.Combine(PlainText.MainPath, "EmailLayouts");

		public String this[String layout]
		{
			get
			{
				var masterPath = Path.Combine(path, "master.htm");
				var mainPath = Path.Combine(path, layout + ".htm");

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