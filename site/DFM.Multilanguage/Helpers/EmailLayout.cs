using System;
using System.IO;
using System.Text;

namespace DFM.Multilanguage.Helpers
{
	public class EmailLayout
	{
		private static readonly String path = Path.Combine(PlainText.MainPath, "EmailLayouts");

		public String this[String language, String layout]
		{
			get
			{
				var filePath =
					Path.Combine(path, language, layout + ".htm");

				if (!File.Exists(filePath))
					return "";

				var content = new StringBuilder();

				using (var reader = new StreamReader(filePath))
				{
					while (!reader.EndOfStream)
					{
						content.AppendLine(reader.ReadLine());
					}
				}

				return content.ToString();
			}
		}


	}
}