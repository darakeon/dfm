using System;
using System.IO;

namespace DFM.Multilanguage.Helpers
{
	public class EmailLayout
	{
		private static readonly String path = Path.Combine(PlainText.MainPath, "EmailLayouts");

		public String this[String language, String layout]
		{
			get
			{
				var filePath = Path.Combine(path, language, layout + ".htm");

				if (!File.Exists(filePath))
					return "";

				return File.ReadAllText(filePath);
			}
		}


	}
}